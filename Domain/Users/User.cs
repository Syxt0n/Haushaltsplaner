using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Persons;
using Domain.Users.Events;

namespace Domain.Users;
public class UserAggregate : AggregateRoot<UserAggregate, Guid>
{
	public Guid ID { get; private set; }
	public PersonAggregate Person { get; private set; }
	public string Username { get; private set; }
	public string Password { get; private set; }
	public JsonObject UserSettings { get; private set; }
	public bool Deleted { get; private set; }

	public UserAggregate(PersonAggregate person, string username, string password, JsonObject usersettings, bool deleted)
	{
		Person = person;
		Username = username;
		Password = password;
		UserSettings = usersettings;
		Deleted = deleted;
	}

	public UserAggregate(Guid id, PersonAggregate person, string username, string password, JsonObject usersettings, bool deleted)
	{
		ID = id;
		Person = person;
		Username = username;
		Password = password;
		UserSettings = usersettings;
		Deleted = deleted;
	}

	public void ChangeUsername(string username)
	{
		if (string.IsNullOrEmpty(username))
			return;

		Username = username;
		AddDomainEvent(new UsernameChangedEvent(this));
	}

	public void ChangePassword(string password)
	{
		if (string.IsNullOrEmpty(password))
			return;

		Password = password;
		AddDomainEvent(new UserPasswordChangedEvent(this));
	}

	public void ChangeUserSettings(JsonObject usersettings)
	{
		UserSettings = usersettings;
		AddDomainEvent(new UserSettingsChangedEvent(this));
	}

	public void Delete()
	{
		Deleted = true;
		AddDomainEvent(new UserDeletedEvent(this));
	}
}
