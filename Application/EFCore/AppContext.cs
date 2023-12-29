using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain.Shared;
using Domain.Foods;
// using Domain.Shoppinglists;
// using Domain.Mealplans;
// using Domain.Choreplans;
// using Domain.Calendars;
// using Domain.Persons;

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}


#region Configs
// Configuration for Item
public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items", "main");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name).HasColumnName("name").IsRequired();

        builder.Property(i => i.Deleted).HasColumnName("deleted");
    }
}

// Configuration for Ingredient
public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("ingredients", "main");

        builder.HasKey(i => new { i.FoodId, i.ItemId });

        builder.Property(i => i.Amount).HasColumnName("amount").IsRequired();

        builder.HasOne(i => i.Food)
            .WithMany(f => f.Ingredients)
            .HasForeignKey(i => i.FoodId)
            .IsRequired();

        builder.OwnsOne(i => i.Item, item =>
        {
            item.Property(p => p.Name).HasColumnName("name").IsRequired();
        });
    }
}

// Configuration for Food
public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ToTable("food", "main");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name).HasColumnName("name").IsRequired();

        builder.Property(f => f.Deleted).HasColumnName("deleted");

        builder.OwnsMany(f => f.Ingredients, ingr =>
        {
            ingr.ToTable("ingredients", "main");
            ingr.HasKey(i => new { i.FoodId, i.ItemId });

            ingr.Property(i => i.Amount).HasColumnName("amount").IsRequired();

            ingr.OwnsOne(i => i.Item, item =>
            {
                item.Property(p => p.Name).HasColumnName("name").IsRequired();
            });
        });
    }
}

#endregion