using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Persons.Events;
public class PersonUserConnectedEvent : DomainEvent<PersonAggregate>
{
	public PersonUserConnectedEvent(PersonAggregate value) : base(value)
	{
	}
}
