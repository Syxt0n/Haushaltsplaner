using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;

namespace Domain.Mealplans;
public class Mealplan : AggregateRoot<Guid?>
{
	public int Week { get; private set; }
	public List<Meal> Meals { get; private set; }

	public Mealplan(int week, List<Meal> meals):base(null)
	{
		Week = week;
		Meals = meals;
	}

	public Mealplan(Guid id, int week, List<Meal> meals):base(id)
	{
		Week = week;
		Meals = meals;
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
		if (Meals.Contains<MealSlot>(meal))
		{
			int index = Meals.FindIndex(ms => ms as MealSlot == meal as MealSlot);
			Meals[index] = meal;
			this.AddDomainEvent(new MealplanMealSlotOverridenEvent(this, meal));
		}
		else
		{
			Meals.Add(meal);
			this.AddDomainEvent(new MealplanMealslotAddedEvent(this, meal));
		}
	}

	public void ClearMealSlot(MealSlot mealSlot)
	{
		if (Meals.Contains(mealSlot))
		{
			Meals.RemoveAt(Meals.FindIndex(m => m.Equals(mealSlot)));
			this.AddDomainEvent(new MealplanMealSlotClearedEvent(this, mealSlot));
		}
	}

	static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}
}
