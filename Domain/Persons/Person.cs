using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;

namespace Domain.Persons;
public class Person : AggregateRoot<Guid?>
{
	public string Displayname { get; private set; }
	public bool Deleted { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Person() : base(null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}
	
	public Person(string displayname, bool deleted) : base(null)
	{
		Displayname = displayname;
		Deleted = deleted;

		this.AddDomainEvent(new PersonCreatedEvent(this));
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
