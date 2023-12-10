using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Mealplans;
using Domain.Shared;
using MediatR;
using Npgsql;

namespace Infrastructure.Repositories;

public struct MealplanRow
{
	
}

public class MealplanRepository : BaseRepository
{
	public MealplanRepository(string ConnectionString, Mediator Mediator) : base(ConnectionString, Mediator){}

	private async Task<List<MealplanAggregate>> callDb(string SQL, NpgsqlParameter[] Params)
	{
		List<MealplanAggregate> result = [];
		FoodRow[] Rows = [];
		using (dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();
			using (var command = new NpgsqlCommand(SQL, dbCon))
			{
				foreach (var param in Params)
					command.Parameters.Add(param);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return result;

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

	private async Task<List<MealplanAggregate>> callDb(string SQL)
	{
		List<MealplanAggregate> result = [];
		FoodRow[] Rows = [];
		using (dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();
			using (var command = new NpgsqlCommand(SQL, dbCon))
			{
				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return result;

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

			result.Add(new MealplanAggregate(Id, Name, Ingredients, Deleted));

			iCount = SubCount - 1; // iCount wird mit dem nächsten Schleifendurchgang um eins hochgezählt
		}

		return result;
	}

	public async Task AddAsync(MealplanAggregate[] aggregates)
	{

	}

	public async Task<List<MealplanAggregate>?> GetByIdAsync(Guid[] ids)
	{

	}

	public async Task<List<MealplanAggregate>?> GetBySQL(string WhereSQL, NpgsqlParameter[] Params)
	{

	}

	public async Task UpdateAsync(MealplanAggregate[] aggregates)
	{

	}
}
