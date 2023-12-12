using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainBase.Domain;
public abstract class Entity<TId>
{
	public TId? Id { get; protected set; }

	private readonly List<IDomainEvent> _domainEvents = [];
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

	public Entity(TId? ID)
	{
		Id = ID;
	}

	protected void AddDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}

	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj) || obj.GetType() != GetType())
			return false;

		if (ReferenceEquals(this, obj))
			return true;

		var other = (Entity<TId>)obj;

		return Equals(Id, other.Id);
	}

	public override int GetHashCode()
	{
		return Id?.GetHashCode() ?? 0;
	}
}
