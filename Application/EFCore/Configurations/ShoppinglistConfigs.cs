using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Shared;
using Domain.Shoppinglists;

namespace Application.EFCore.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles", "main");

        builder.Property<Guid>("id_shoppinglist")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);

        builder.Property<Guid>("id_item")
            .HasColumnType("uuid")
            .HasAnnotation("Foreign Key", 0);

        // Define composite key
        builder.HasKey("id_shoppinglist", "id_item");

        builder.Property(i => i.Amount).HasColumnName("amount").IsRequired();

        // Define foreign key relationships using shadow properties
        builder.HasOne<Shoppinglist>()
            .WithMany(f => f.Articles)
            .HasForeignKey("id_shoppinglist")
            .IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey("id_item")
            .IsRequired();
    }
}

public class ShoppinglistConfiguration : IEntityTypeConfiguration<Shoppinglist>
{
    public void Configure(EntityTypeBuilder<Shoppinglist> builder)
    {
        builder.ToTable("shoppinglists", "main");

        builder.Property(sl => sl.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();

        builder.Property(sl => sl.Date).HasColumnName("date").IsRequired();

        builder.Property(sl => sl.Deleted).HasColumnName("deleted");

        builder.HasMany(sl => sl.Articles) // One Food has many Ingredients
            .WithOne() // One Ingredient belongs to one Food
            .HasForeignKey("id_shoppinglist") // The foreign key in the Ingredients table is "id_food"
            .IsRequired();

        builder.HasKey(sl => sl.Id); // Assuming the column name in the database is "id"
    }
}