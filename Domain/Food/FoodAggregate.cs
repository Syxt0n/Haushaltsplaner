﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Food.Events;

namespace Domain.Food;
public class FoodAggregate : AggregateRoot<Guid>
{
	public Guid ID { get; private set; }
	public string Name { get; private set; }
	public List<Ingredient> Ingredients { get; private set; }
	public bool Deleted { get; set; }

	public bool ChangeName(string name)
	{
		if (name == "")
			return false;

		Name = name;

		this.AddDomainEvent(new FoodNameChangedEvent(Name));
		return true;
	}

	public void Delete()
	{
		Deleted = true;
		this.AddDomainEvent(new FoodDeletedEvent());
	}

	public void AddIngredients(Ingredient[] ingredients)
	{
		foreach (Ingredient ingredient in ingredients)
		{
			addIngredientToList(ingredient);
		}

		this.AddDomainEvent(new FoodIngredientsAddedEvent(ingredients));
	}

	public void RemoveIngredients(Ingredient[] ingredients)
	{
		foreach (Ingredient ingredient in ingredients)
		{
			removeIngredientFromList(ingredient);
		}

		this.AddDomainEvent(new FoodIngredientsRemovedEvent(ingredients));
	}

	private void addIngredientToList(Ingredient ingredient)
	{
		Ingredient newIngredient;

		if (Ingredients.Contains(ingredient))
		{
			Ingredient listIngredient = Ingredients.Where(i => i == ingredient).First();

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
		if (!Ingredients.Contains(ingredient))
			return;

		Ingredient listIngredient = Ingredients.Where(i => i == ingredient).First();

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