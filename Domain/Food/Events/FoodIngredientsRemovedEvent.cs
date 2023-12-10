﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Food.Events;
public class FoodIngredientsRemovedEvent : DomainEvent<FoodAggregate>
{
	public FoodIngredientsRemovedEvent(FoodAggregate value) : base(value)
	{
	}
}