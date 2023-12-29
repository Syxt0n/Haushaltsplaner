using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Shared;
using Domain.Foods;

namespace Application.EFCore.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items", "main");

        builder.Property<Guid>("id")
            .HasColumnType("UUID")
            .ValueGeneratedOnAdd()
            .HasAnnotation("Key", 0);

        builder.HasKey("id");

        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
    }
}

// Configuration for Ingredient
// Configuration for Ingredient
public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("ingredients", "main");

        builder.Property<Guid>("id_food")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);

        builder.Property<Guid>("id_item")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);

        // Define composite key
        builder.HasKey("id_food", "id_item");

        builder.Property(i => i.Amount).HasColumnName("amount").IsRequired();

        // Define foreign key relationships using shadow properties
        builder.HasOne<Food>()
            .WithMany(f => f.Ingredients)
            .HasForeignKey("id_food")
            .IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey("id_item")
            .IsRequired();
    }
}



// Configuration for Food
public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ToTable("food", "main");

        builder.HasKey(f => f.Id); // Assuming the column name in the database is "id"

        builder.Property(f => f.Name).HasColumnName("name").IsRequired();

        builder.Property(f => f.Deleted).HasColumnName("deleted").IsRequired();

        builder.HasMany(f => f.Ingredients) // One Food has many Ingredients
            .WithOne() // One Ingredient belongs to one Food
            .HasForeignKey("id_food") // The foreign key in the Ingredients table is "id_food"
            .IsRequired();
    }
}