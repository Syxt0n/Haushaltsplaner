using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Persons.Events;

namespace Domain.Persons;
public class PersonAggregate : AggregateRoot<PersonAggregate, Guid>
{
	public Guid ID { get; private set; }
	public string Displayname { get; private set; }
	public User? User { get; private set; }
	public bool Deleted { get; private set; }

	public PersonAggregate(string displayname, bool deleted)
	{
		Displayname = displayname;
		Deleted = deleted;

		this.AddDomainEvent(new PersonCreatedEvent(this));
	}

	public PersonAggregate(string displayname, User user, bool deleted)
	{
		Displayname = displayname;
		User = user;
		Deleted = deleted;

		this.AddDomainEvent(new PersonCreatedEvent(this));
	}

	public PersonAggregate(Guid id, string displayname, User user, bool deleted)
	{
		ID = id;
		Displayname = displayname;
		User = user;
		Deleted = deleted;
	}

	public void ChangeDisplayName(string displayname)
	{
		if (string.IsNullOrEmpty(displayname))
			return;

		Displayname = displayname;

		this.AddDomainEvent(new PersonDisplayNameChangedEvent(this));
	}

	public void ConnectUser(User user)
	{
		User = user;
		this.AddDomainEvent(new PersonUserConnectedEvent(this));
	}

	public void RemoveUser()
	{
		User = null;
		this.AddDomainEvent(new PersonUserRemovedEvent(this));
	}

	public void Delete()
	{
		Deleted = true;
		this.AddDomainEvent(new PersonDeletedEvent(this));
	}

	public bool ChangeUserName(string Username)
	{
		if (User is null)
			return false;

		if (User.ChangeUsername(Username))
		{
			this.AddDomainEvent(new PersonUsernameChangedEvent(this));
			return true;
		}
		else
			return false;
	}

	public bool ChangeUserPassword(string UserPassword)
	{
		if (User is null)
			return false;

		if (User.ChangePassword(UserPassword))
		{
			this.AddDomainEvent(new PersonUserPasswordChangedEvent(this));
			return true;
		}
		else 
			return false;
	}
}
