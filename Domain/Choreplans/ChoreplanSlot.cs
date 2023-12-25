using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using Domain.Persons;

namespace Domain.Choreplans;

public abstract class ChoreplanSlot : ValueObject
{
    public Person Person { get; }
	public DayOfWeek DayofWeek { get; }

    public ChoreplanSlot(Person person, DayOfWeek dayOfWeek)
    {
        Person = person;
        DayofWeek = dayOfWeek;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Person;
        yield return DayofWeek;
    }
}