using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainBase.Domain;

public interface IRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
	Task<TAggregate> GetByIdAsync(TId id);
	Task<List<TAggregate>> GetBySQL(string sql);
	Task AddAsync(TAggregate aggregate);
	Task UpdateAsync(TAggregate aggregate);
	Task RemoveAsync(TAggregate aggregate);
}
