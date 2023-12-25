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
    }

    public void ChangeWeek(int week)
	{
		if (!IsValidWeekNumber(week))
			return;

		Week = week;
		this.AddDomainEvent(new ChoreplanWeekChangedEvent(this));
	}

    public void AddChore(Assignment chore)
	{
		if (Assignments.Contains<Assignment>(chore))
		{
			int index = Assignments.FindIndex(chs => chs as ChoreplanSlot == chore as ChoreplanSlot);
			Assignments[index] = chore;
			this.AddDomainEvent(new ChoreplanAssignmentOverridenEvent(this, chore));
		}
		else
		{
			Assignments.Add(chore);
			this.AddDomainEvent(new ChoreplanAssignmentAddedEvent(this, chore));
		}
	}

	public void ClearChoreplanSlot(ChoreplanSlot chore)
	{
		if (Assignments.Contains(chore))
		{
			Assignments.RemoveAt(Assignments.FindIndex(chs => chs.Equals(chore)));
			this.AddDomainEvent(new ChoreplanAssignmentClearedEvent(this, chore));
		}
	}

    static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}

}
