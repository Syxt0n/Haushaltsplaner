using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swaksoft.Domain.Seedwork.Aggregates;

namespace Domain.ValueObjects;
public class Ingredient : ValueObject<Ingredient>
{
	public Item Item { get; private set; }
	public int Amount { get; private set; }
}
