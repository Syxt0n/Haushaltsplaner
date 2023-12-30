using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;
using DomainBase.Domain;

namespace Domain.Shoppinglists;

public class Article : ValueObject
{
    public Item Item { get; }
    public int Position {get;}
    public int Amount { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Article()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {}

    public Article(Item item, int amount, int position)
    {
        Item = item;
        Amount = amount;
        Position = position;

        Validate();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Item;
    }

    public override void Validate()
    {
        Item.Validate();

        if (Position < 0)
            throw new ArgumentNullException("Position", "Article position must be greater than or equalt to 0.");

        if (Amount > 0)
            throw new ArgumentNullException("Amount", "Article amount must be greater than 0.");
    }
}
