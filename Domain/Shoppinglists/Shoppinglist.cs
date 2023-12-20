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
	public void AddArticle(Article[] articles)
	{
		foreach (var article in articles)
		{
			if (Articles.Contains(article))
			{
				int listArticelIndex = Articles.IndexOf(article);
				Article listArticel = Articles[listArticelIndex];
				Articles[listArticelIndex] = new Article(
					article.Item, listArticel.Amount + article.Amount, listArticel.Position
				);
			}
			else
				Articles.Add(article);
		}
	}

	public void RemoveArticle((int Position, int Amout)[] removableItems)
	{
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
			}
			else
				Articles.Remove(listArticle);
		}
	}

	//ChangePosition
		//lade Item aus [ZielPos] in Zwischenspeicher
		//Schreibe Item von [ParamPos] in [ZielPos]
		//Solange [ParamPos] = frei
			//lade Item aus [ZielPos] in Zwischenspeicher
			//Schreibe Item von [ParamPos] in [ZielPos]

		//Bedacht werden muss hier: 
		//	-ZielPos kann größer ODER kleiner = [ParamPos] sein
		//=>	Die Schleife muss unabhängig von den wirklichen Positionen durchlaufen
		//		(Differenz errechnen => wenn Differenz < 0 => Differenz mal -1)
		//	-Das Prinzip innerhalb der Schleife besteht immer aus:
		//		+Item an [Pos] rausnehmen und zwischenspeichern
		//		+vorheriges, zwischengespeichertes Item in [Pos] überschreiben
		

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
	}
}