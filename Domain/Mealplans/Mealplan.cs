using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using Domain.Shoppinglists;
using Domain.Foods;

namespace Domain.Mealplans;
public class Mealplan : AggregateRoot<Guid?>
{
	public int Week { get; private set; }
	public List<Meal> Meals { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Mealplan():base(null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}
	public Mealplan(int week, List<Meal> meals):base(null)
	{
		Week = week;
		Meals = meals;

		this.AddDomainEvent(new MealplanCreatedEvent(this));
	}


	public void ChangeWeek(int week)
	{
		if (!IsValidWeekNumber(week))
			return;

		Week = week;
		this.AddDomainEvent(new MealplanWeekChangedEvent(this));
	}

	public void AddMeal(Meal meal)
	{
		int index = Meals.FindIndex(ms => ms as MealSlot == meal as MealSlot);
		if (index > -1)
		{
			Meals[index] = meal;
			this.AddDomainEvent(new MealplanMealSlotOverridenEvent(this, meal));
		}
		else
		{
			Meals.Add(meal);
			this.AddDomainEvent(new MealplanMealAddedEvent(this, meal));
		}
	}

	public void ClearMealSlot(MealSlot mealSlot)
	{
		int index = Meals.FindIndex(m => m.Equals(mealSlot));
		if (index > -1)
		{
			Meals.RemoveAt(index);
			this.AddDomainEvent(new MealplanMealSlotClearedEvent(this, mealSlot));
		}
	}

	public void ExportToShoppinglist(Shoppinglist shoppinglist)
	{
		Article[] values = [];
		foreach (var meal in Meals)
		{
			foreach (var ingredient in meal.Food.Ingredients)
				values.Append(new Article(ingredient.Item, ingredient.Amount, -1));
		}

		shoppinglist.AddArticles(values);
		this.AddDomainEvent(new MealPlanExportedToShoppinglistEvent(this, shoppinglist));
	}

	private static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}
}
