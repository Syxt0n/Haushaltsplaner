using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Users;
using Domain.Persons;

namespace Application.EFCore.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "main");

        builder.HasKey(u => u.Id);
		
		builder.Property<Guid?>("id_person")
			.HasColumnType("uuid")
			.HasColumnName("id_person")
			.IsRequired(false);

		builder.HasOne(u => u.Person)
			.WithOne()
			.HasForeignKey<User>("id_person");

		builder.Property(u => u.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
    	builder.Property(u => u.Username).HasColumnName("username").IsRequired();
		builder.Property(u => u.Password).HasColumnName("password").IsRequired();
		builder.Property(u => u.Role).HasColumnName("userrole").IsRequired();        
    }
}