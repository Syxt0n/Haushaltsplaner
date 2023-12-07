using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Food;
using Domain.Repositories;
using Domain.Shared;
using Npgsql;

namespace Infrastructure.Repositories;
public class FoodRepositories : BaseRepository, IFoodRepository
{
	public FoodRepositories(string connectionstring)
	{
		this.connectionString = connectionstring;
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

	public async Task<FoodAggregate?> GetByIdAsync(Guid id)
	{
		string sql = "SELECT f.ID, f.Name, f.Deleted, ing.Amount, i.Name"
					+ " FROM main.Food f"
					+ " INNER JOIN main.Ingredients ing ON f.ID = ing.id_food"
					+ " INNER JOIN main.Items i ON i.ID = ing.id_item"
					+ " WHERE f.ID = @FoodID;";

		NpgsqlParameter<Guid> IDParam = new NpgsqlParameter<Guid>("FoodID", id);

		using (dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();
			using (var command = new NpgsqlCommand(sql, dbCon))
			{
				command.Parameters.Add(IDParam);
				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return null;

					await reader.ReadAsync();
					Guid ID = reader.GetGuid(0);
					string Name = reader.GetString(1);
					bool Deleted = reader.GetBoolean(2);
					List<Ingredient> Ingredients = [];

					do
					{
						Ingredient ingredientTemp = new Ingredient(new Item(reader.GetString(4)), reader.GetInt32(3));
						Ingredients.Add(ingredientTemp);
					} while (await reader.ReadAsync());

					return new FoodAggregate(ID, Name, Ingredients, Deleted);
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
