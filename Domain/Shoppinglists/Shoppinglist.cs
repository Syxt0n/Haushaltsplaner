using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;
using DomainBase.Domain;

namespace Domain.Shoppinglists;

public class Shoppinglist : AggregateRoot<Guid?>
{
	public DateOnly Date;
	public List<Article> Articles = [];
	public Shoppinglist(Guid? ID, DateOnly date, List<Article> articles) : base(ID)
	{
		Date = date;
		Articles = articles;
	}

	public Shoppinglist(DateOnly date, List<Article> articles) : base(null)
	{
		Date = date;
		Articles = articles;
		this.AddDomainEvent(new ShoppinglistCreatedEvent(this));
	}

	//Add/Remove
	public void AddArticles((Item Item, int Amount)[] articles)
	{
		bool amountWasChanged = false;
		bool articleWasAdded =false;

		foreach (var tuple in articles)
		{
			Article listArticle = Articles.Find(x => x.Item == tuple.Item);
			if (listArticle is not null)
			{
				int listArticelIndex = Articles.IndexOf(listArticle);
				Article listArticel = Articles[listArticelIndex];
				Articles[listArticelIndex] = new Article(
					tuple.Item, listArticel.Amount + tuple.Amount, listArticel.Position
				);
				amountWasChanged = true;
			}
			else
			{
				//Position errechnen
				Articles.Add(new Article(tuple.Item, tuple.Amount, Articles.Count));
				articleWasAdded = true;
			}
		}

		if (amountWasChanged)
			this.AddDomainEvent(new ShoppinglistArticleAmountChangedEvent(this));
		
		if (articleWasAdded)
			this.AddDomainEvent(new ShoppinglistArticleAddedEvent(this));
	}

	public void RemoveArticles((int Position, int Amout)[] removableItems)
	{
		bool articleWasChanged = false;
		bool articleWasRemoved = false;

		foreach (var tuple in removableItems)
		{
			Article listArticle = Articles.Find(x => x.Position == tuple.Position);
			if (listArticle is null)
				continue;

			int listArticelIndex = Articles.IndexOf(listArticle);

			if (listArticle.Amount > tuple.Amout)
			{
				Articles[listArticelIndex] = new Article(
					listArticle.Item, listArticle.Amount - tuple.Amout, listArticle.Position
				);
				articleWasChanged = true;
			}
			else
			{
				swapArticles(listArticelIndex, Articles.Count);
				Articles.Remove(listArticle);
				articleWasChanged = true;
				articleWasRemoved = true;
			}
		}

		if (articleWasChanged)
			this.AddDomainEvent(new ShoppinglistArticleAmountChangedEvent(this));
			
		if (articleWasRemoved)
			this.AddDomainEvent(new ShoppinglistArticleRemovedEvent(this));
	}

	public void ChangePosition((int StartPos, int TargetPos)[] values)
	{
		foreach (var tuple in values)
		{
			if (!isValidPosition(tuple.StartPos) || !isValidPosition(tuple.TargetPos))
				continue;

			if (tuple.StartPos < tuple.TargetPos)
				swapArticles(tuple.StartPos, tuple.TargetPos);
			else
				swapArticles(tuple.TargetPos, tuple.StartPos);
		}

		this.AddDomainEvent(new ShoppinglistArticleSwappedEvent(this));
	}	

	private bool isValidPosition(int positionValue)
	{
		return positionValue >= 0 && positionValue < Articles.Count;
	}

	private void swapArticles(int smallerPos, int greaterPos)
	{
		Article tempArticle = Articles[smallerPos];

		for	(int iCount = smallerPos + 1; iCount <= greaterPos; iCount++)
		{
			Article tempArticle2 = Articles[iCount];
			Articles[iCount-1] = tempArticle2;
		}

		Articles[greaterPos] = tempArticle;
	}

	public void ChangeAmount((int Position, int Amount)[] items)
	{
		foreach (var tuple in items)
		{
			if (tuple.Amount <= 0)
				return;

			Article listArticle = Articles.Find(x => x.Position == tuple.Position);
			
			if (listArticle is null)
				continue;
			
			int listArticelIndex = Articles.IndexOf(listArticle);

			Articles[listArticelIndex] = new Article(
				listArticle.Item, tuple.Amount, listArticle.Amount
			);
		}

		this.AddDomainEvent(new ShoppinglistArticleAmountChangedEvent(this));
	}
}