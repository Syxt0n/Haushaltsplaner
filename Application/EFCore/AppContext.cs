using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Shared;
using Domain.Foods;

namespace Application.EFCore;

public class AppContext: DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Food> Foods { get; set; }
    private string DbPath = "";

    
    public AppContext()
    {
        DbPath = "Server=federlein.website;Database=hpEfCore;Trusted_Connection=True;";
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql($"Data Source={DbPath}");

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>();
        modelBuilder.Entity<Ingredient>();
        modelBuilder.Entity<Food>();
    }
}


#region Configs
// Configuration for Item
public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items", "main");

        builder.HasKey("id");

        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
    }
}

// Configuration for Ingredient
public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    // Configuration for Ingredient

    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("ingredients", "main");

        builder.HasKey("id_food"); // Assuming the column name in the database is "FoodId"

        builder.Property(i => i.Amount).HasColumnName("amount").IsRequired();

        builder.Property<Guid>("ItemId").HasColumnName("id_item").IsRequired(); // Assuming the column name in the database is "item_id"

        builder.HasOne<Food>()
            .WithOne()
            .HasForeignKey<Ingredient>("id_food")
            .IsRequired();

        builder.HasOne<Item>()
            .WithOne()
            .HasForeignKey<Ingredient>("ItemId")
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

#endregion