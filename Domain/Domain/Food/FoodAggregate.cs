using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Food;
public class FoodAggregate : AggregateRoot
{
	public int ID { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }
	public List<Ingredient> Ingredient { get; private set; }

}
