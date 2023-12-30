using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;

namespace Domain.Choreplans;

public class Choreplan : AggregateRoot<Guid?>
{
    public int Week { get; private set; }
	public List<Assignment> Assignments { get; private set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Choreplan() : base(null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }
    public Choreplan(int week, List<Assignment> assignments) : base(null)
    {
        Week = week;
        Assignments = assignments;

		Validate();
		this.AddDomainEvent(new ChoreplanCreatedEvent(this));
    }

    public void ChangeWeek(int week)
	{
		if (!IsValidWeekNumber(week))
			return;

		Week = week;

		Validate();
		this.AddDomainEvent(new ChoreplanWeekChangedEvent(this));
	}

    public void AddChore(Assignment assignment)
	{
		int index = Assignments.FindIndex(chs => chs as ChoreplanSlot == assignment as ChoreplanSlot);
		if (index > -1)
		{
			Assignments[index] = assignment;

			Validate();
			this.AddDomainEvent(new ChoreplanAssignmentOverridenEvent(this, assignment));
		}
		else
		{
			Assignments.Add(assignment);

			Validate();
			this.AddDomainEvent(new ChoreplanAssignmentAddedEvent(this, assignment));
		}
	}

	public void ClearChoreplanSlot(ChoreplanSlot choreplanSlot)
	{
		int index = Assignments.FindIndex(chs => chs.Equals(choreplanSlot));
		if (index > -1)
		{
			Assignments.RemoveAt(index);

			Validate();
			this.AddDomainEvent(new ChoreplanChoreplanSlotClearedEvent(this, choreplanSlot));
		}
	}

    static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}

    public override void Validate()
    {
		/* public int Week { get; private set; }
		public List<Assignment> Assignments { get; private set; } */   

		if (Week > 53 || Week < 1)
			throw new ArgumentNullException("Week", "Choreplan must have a valid Weeknumber.");

		foreach	(Assignment assignment in Assignments)
			assignment.Validate();
    }

}
