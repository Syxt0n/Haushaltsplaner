using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Mealplans.Events;

namespace Domain.Mealplans;
public class MealplanAggregate : AggregateRoot<MealplanAggregate, Guid>
{
	public Guid ID { get; private set; }
	public int Week { get; private set; }
	public List<MealSlot> Meals { get; private set; }

	public MealplanAggregate(int week, List<MealSlot> meals)
	{
		Week = week;
		Meals = meals;
	}

	public MealplanAggregate(Guid id, int week, List<MealSlot> meals)
	{
		ID = id;
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
		List<MealSlot> temp = Meals;
		if (temp.Contains(meal))
		{
			int index = temp.FindIndex(m => m.Equals(meal));
			temp[index] = meal;
			this.AddDomainEvent(new MealSlotOverridenEvent(this, meal));
		}
		else
		{
			temp.Add(meal);
			this.AddDomainEvent(new MealAddedEvent(this, meal));
		}
	}

	public void ClearMealSlot(MealSlot mealSlot)
	{
		List<MealSlot> temp = Meals;
		if (temp.Contains(mealSlot))
		{
			temp.Remove(mealSlot);
			this.AddDomainEvent(new MealSlotClearedEvent(this, mealSlot));
		}
	}

	static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}
}
