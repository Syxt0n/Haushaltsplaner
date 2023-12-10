using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Mealplans.Events;
public class MealplanWeekChangedEvent : DomainEvent<MealplanAggregate>
{
	public MealplanWeekChangedEvent(MealplanAggregate value) : base(value)
	{
	}
}
