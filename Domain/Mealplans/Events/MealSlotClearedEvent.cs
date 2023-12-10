using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Mealplans.Events;
public class MealSlotClearedEvent : DomainEvent<MealplanAggregate>
{
	MealSlot MealSlot;
	public MealSlotClearedEvent(MealplanAggregate value, MealSlot mealSlot) : base(value)
	{
		MealSlot = mealSlot;
	}
}
