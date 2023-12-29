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

	public Ingredient()
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
