using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Application.EFCore.Configurations;
using Domain.Shared;
using Domain.Foods;
using Domain.Shoppinglists;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Calendars;
using Domain.Mealplans;
using Domain.Persons;
using Domain.Choreplans;
using Microsoft.Extensions.Configuration;
using DomainBase.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Users;

namespace Application.EFCore;

public class HpContext: DbContext
{
	#region FoodAggregate
	public DbSet<Item> Items { get; set; }
	public DbSet<Ingredient> Ingredients { get; set; }
	public DbSet<Food> Foods { get; set; }
	#endregion

	#region ShoppinglistAggregate
	public DbSet<Article> Articles {get; set;}
	public DbSet<Shoppinglist> Shoppinglists {get; set;}
	#endregion

	#region Personalcalender
	public DbSet<Appointment> Appointments {get; set;}
	public DbSet<PersonalCalendar> PersonalCalendars {get;set;}
	#endregion

	#region Person
	public DbSet<Person> Persons {get;set;}
	#endregion
	
	#region Mealplanner
	public DbSet<Mealtype> Mealtypes {get; set;}
	public DbSet<Meal> Meals {get; set;}
	public DbSet<Mealplan> Mealplans {get; set;}
	#endregion

	#region Choreplanner
	public DbSet<Chore> Chores {get; set;}
	public DbSet<Assignment> Assignments {get; set;}
	public DbSet<Choreplan> Choreplans {get; set;}
	#endregion

	#region User
	public DbSet<User> Users {get; set;}
	#endregion

	private string DbPath = "";
	private readonly IPublisher Publisher;
	private readonly ILogger Logger;

	
	public HpContext(IPublisher publisher, ILogger logger)
	{
		DbPath = "Server=federlein.website:5432;Database=Haushaltsplaner;Username=admin;Password=Lindach1210;pooling=true;SearchPath=main";
		Publisher = publisher;
		Logger = logger;
	}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public HpContext()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
		DbPath = DbPath = "Server=federlein.website:5432;Database=Haushaltsplaner;Username=admin;Password=Lindach1210;pooling=true;SearchPath=main";
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		Logger.Log(LogLevel.Debug, "Saving Changes");

		var domainEvents = ChangeTracker.Entries<Entity<Guid?>>()
			.Select(e => e.Entity)
			.Where(e => e.DomainEvents.Any())
			.SelectMany(e => e.DomainEvents);

		foreach (var domevent in domainEvents)
		{
			await Publisher.Publish(domevent, cancellationToken);
			Logger.Log(LogLevel.Debug, $"Publishing {nameof(domevent)}");
		}

		Logger.Log(LogLevel.Debug, "Persisting to Database");
		return await base.SaveChangesAsync(cancellationToken);
	}
	
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseNpgsql(DbPath);
	
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new ItemConfiguration());
		modelBuilder.ApplyConfiguration(new IngredientConfiguration());
		modelBuilder.ApplyConfiguration(new FoodConfiguration());
		modelBuilder.ApplyConfiguration(new ArticleConfiguration());
		modelBuilder.ApplyConfiguration(new ShoppinglistConfiguration());
		modelBuilder.ApplyConfiguration(new PersonConfigurations());
		modelBuilder.ApplyConfiguration(new CalendarConfigurations());
		modelBuilder.ApplyConfiguration(new ChoreConfiguration());
		modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
		modelBuilder.ApplyConfiguration(new ChoreplanConfigurations());
		modelBuilder.ApplyConfiguration(new MealtypeConfiguration());
		modelBuilder.ApplyConfiguration(new MealConfiguration());
		modelBuilder.ApplyConfiguration(new MealplanConfigurations());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
	}
}