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
	public List<ChoreplanSlot> Assignments { get; private set; }
    public Choreplan(Guid? id, int week, List<ChoreplanSlot> assignments) : base(id)
    {
        Week = week;
        Assignments = assignments;
    }
    public Choreplan(int week, List<ChoreplanSlot> assignments) : base(null)
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

    public void AddChore(ChoreplanSlot chore)
	{
		if (Assignments.Contains<ChoreplanSlot>(chore))
		{
			int index = Assignments.FindIndex(chs => chs.Equals(chore));
			Assignments[index] = chore;
			this.AddDomainEvent(new ChoreplanChorePlanSlotOverridenEvent(this, chore));
		}
		else
		{
			Assignments.Add(chore);
			this.AddDomainEvent(new ChoreplanChorePlanSlotAddedEvent(this, chore));
		}
	}

	public void ClearChoreplanSlot(ChoreplanSlot chore)
	{
		if (Assignments.Contains(chore))
		{
			Assignments.RemoveAt(Assignments.FindIndex(chs => chs.Equals(chore)));
			this.AddDomainEvent(new ChoreplanChorePlanSlotClearedEvent(this, chore));
		}
	}

    static bool IsValidWeekNumber(int weekNumber)
	{
		return weekNumber >= 1 && weekNumber <= 53;
	}

}
