using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Food;
using Domain.Persons;

namespace Domain.Mealplans;
public class Meal : MealSlot
{
	public FoodAggregate Food { get; set; }
	public Meal(FoodAggregate food, Mealtype mealtype, PersonAggregate person, DayOfWeek dayOfWeek) : base(mealtype, person, dayOfWeek)
	{
		Food = food;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Type;
		yield return Person;
		yield return DayofWeek;
		yield return Food;
	}
}
