using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Persons;

namespace Domain.Mealplans;
public abstract class MealSlot : ValueObject
{
	public Guid ID_Mealplan {get;}
	public Mealtype Type { get; }
	public Person Person { get; }
	public DayOfWeek DayofWeek { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public MealSlot()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{}

	public MealSlot(Mealtype mealtype, Person person, DayOfWeek dayOfWeek)
	{
		Type = mealtype;
		Person = person;
		DayofWeek = dayOfWeek;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Type;
		yield return Person;
		yield return DayofWeek;
	}
}
