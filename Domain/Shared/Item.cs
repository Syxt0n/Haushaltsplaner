using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;

namespace Domain.Shared;
public class Item : ValueObject
{
	public string Name { get; }

	public Item(string name)
	{
		Name = name;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Name;
	}
}
