using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Food.Events;
public class FoodNameChangedEvent : DomainEvent
{
	public string Name;

	public FoodNameChangedEvent(string name)
	{
		Name = name;
	}
}
