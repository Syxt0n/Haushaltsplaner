using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Events;
using Domain.Users;

namespace Domain.Users.Events;
public class UserPasswordChangedEvent : DomainEvent<UserAggregate>
{
    public UserPasswordChangedEvent(UserAggregate value) : base(value)
    {
    }
}
