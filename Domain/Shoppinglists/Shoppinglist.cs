using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Domain.Shared;
using DomainBase.Domain;

namespace Domain.Shoppinglists;

public class Shoppinglist : AggregateRoot<Guid?>
{
	public DateOnly Date {get; private set;}
	public List<Article> Articles {get; private set;} = [];
	public bool Deleted {get; private set;} = false;

	public Shoppinglist() : base(null)
	{
	}

	public Shoppinglist(DateOnly date, List<Article> articles) : base(null)
	{
		Date = date;
		Articles = articles;
		this.AddDomainEvent(new ShoppinglistCreatedEvent(this));
	}

	public void AddArticles(Article[] articles)
	{
		List<Article> overridenArticles = [];
		foreach (var article in articles)
		{
			overridenArticles = addArticleToList(article);
		}

		if (overridenArticles.Count > 0)
			this.AddDomainEvent(new ShoppinglistArticlesOverridenChangedEvent(this, overridenArticles));
		
		this.AddDomainEvent(new ShoppinglistArticleAddedEvent(this));
	}

	private List<Article> addArticleToList(Article article)
	{
		List<Article> result = [];
		Article newArticle;

		int index = Articles.IndexOf(article);
		if (index > -1)
		{
			Article listArticle = Articles[index];

			newArticle = new Article(
				article.Item,
				listArticle.Amount + article.Amount,
				listArticle.Position
			);
			result.Add(listArticle);
			Articles.Remove(listArticle);
		}
		else
			newArticle = new Article(article.Item, article.Amount, Articles.Count);

		Articles.Add(newArticle);
		return result;
	}

	public void RemoveArticles(Article[] articles)
	{
		List<Article> overridenArticles = [];
		foreach (var article in articles)
		{
			int index = Articles.IndexOf(article);
			
			if (index > -1)
			{

				Article listArticle = Articles[index];

				if (listArticle.Amount > article.Amount)
				{
					overridenArticles.Add(listArticle);

					Articles[index] = new Article(
						listArticle.Item, 
						listArticle.Amount - article.Amount, 
						listArticle.Position
					);
				}
				else
				{
					overridenArticles.AddRange(swapArticles(index, Articles.Count));
					Articles.Remove(listArticle);
				}
			}
		}

		if (overridenArticles.Count > 0)
			this.AddDomainEvent(new ShoppinglistArticlesOverridenChangedEvent(this, overridenArticles));
			
		this.AddDomainEvent(new ShoppinglistArticleRemovedEvent(this));
	}

	public void ChangePosition((int StartPos, int TargetPos)[] values)
	{
		bool trigger = false;
		foreach (var tuple in values)
		{
			if (!isValidPosition(tuple.StartPos) || !isValidPosition(tuple.TargetPos))
				continue;

			if (tuple.StartPos < tuple.TargetPos)
				swapArticles(tuple.StartPos, tuple.TargetPos);
			else
				swapArticles(tuple.TargetPos, tuple.StartPos);
			trigger = true;
		}

		if (trigger)
			this.AddDomainEvent(new ShoppinglistArticleSwappedEvent(this));
	}	

	private bool isValidPosition(int positionValue)
	{
		return positionValue >= 0 && positionValue < Articles.Count;
	}

	private List<Article> swapArticles(int smallerPos, int greaterPos)
	{
		List<Article> result = [];
		Article tempArticle = Articles[smallerPos];

		for	(int iCount = smallerPos + 1; iCount <= greaterPos; iCount++)
		{
			Article tempArticle2 = Articles[iCount];
			Articles[iCount-1] = tempArticle2;
			if (!Articles.Contains(tempArticle2))
				result.Add(tempArticle2);
		}

		Articles[greaterPos] = tempArticle;
		result.Add(tempArticle);
		return result;
	}

	public void ChangeAmount(Article[] articles)
	{
		bool trigger = false;
		List<Article> result = [];
		foreach (var article in articles)
		{
			if (article.Amount <= 0)
				continue;

			int index = Articles.IndexOf(article);
			if (index > -1)
			{
				Article listArticle = Articles[index];

				Articles[index] = new Article(
					listArticle.Item, 
					article.Amount, 
					listArticle.Amount
				);
				result.Add(article);
				
				trigger = true;
			}
		}
		if (trigger)
			this.AddDomainEvent(new ShoppinglistArticlesOverridenChangedEvent(this, result));
	}

	public void Delete()
	{
		Deleted = true;
		this.AddDomainEvent(new ShoppingListDeletedEvent(this));
	}
}