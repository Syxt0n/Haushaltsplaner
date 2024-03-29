﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Application.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Application.EFCore.Migrations
{
    [DbContext(typeof(HpContext))]
    [Migration("20240205174129_init")]
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

            modelBuilder.Entity("Domain.Calendars.Appointment", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Done")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<int>>("ReminderInMinutes")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<TimeSpan>("TimeRange")
                        .HasColumnType("interval");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("id_calendar")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("id_calendar");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Domain.Calendars.PersonalCalendar", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ActiveCulture")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("activeCulture");

                    b.Property<int>("ActiveWeek")
                        .HasColumnType("integer")
                        .HasColumnName("activeWeek");

                    b.HasKey("Id");

                    b.ToTable("calendars", "main");
                });

            modelBuilder.Entity("Domain.Choreplans.Assignment", b =>
                {
                    b.Property<Guid>("id_choreplan")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<Guid>("id_person")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<Guid>("id_chore")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<int>("day")
                        .HasColumnType("int");

                    b.HasKey("id_choreplan", "id_person", "id_chore", "day");

                    b.HasIndex("id_chore");

                    b.HasIndex("id_person");

                    b.ToTable("assignments", "main");
                });

            modelBuilder.Entity("Domain.Choreplans.Chore", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("id");

                    b.ToTable("chores", "main");
                });

            modelBuilder.Entity("Domain.Choreplans.Choreplan", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("Week")
                        .HasColumnType("integer")
                        .HasColumnName("weeknumber");

                    b.HasKey("Id");

                    b.ToTable("choreplans", "main");
                });

            modelBuilder.Entity("Domain.Foods.Food", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

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

            modelBuilder.Entity("Domain.Mealplans.Meal", b =>
                {
                    b.Property<Guid>("id_mealplan")
                        .HasColumnType("uuid")
                        .HasColumnName("id_mealplan")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<Guid>("id_food")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<Guid>("id_person")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreing Key", 0);

                    b.Property<Guid>("id_mealtype")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<int>("day")
                        .HasColumnType("int")
                        .HasAnnotation("Foreign Key", 0);

                    b.HasKey("id_mealplan", "id_food", "id_person", "id_mealtype", "day");

                    b.HasIndex("id_food");

                    b.HasIndex("id_mealtype");

                    b.HasIndex("id_person");

                    b.ToTable("meals", "main");
                });

            modelBuilder.Entity("Domain.Mealplans.Mealplan", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("Week")
                        .HasColumnType("integer")
                        .HasColumnName("weeknumber");

                    b.HasKey("Id");

                    b.ToTable("mealplans", "main");
                });

            modelBuilder.Entity("Domain.Mealplans.Mealtype", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("id");

                    b.ToTable("mealtypes", "main");
                });

            modelBuilder.Entity("Domain.Persons.Person", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean")
                        .HasColumnName("deleted");

                    b.Property<string>("Displayname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("displayname");

                    b.HasKey("Id");

                    b.ToTable("persons", "main");
                });

            modelBuilder.Entity("Domain.Shared.Item", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasDefaultValueSql("gen_random_uuid()")
                        .HasAnnotation("Key", 0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("id");

                    b.ToTable("items", "main");
                });

            modelBuilder.Entity("Domain.Shoppinglists.Article", b =>
                {
                    b.Property<Guid>("id_shoppinglist")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<Guid>("id_item")
                        .HasColumnType("uuid")
                        .HasAnnotation("Foreign Key", 0);

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.HasKey("id_shoppinglist", "id_item");

                    b.HasIndex("id_item");

                    b.ToTable("articles", "main");
                });

            modelBuilder.Entity("Domain.Shoppinglists.Shoppinglist", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean")
                        .HasColumnName("deleted");

                    b.HasKey("Id");

                    b.ToTable("shoppinglists", "main");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("userrole");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<Guid?>("id_person")
                        .HasColumnType("uuid")
                        .HasColumnName("id_person");

                    b.HasKey("Id");

                    b.HasIndex("id_person")
                        .IsUnique();

                    b.ToTable("users", "main");
                });

            modelBuilder.Entity("Domain.Calendars.Appointment", b =>
                {
                    b.HasOne("Domain.Calendars.PersonalCalendar", null)
                        .WithMany("Appointments")
                        .HasForeignKey("id_calendar")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Choreplans.Assignment", b =>
                {
                    b.HasOne("Domain.Choreplans.Chore", null)
                        .WithMany()
                        .HasForeignKey("id_chore")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Choreplans.Choreplan", null)
                        .WithMany("Assignments")
                        .HasForeignKey("id_choreplan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Persons.Person", null)
                        .WithMany()
                        .HasForeignKey("id_person")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Domain.Mealplans.Meal", b =>
                {
                    b.HasOne("Domain.Foods.Food", null)
                        .WithMany()
                        .HasForeignKey("id_food")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Mealplans.Mealplan", null)
                        .WithMany("Meals")
                        .HasForeignKey("id_mealplan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Mealplans.Mealtype", null)
                        .WithMany()
                        .HasForeignKey("id_mealtype")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Persons.Person", null)
                        .WithMany()
                        .HasForeignKey("id_person")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Shoppinglists.Article", b =>
                {
                    b.HasOne("Domain.Shared.Item", null)
                        .WithMany()
                        .HasForeignKey("id_item")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Shoppinglists.Shoppinglist", null)
                        .WithMany("Articles")
                        .HasForeignKey("id_shoppinglist")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.HasOne("Domain.Persons.Person", "Person")
                        .WithOne()
                        .HasForeignKey("Domain.Users.User", "id_person");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Domain.Calendars.PersonalCalendar", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Domain.Choreplans.Choreplan", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Domain.Foods.Food", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("Domain.Mealplans.Mealplan", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("Domain.Shoppinglists.Shoppinglist", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
