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
	public Mealtype Type { get; }
	public Person Person { get; }
	public DayOfWeek DayofWeek { get; }

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
