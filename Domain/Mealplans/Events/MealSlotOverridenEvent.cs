using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Mealplans.Events;
public class MealSlotOverridenEvent : DomainEvent<MealplanAggregate>
{
	Meal Meal;
	public MealSlotOverridenEvent(MealplanAggregate value, Meal meal) : base(value)
	{
		Meal = meal;
	}
}
