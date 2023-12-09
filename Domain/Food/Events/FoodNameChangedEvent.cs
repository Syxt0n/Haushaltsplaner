using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Food.Events;
public class FoodNameChangedEvent : DomainEvent<FoodAggregate>
{
	public FoodNameChangedEvent(FoodAggregate value) : base(value)
	{
	}
}
