using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Mealplans;
using Domain.Persons;
using Domain.Foods;

namespace Application.EFCore.Configurations;

public class MealtypeConfiguration : IEntityTypeConfiguration<Mealtype>
{
    public void Configure(EntityTypeBuilder<Mealtype> builder)
    {
        builder.ToTable("mealtypes", "main");

        builder.Property<Guid>("id").HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(mt => mt.Value).HasColumnName("name").IsRequired();

        builder.HasKey("id");
    }
}

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.ToTable("meals", "main");

        builder.Property<Guid>("id_mealplan")
            .HasColumnName("id_mealplan")
            .HasAnnotation("Foreign Key", 0);

        builder.Property<Guid>("id_food")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);

        builder.Property<Guid>("id_person")
            .HasColumnType("uuid")
            .HasAnnotation("Foreing Key", 0);

        builder.Property<Guid>("id_mealtype")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);

        builder.Property<DayOfWeek>("day")
            .HasColumnType("int")
            .HasAnnotation("Foreign Key", 0);


        builder.HasOne<Mealplan>()
            .WithMany(m => m.Meals)
            .HasForeignKey("id_mealplan")
            .IsRequired();

        builder.HasOne<Food>()
            .WithMany()
            .HasForeignKey("id_food")
            .IsRequired();

        builder.HasOne<Person>()
            .WithMany()
            .HasForeignKey("id_person")
            .IsRequired();

        builder.HasOne<Mealtype>()
            .WithMany()
            .HasForeignKey("id_mealtype")
            .IsRequired();


        builder.HasKey("id_mealplan", "id_food", "id_person", "id_mealtype", "day");
    }
}

public class MealplanConfigurations : IEntityTypeConfiguration<Mealplan>
{
    public void Configure(EntityTypeBuilder<Mealplan> builder)
    {
        builder.ToTable("mealplans", "main");

        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Week).HasColumnName("weeknumber").IsRequired();

        builder.HasMany(mp => mp.Meals) // One Food has many Ingredients
            .WithOne() // One Ingredient belongs to one Food
            .HasForeignKey("id_mealplan") // The foreign key in the Ingredients table is "id_food"
            .IsRequired();

        builder.HasKey(p => p.Id);
    }
}