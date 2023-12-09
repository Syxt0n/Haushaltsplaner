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
using Domain.Users;

namespace Domain.Persons;
public class PersonAggregate : AggregateRoot<PersonAggregate, Guid>
{
	public Guid ID { get; private set; }
	public string Displayname { get; private set; }
	public bool Deleted { get; private set; }

	public PersonAggregate(string displayname, bool deleted)
	{
		Displayname = displayname;
		Deleted = deleted;

		this.AddDomainEvent(new PersonCreatedEvent(this));
	}

	public PersonAggregate(string displayname, string username, string password, bool deleted)
	{
		Displayname = displayname;
		Deleted = deleted;

		this.AddDomainEvent(new PersonCreatedEvent(this));
	}

	public PersonAggregate(Guid id, string displayname, bool deleted)
	{
		ID = id;
		Displayname = displayname;
		Deleted = deleted;
	}

	public void ChangeDisplayName(string displayname)
	{
		if (string.IsNullOrEmpty(displayname))
			return;

		Displayname = displayname;

		this.AddDomainEvent(new PersonDisplayNameChangedEvent(this));
	}

	public void Delete()
	{
		Deleted = true;
		this.AddDomainEvent(new PersonDeletedEvent(this));
	}
}
