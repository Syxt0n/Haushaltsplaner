using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Users;

namespace Application.EFCore.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "main");

        builder.Property<Guid>("id")
            .HasColumnType("UUID")
            .ValueGeneratedOnAdd()
            .HasAnnotation("Key", 0);

        builder.Property(u => u.Username).HasColumnName("username").IsRequired();
        builder.Property(u => u.Password).HasColumnName("password").IsRequired();
        builder.HasOne(u => u.Person)
            .WithOne()
            .HasForeignKey("id_person")
            .IsRequired();

        builder.HasKey("id");
    }
}