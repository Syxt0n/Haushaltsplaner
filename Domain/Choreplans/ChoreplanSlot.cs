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
    public Guid ID_Choreplan { get; }
    public Person Person { get; }
	public DayOfWeek DayofWeek { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ChoreplanSlot()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {}

    public ChoreplanSlot(Person person, DayOfWeek dayOfWeek)
    {
        Person = person;
        DayofWeek = dayOfWeek;

        Validate();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Person;
        yield return DayofWeek;
    }

    public override void Validate()
    {
        Person.Validate();
    }
}