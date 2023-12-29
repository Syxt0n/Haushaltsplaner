﻿// <auto-generated />
using System;
using Application.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Application.EFCore.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20231229140341_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Foods.Food", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean")
                        .HasColumnName("deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("food", "main");
                });

            modelBuilder.Entity("Domain.Foods.Ingredient", b =>
                {
                    b.Property<Guid>("id_food")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<Guid>("id_item")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.HasKey("id_food", "id_item");

                    b.HasIndex("id_item");

                    b.ToTable("ingredients", "main");
                });

            modelBuilder.Entity("Domain.Shared.Item", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasAnnotation("Key", 0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("id");

                    b.ToTable("items", "main");
                });

            modelBuilder.Entity("Domain.Foods.Ingredient", b =>
                {
                    b.HasOne("Domain.Foods.Food", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("id_food")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Shared.Item", null)
                        .WithMany()
                        .HasForeignKey("id_item")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Foods.Food", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}
