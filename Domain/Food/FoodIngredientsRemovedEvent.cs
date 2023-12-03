using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Food;
public class FoodIngredientsRemovedEvent : DomainEvent
{
	public Ingredient[] Ingredients;

	public FoodIngredientsRemovedEvent(Ingredient[] ingredients)
	{
		Ingredients = ingredients;
	}
}
