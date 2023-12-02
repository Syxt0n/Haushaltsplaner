using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swaksoft.Domain.Seedwork.Aggregates;

namespace Domain.ValueObjects;
public class Item : ValueObject<Item>
{
	public string Name { get; private set; }
}
