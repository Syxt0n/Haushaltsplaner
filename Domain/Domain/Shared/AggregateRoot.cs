using Swaksoft.Domain.Seedwork.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swaksoft.Domain.Seedwork.Aggregates;

namespace Domain.Shared;
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyList<IDomainEvent> DomainEvents
    {
        get => this._domainEvents.AsReadOnly();
    }

    protected AggregateRoot() { }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        this._domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        this._domainEvents.Clear();
    }
}