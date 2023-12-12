﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Foods;
using Domain.Persons;
using Domain.Mealplans;
using DomainBase.Domain;

namespace Domain.Shared;
public record FoodCreatedEvent(Food Food) : IDomainEvent;
public record FoodNameChangedEvent(Food Food) : IDomainEvent;
public record FoodDeletedEvent(Food Food) : IDomainEvent;
public record FoodIngredientsAddedEvent(Food Food, Ingredient[] Ingredients) : IDomainEvent;
public record FoodIngredientsRemovedEvent(Food Food, Ingredient[] Ingredients) : IDomainEvent;

public record PersonCreatedEvent(Person Person) : IDomainEvent;
public record PersonDeletedEvent(Person Person) : IDomainEvent;
public record PersonDisplayNameChangedEvent(Person Person) : IDomainEvent;

public record MealplanWeekChangedEvent(Mealplan Mealplan) : IDomainEvent;
public record MealplanMealslotAddedEvent(Mealplan Mealplan, Meal Meal) : IDomainEvent;
public record MealplanMealSlotClearedEvent(Mealplan Mealplan, MealSlot MealSlot) : IDomainEvent;
public record MealplanMealSlotOverridenEvent(Mealplan Mealplan, Meal Meal) : IDomainEvent;