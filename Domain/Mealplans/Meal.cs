using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Foods;
using Domain.Persons;

namespace Domain.Mealplans;
public class Meal : MealSlot
{
	public Food Food { get; }
	public Meal(Food food, Mealtype mealtype, Person person, DayOfWeek dayOfWeek) : base(mealtype, person, dayOfWeek)
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
