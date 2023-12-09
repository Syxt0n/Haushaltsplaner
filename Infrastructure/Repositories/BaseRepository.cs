using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD_Base.Domain;
using MediatR;
using Npgsql;

namespace Infrastructure.Repositories;
public abstract class BaseRepository
{
	protected string connectionString;
	protected NpgsqlConnection? dbCon;
	protected NpgsqlCommand? command;
	protected Mediator mediator;

	public BaseRepository(string ConnectionString, Mediator Mediator)
	{
		connectionString = ConnectionString;
		mediator = Mediator;
	}

	protected async Task executeDb(string SQL, NpgsqlParameter[] Params)
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

	protected async Task executeDb(string SQL)
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
}