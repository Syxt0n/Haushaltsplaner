using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;

namespace Domain.Foods;
public class Ingredient : ValueObject
{
	public Item Item { get; }
	public int Amount { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Ingredient()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}

	public Ingredient(Item item, int amount)
	{
		Item = item;
		Amount = amount;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Item;
	}
}
