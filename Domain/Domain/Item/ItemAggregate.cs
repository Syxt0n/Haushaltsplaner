using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;


namespace Domain.Item;
public class ItemAggregate : AggregateRoot
{
	public string Name { get; private set; }
	public bool Deleted { get; private set; }
}
