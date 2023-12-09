using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;

namespace Domain.Users.Events;
public class UserDeletedEvent : DomainEvent<UserAggregate>
{
	public UserDeletedEvent(UserAggregate value) : base(value)
	{
	}
}
