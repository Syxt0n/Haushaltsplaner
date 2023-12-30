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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Meal()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{}

	public Meal(Food food, Mealtype mealtype, Person person, DayOfWeek dayOfWeek) : base(mealtype, person, dayOfWeek)
	{
		Food = food;

		Validate();
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Type;
		yield return Person;
		yield return DayofWeek;
		yield return Food;
	}

    public override void Validate()
    {
		Type.Validate();
		Person.Validate();
		Food.Validate();
    }
}
