using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;

namespace Domain.Food;
public class FoodAggregate : AggregateRoot<Guid>
{
	public Guid ID { get; private set; }
	public string Name { get; private set; }
	public List<Ingredient> Ingredients { get; private set; }
	public bool Deleted { get; set; }


}
