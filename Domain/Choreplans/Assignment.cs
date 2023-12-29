using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using Domain.Persons;

namespace Domain.Choreplans;

public class Assignment : ChoreplanSlot
{
    public Chore Chore { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Assignment()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {}

    public Assignment(Person person, DayOfWeek dayOfWeek, Chore chore) : base(person, dayOfWeek)
    {
        Chore = chore;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Person;
        yield return DayofWeek;
        yield return Chore;
    }
}