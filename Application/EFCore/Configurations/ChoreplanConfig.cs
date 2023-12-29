using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Choreplans;

namespace Application.EFCore.Configurations;

public class ChoreConfiguration : IEntityTypeConfiguration<Chore>
{
    public void Configure(EntityTypeBuilder<Chore> builder)
    {
        builder.ToTable("chores", "main");

        builder.Property<Guid>("id").HasColumnName("id").ValueGeneratedOnAdd().IsRequired();
        builder.Property(c => c.Name).HasColumnName("name").IsRequired();
        builder.Property(c => c.Description).HasColumnName("description").IsRequired();

        builder.HasKey("id");
        
    }
}

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("assignments", "main");

        builder.Property<Guid>("id_choreplan")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);


        builder.Property<Guid>("id_person")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);


        builder.Property<Guid>("id_chore")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);


        builder.Property<DayOfWeek>("day").HasColumnType("int");


        builder.HasOne<Choreplan>()
            .WithMany(a => a.Assignments)
            .HasForeignKey("id_choreplan")
            .IsRequired();

        builder.HasKey("id_choreplan", "id_person", "id_chore", "day");
    }
}

public class ChoreplanConfigurations : IEntityTypeConfiguration<Choreplan>
{
    public void Configure(EntityTypeBuilder<Choreplan> builder)
    {
        builder.ToTable("choreplans", "main");

        builder.Property(cp => cp.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();

        builder.Property(cp => cp.Week).HasColumnName("weeknumber").IsRequired();

        builder.HasMany(cp => cp.Assignments) // One Food has many Ingredients
            .WithOne() // One Ingredient belongs to one Food
            .HasForeignKey("id_choreplan") // The foreign key in the Ingredients table is "id_food"
            .IsRequired();

        builder.HasKey(cp => cp.Id);
    }
}