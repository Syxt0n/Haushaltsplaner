using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Calendars;

namespace Application.EFCore.Configurations;

public class CalendarConfigurations : IEntityTypeConfiguration<PersonalCalendar>
{
    public void Configure(EntityTypeBuilder<PersonalCalendar> builder)
    {
        builder.ToTable("calendars", "main");

        builder.Property(u => u.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.ActiveCulture)       
            .HasConversion(
                v => v.Name,
                v => CultureInfo.GetCultureInfo(v))
            .HasColumnName("activeCulture").IsRequired();
        
        builder.Property(c => c.ActiveWeek).HasColumnName("activeWeek").IsRequired();

        builder.HasMany(c => c.Appointments) // One Food has many Ingredients
            .WithOne() // One Ingredient belongs to one Food
            .HasForeignKey("id_calendar") // The foreign key in the Ingredients table is "id_food"
            .IsRequired();

        builder.HasKey(c => c.Id);
    }
}