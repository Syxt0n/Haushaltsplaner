using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;

namespace Domain.Foods;
public class Food : AggregateRoot<Guid?>
{
	public string Name { get; private set; }
	public List<Ingredient> Ingredients { get; private set; }
	public bool Deleted { get; set; }

	public Food(Guid id, string name, List<Ingredient> ingredients, bool deleted): base(id)
	{
		Name = name;
		Ingredients = ingredients;
		Deleted = deleted;
	}

	public Food(string name, List<Ingredient> ingredients) : base(null)
	{
		Name = name;
		Ingredients = ingredients;
		Deleted = false;

		this.AddDomainEvent(new FoodCreatedEvent(this));
	}

	public bool ChangeName(string name)
	{
		if (string.IsNullOrEmpty(name))
			return false;

		Name = name;

		this.AddDomainEvent(new FoodNameChangedEvent(this));
		return true;
	}

	public void Delete()
	{
		Deleted = true;
		this.AddDomainEvent(new FoodDeletedEvent(this));
	}

	public void AddIngredients(Ingredient[] ingredients)
	{
		foreach (Ingredient ingredient in ingredients)
		{
			addIngredientToList(ingredient);
		}

		this.AddDomainEvent(new FoodIngredientsAddedEvent(this, ingredients));
	}

	public void RemoveIngredients(Ingredient[] ingredients)
	{
		foreach (Ingredient ingredient in ingredients)
		{
			removeIngredientFromList(ingredient);
		}

		this.AddDomainEvent(new FoodIngredientsRemovedEvent(this, ingredients));
	}

	private void addIngredientToList(Ingredient ingredient)
	{
		Ingredient newIngredient;

		int index = Ingredients.IndexOf(ingredient);
		if (index > -1)
		{
			Ingredient listIngredient = Ingredients[index];

			newIngredient = new Ingredient(
				ingredient.Item,
				ingredient.Amount + listIngredient.Amount
			);

			Ingredients.Remove(listIngredient);
		}
		else
			newIngredient = ingredient;

		Ingredients.Add(newIngredient);
	}

	private void removeIngredientFromList(Ingredient ingredient)
	{
		int index = Ingredients.IndexOf(ingredient);

		if (index > -1)
		{
			Ingredient listIngredient = Ingredients[index];
			Ingredients.Remove(listIngredient);

			if ((listIngredient.Amount - ingredient.Amount) > 0)
			{
				Ingredients.Add(new Ingredient(
					ingredient.Item,
					listIngredient.Amount - ingredient.Amount
				));
			}
		}
	}
}
