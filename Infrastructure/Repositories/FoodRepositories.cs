using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using Domain.Food;
using Domain.Shared;
using Npgsql;

namespace Infrastructure.Repositories;

public struct FoodRow
{
	public Guid ID;
	public string Name;
	public bool Deleted;
	public string ItemName;
	public int IngredientAmount;
}


public class FoodRepositories : BaseRepository
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

		NpgsqlParameter<string> ParamFoodName = new NpgsqlParameter<string>("FoodName", aggregate.Name);
		NpgsqlParameter<string[]> ParamIngredientNames = new NpgsqlParameter<string[]>("Ingredientnames", items);
		NpgsqlParameter<int[]> ParamIngredientCounts = new NpgsqlParameter<int[]>("Ingredientcounts", amounts);

		using (NpgsqlConnection dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();

			using (var command = new NpgsqlCommand(sql, dbCon))
			{
				command.Parameters.Add(ParamFoodName);
				command.Parameters.Add(ParamIngredientNames);
				command.Parameters.Add(ParamIngredientCounts);

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

	public async Task<List<FoodAggregate>?> GetBySQL(string WhereSQL, NpgsqlParameter[] Params)
	{
		List<FoodAggregate> result = [];
		FoodRow[] Rows = [];

		string sql = "SELECT f.ID, f.Name, f.Deleted, ing.Amount, i.Name"
					+ " FROM main.Food f"
					+ " INNER JOIN main.Ingredients ing ON f.ID = ing.id_food"
					+ " INNER JOIN main.Items i ON i.ID = ing.id_item"
					+ WhereSQL
					+ " ORDER BY f.ID;";

		using (dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();
			using (var command = new NpgsqlCommand(sql, dbCon))
			{
				foreach (var param in Params)
				{
					command.Parameters.Add(param);
				}

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return null;

					while (await reader.ReadAsync())
					{
						Rows.Append(new FoodRow
						{
							ID = reader.GetGuid(0),
							Name = reader.GetString(1),
							Deleted = reader.GetBoolean(2),
							IngredientAmount = reader.GetInt32(3),
							ItemName = reader.GetString(4)
						});
					}
				}
			}
		}

		for (int iCount = 0; iCount < Rows.Count(); iCount++)
		{
			Guid Id = Rows[iCount].ID;
			string Name = Rows[iCount].Name;
			bool Deleted = Rows[iCount].Deleted;
			List<Ingredient> Ingredients = [];

			int SubCount = iCount;
			do
			{
				Ingredients.Add(new Ingredient(new Item(Rows[SubCount].ItemName), Rows[SubCount].IngredientAmount));
				SubCount++;
			} while (Id == Rows[SubCount].ID);

			result.Add(new FoodAggregate(Id, Name, Ingredients, Deleted));

			iCount = SubCount - 1; // iCount wird mit dem nächsten Schleifendurchgang um eins hochgezählt
		}

		return result;
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
