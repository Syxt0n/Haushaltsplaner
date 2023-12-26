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
    public Choreplan(Guid? id, int week, List<Assignment> assignments) : base(id)
    {
        Week = week;
        Assignments = assignments;
    }
    public Choreplan(int week, List<Assignment> assignments) : base(null)
    {
        Week = week;
        Assignments = assignments;
		this.AddDomainEvent(new ChoreplanCreatedEvent(this));
    }

    public void ChangeWeek(int week)
	{
		if (!IsValidWeekNumber(week))
			return;

		Week = week;
		this.AddDomainEvent(new ChoreplanWeekChangedEvent(this));
	}

    public void AddChore(Assignment assignment)
	{
		int index = Assignments.FindIndex(chs => chs as ChoreplanSlot == assignment as ChoreplanSlot);
		if (index > -1)
		{
			Assignments[index] = assignment;
			this.AddDomainEvent(new ChoreplanAssignmentOverridenEvent(this, assignment));
		}
		else
		{
			Assignments.Add(assignment);
			this.AddDomainEvent(new ChoreplanAssignmentAddedEvent(this, assignment));
		}
	}

	public void ClearChoreplanSlot(ChoreplanSlot choreplanSlot)
	{
		int index = Assignments.FindIndex(chs => chs.Equals(choreplanSlot));
		if (index > -1)
		{
			Assignments.RemoveAt(index);
			this.AddDomainEvent(new ChoreplanChoreplanSlotClearedEvent(this, choreplanSlot));
		}
	}

    static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}

}
