using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DDD_Base.Domain;
using Domain.Food;
using Domain.Shared;
using MediatR;
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

/// <summary>
/// Todo: Resolve Domain-Events
/// </summary>
public class FoodRepositories : BaseRepository
{
	public FoodRepositories(string ConnectionString, Mediator Mediator) : base(ConnectionString, Mediator){}

	private async Task<List<FoodAggregate>> callDb(string SQL, NpgsqlParameter[] Params)
	{
		List<FoodAggregate> result = [];
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

	private async Task<List<FoodAggregate>> callDb(string SQL)
	{
		List<FoodAggregate> result = [];
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

			result.Add(new FoodAggregate(Id, Name, Ingredients, Deleted));

			iCount = SubCount - 1; // iCount wird mit dem nächsten Schleifendurchgang um eins hochgezählt
		}

		return result;
	}

	private async Task executeDb(string SQL, NpgsqlParameter[] Params)
	{
		using (dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();
			var t = await dbCon.BeginTransactionAsync();
			try
			{
				using (var command = new NpgsqlCommand(SQL, dbCon))
				{
					foreach (var param in Params)
						command.Parameters.Add(param);

					await command.ExecuteNonQueryAsync();
				}
				await t.CommitAsync();
			}
			catch (Exception)
			{
				await t.RollbackAsync();
				throw;
			}
		}
	}

	private async Task executeDb(string SQL)
	{
		using (dbCon = new NpgsqlConnection(connectionString))
		{
			await dbCon.OpenAsync();
			var t = await dbCon.BeginTransactionAsync();
			try
			{
				using (var command = new NpgsqlCommand(SQL, dbCon))
				{
					await command.ExecuteNonQueryAsync();
				}
				await t.CommitAsync();
			}
			catch (Exception)
			{
				await t.RollbackAsync();
				throw;
			}
		}
	}

	public async Task AddAsync(FoodAggregate[] aggregates)
	{
		List<Task> calls = [];
		List<Task> events = [];
		string sql = "CALL main.spInsertFood(@FoodName, @Ingredientnames, @Ingredientcounts)";

		foreach (var value in aggregates)
		{
			List<string> items = [];
			List<int> amounts = [];
			NpgsqlParameter[] Params = new NpgsqlParameter[3];

			foreach (Ingredient ingredient in value.Ingredients)
			{
				items.Add(ingredient.Item.Name);
				amounts.Add(ingredient.Amount);
			}

			Params[0] = new NpgsqlParameter<string>("FoodName", value.Name);
			Params[1] = new NpgsqlParameter<string[]>("Ingredientnames", items.ToArray());
			Params[2] = new NpgsqlParameter<int[]>("Ingredientcounts", amounts.ToArray());

			calls.Add(executeDb(sql, Params));
			foreach (var Event in value.DomainEvents)
				events.Add(mediator.Publish(Event));

			value.ClearDomainEvents();
		}

		await Task.WhenAll(calls);
		await Task.WhenAll(events);
	}

	public async Task<List<FoodAggregate>?> GetByIdAsync(Guid[] ids)
	{
		string sql = "SELECT f.ID, f.Name, f.Deleted, ing.Amount, i.Name"
					+" FROM main.Food f"
					+" INNER JOIN main.Ingredients ing ON f.ID = ing.id_food"
					+" INNER JOIN main.Items i ON i.ID = ing.id_item"
					+" WHERE f.ID IN [";

		foreach (Guid id in ids)
			sql += $" '{id}',";

		sql += "'0'];";

		return await callDb(sql);
	}

	public async Task<List<FoodAggregate>?> GetBySQL(string WhereSQL, NpgsqlParameter[] Params)
	{
		List<FoodAggregate> result = [];
		FoodRow[] Rows = [];

		string sql = "SELECT f.ID, f.Name, f.Deleted, ing.Amount, i.Name"
					+" FROM main.Food f"
					+" INNER JOIN main.Ingredients ing ON f.ID = ing.id_food"
					+" INNER JOIN main.Items i ON i.ID = ing.id_item "
					+ WhereSQL
					+" ORDER BY f.ID;";

		return await callDb(sql, Params);
	}

	public async Task UpdateAsync(FoodAggregate[] aggregates)
	{
		List<Task> calls = [];
		List<Task> events = [];
		string sql = "CALL main.spUpdateFood(@FoodID, @FoodName, @FoodDeleted, @IngredientNames, @IngredientAmounts)";

		foreach (var value in aggregates)
		{
			List<string> items = [];
			List<int> amounts = [];
			NpgsqlParameter[] Params = new NpgsqlParameter[5];

			foreach (Ingredient ingredient in value.Ingredients)
			{
				items.Add(ingredient.Item.Name);
				amounts.Add(ingredient.Amount);
			}

			Params[0] = new NpgsqlParameter<Guid>("FoodID", value.Id);
			Params[1] = new NpgsqlParameter<string>("FoodName", value.Name);
			Params[2] = new NpgsqlParameter<bool>("FoodDeleted", value.Deleted);
			Params[3] = new NpgsqlParameter<string[]>("IngredientNames", items.ToArray());
			Params[4] = new NpgsqlParameter<int[]>("IngredientAmounts", amounts.ToArray());

			calls.Add(executeDb(sql, Params));
			foreach (var Event in value.DomainEvents)
				events.Add(mediator.Publish(Event));

			value.ClearDomainEvents();
		}
		await Task.WhenAll(calls);
	}
}
