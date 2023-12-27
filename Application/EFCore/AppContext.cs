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
    public DbSet<Food> Foods {get;set;}
    public DbSet<Shoppinglist> Shoppinglists {get;set;}
    public DbSet<Mealtype> Mealtypes {get;set;}
    public DbSet<Mealplan> Mealplans {get;set;}
    public DbSet<Choreplan> Choreplans {get;set;}
    public DbSet<Calendar> Calendars {get;set;}
    public DbSet<Person> Persons {get;set;}
    */
    private string DbPath = "";

    public AppContext()
    {
        DbPath = "Server=federlein.website;Database=hpEfCore;Trusted_Connection=True;";
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql($"Data Source={DbPath}");
}