using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Persons;

namespace Application.EFCore.Configurations;

public class PersonConfigurations : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons", "main");

        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Displayname).HasColumnName("displayname").IsRequired();
        
        builder.Property(p => p.Deleted).HasColumnName("deleted");

        builder.HasKey(p => p.Id);
    }
}