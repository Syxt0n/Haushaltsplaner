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
	public List<Ingredient> Ingredients { get; private set; } = [];
	public bool Deleted { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Food(): base(null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}

	public Food(string name, List<Ingredient> ingredients) : base(null)
	{
		Name = name;
		Ingredients = ingredients;
		Deleted = false;

		Validate();
		this.AddDomainEvent(new FoodCreatedEvent(this));
	}

	public bool ChangeName(string name)
	{
		Name = name;

		Validate();
		this.AddDomainEvent(new FoodNameChangedEvent(this));
		return true;
	}

	public void Delete()
	{
		Deleted = true;

		Validate();
		this.AddDomainEvent(new FoodDeletedEvent(this));
	}

	public void AddIngredients(Ingredient[] ingredients)
	{
		foreach (Ingredient ingredient in ingredients)
		{
			addIngredientToList(ingredient);
		}

		Validate();
		this.AddDomainEvent(new FoodIngredientsAddedEvent(this, ingredients));
	}

	public void RemoveIngredients(Ingredient[] ingredients)
	{
		foreach (Ingredient ingredient in ingredients)
		{
			removeIngredientFromList(ingredient);
		}

		Validate();
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

    public override void Validate()
    {
		foreach (var ingredient in Ingredients)
			ingredient.Validate();

		if (string.IsNullOrEmpty(Name))
			throw new ArgumentNullException("Name", "Food must have valid name.");
    }
}
