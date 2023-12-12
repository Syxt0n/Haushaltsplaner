using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainBase.Domain;
public abstract class AggregateRoot<TId> : Entity<TId>
{
	public AggregateRoot(TId? ID) : base(ID){}
}