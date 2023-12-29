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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Item()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{}

	public Item(string name)
	{
		Name = name;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Name;
	}
}
