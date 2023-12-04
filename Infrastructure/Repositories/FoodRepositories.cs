using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Food;
using Domain.Repositories;
using Npgsql;

namespace Infrastructure.Repositories;
public class FoodRepositories : IFoodRepository
{
	private string connectionString = "";
	public FoodRepositories(string connectionstring)
	{
		connectionString = connectionstring;
	}

	public async Task AddAsync(FoodAggregate aggregate)
	{
		string[] items = [];
		int[] amounts = [];

		foreach (Ingredient ingredient in aggregate.Ingredients)
		{
			items.Append(ingredient.Item.Name);
			amounts.Append(ingredient.Amount);
		}

		string sql = "CALL main.spinsertfood(@FoodName, @Ingredientnames, @Ingredientcounts)";

		using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
		{
			await connection.OpenAsync();

			using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@FoodName", aggregate.Name);
				command.Parameters.AddWithValue("@Ingredientnames", items);
				command.Parameters.AddWithValue("@Ingredientcounts", amounts);

				command.ExecuteNonQuery();
			}
		}
	}

	public async Task<FoodAggregate> GetByIdAsync(Guid id)
	{
		string sql = "SELECT * "
					+ "FROM main.Food f "
					+ "INNER JOIN main.Ingredients ing ON f.ID = ing.id_food "
					+ "INNER JOIN main.Items i ON i.ID = ing.id_item "
					+ "WHERE f.ID = @FoodID; ";

		using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
		{
			await connection.OpenAsync();

			using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@FoodID", id);

				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						return new FoodAggregate { ID = }
					}
				}
			}
		}
	}

	public async Task<List<FoodAggregate>> GetBySQL(string sql)
	{
		throw new NotImplementedException();
	}

	public async Task<List<FoodAggregate>> GetFoodByName(string FootName)
	{
		throw new NotImplementedException();
	}

	public async Task RemoveAsync(FoodAggregate aggregate)
	{
		throw new NotImplementedException();
	}

	public async Task UpdateAsync(FoodAggregate aggregate)
	{
		throw new NotImplementedException();
	}
}
