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

	public Person(string displayname, bool deleted) : base(null)
	{
		Displayname = displayname;
		Deleted = deleted;

		this.AddDomainEvent(new PersonCreatedEvent(this));
	}

	public Person(Guid id, string displayname, bool deleted) : base(id)
	{
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
