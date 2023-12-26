using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Foods;
using Domain.Persons;
using Domain.Mealplans;
using DomainBase.Domain;
using Domain.Shoppinglists;
using Domain.Calendars;
using Domain.Choreplans;

namespace Domain.Shared;
public record FoodCreatedEvent(Food Food) : IDomainEvent;
public record FoodNameChangedEvent(Food Food) : IDomainEvent;
public record FoodDeletedEvent(Food Food) : IDomainEvent;
public record FoodIngredientsAddedEvent(Food Food, Ingredient[] Ingredients) : IDomainEvent;
public record FoodIngredientsRemovedEvent(Food Food, Ingredient[] Ingredients) : IDomainEvent;

public record PersonCreatedEvent(Person Person) : IDomainEvent;
public record PersonDeletedEvent(Person Person) : IDomainEvent;
public record PersonDisplayNameChangedEvent(Person Person) : IDomainEvent;

public record MealplanCreatedEvent(Mealplan Mealplan) : IDomainEvent;
public record MealplanWeekChangedEvent(Mealplan Mealplan) : IDomainEvent;
public record MealplanMealAddedEvent(Mealplan Mealplan, Meal Meal) : IDomainEvent;
public record MealplanMealSlotClearedEvent(Mealplan Mealplan, MealSlot MealSlot) : IDomainEvent;
public record MealplanMealSlotOverridenEvent(Mealplan Mealplan, Meal Meal) : IDomainEvent;
public record MealPlanExportedToShoppinglistEvent(Mealplan Mealplan, Shoppinglist Shoppinglist) : IDomainEvent;

public record ShoppinglistCreatedEvent(Shoppinglist Shoppinglist) : IDomainEvent;
public record ShoppinglistArticleAddedEvent(Shoppinglist Shoppinglist) : IDomainEvent;
public record ShoppinglistArticleRemovedEvent(Shoppinglist Shoppinglist) : IDomainEvent;
public record ShoppinglistArticleSwappedEvent(Shoppinglist Shoppinglist) : IDomainEvent;
public record ShoppinglistArticlesOverridenChangedEvent(Shoppinglist Shoppinglist, List<Article> overridenArticles) : IDomainEvent;

public record AppointmentCreatedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentTitleChangedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentDescriptionChangedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentDateChangedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentTimeRangeChangedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentReminderAddedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentReminderRemovedEvent(Appointment Appointment) : IDomainEvent;
public record AppointmentDoneEvent(Appointment Appointment) : IDomainEvent;

public record ChoreplanCreatedEvent(Choreplan Choreplan) : IDomainEvent;
public record ChoreplanWeekChangedEvent(Choreplan choreplan) : IDomainEvent;
public record ChoreplanAssignmentOverridenEvent(Choreplan choreplan, Assignment chore) : IDomainEvent;
public record ChoreplanAssignmentAddedEvent(Choreplan choreplan, Assignment chore) : IDomainEvent;
public record ChoreplanChoreplanSlotClearedEvent(Choreplan choreplan, ChoreplanSlot chore) : IDomainEvent;

public record CalendarCreatedEvent(Calendar Calendar) : IDomainEvent;
public record CalendarAppointmentAddedEvent(Calendar Calendar, List<Appointment> Appointments) : IDomainEvent;