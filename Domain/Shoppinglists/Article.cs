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

    public Article(Item item, int amount, int position)
    {
        Item = item;
        Amount = amount;
        Position = position;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Item;
    }
}
