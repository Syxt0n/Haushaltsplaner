using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Food;

namespace Domain.Repositories;
public interface IFoodRepository : IRepository<FoodAggregate, Guid>
{
	public Task<List<FoodAggregate>> GetFoodByName(string FootName);
}
