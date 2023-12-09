using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DDD_Base.Domain;
using DDD_Base.Events;
using Domain.Persons;
using Domain.Shared;
using MediatR;
using Npgsql;

namespace Infrastructure.Repositories;

public struct PersonRow
{
	public Guid PersonID;
	public string Displayname;
	public bool Deleted;
}

public class PersonRepository : BaseRepository
{
	public PersonRepository(string ConnectionString, Mediator Mediator) : base(ConnectionString, Mediator){}

	private async Task<List<PersonAggregate>> callDb(string SQL)
	{
		List<PersonAggregate> result = [];
		PersonRow[] Rows = [];
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
						Rows.Append(new PersonRow
						{
							PersonID = reader.GetGuid(0),
							Displayname = reader.GetString(1),
							Deleted = reader.GetBoolean(2),
						});
					}
				}
			}
		}

		for (int iCount = 0; iCount < Rows.Count(); iCount++)
		{
			Guid Id = Rows[iCount].PersonID;
			string Displayname = Rows[iCount].Displayname;
			bool Deleted = Rows[iCount].Deleted;

			result.Add(new PersonAggregate(Id, Displayname, Deleted));
		}

		return result;
	}

	private async Task<List<PersonAggregate>> callDb(string SQL, NpgsqlParameter[] Params)
	{
		List<PersonAggregate> result = [];
		PersonRow[] Rows = [];
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
						Rows.Append(new PersonRow
						{
							PersonID = reader.GetGuid(0),
							Displayname = reader.GetString(1),
							Deleted = reader.GetBoolean(2),
						});
					}
				}
			}
		}

		for (int iCount = 0; iCount < Rows.Count(); iCount++)
		{
			Guid Id = Rows[iCount].PersonID;
			string Displayname = Rows[iCount].Displayname;
			bool Deleted = Rows[iCount].Deleted;

			result.Add(new PersonAggregate(Id, Displayname, Deleted));
		}

		return result;
	}

	public async Task AddAsync(PersonAggregate[] aggregates)
	{
		List<Task> calls = [];
		List<Task> events = [];
		string sql = "";
		NpgsqlParameter[] Params = [];

		foreach (var value in aggregates)
		{
			sql = "CALL main.spInsertPerson(@Displayname)";
			Params.Append(new NpgsqlParameter<string>("Displayname", value.Displayname));

			calls.Add(executeDb(sql, Params));
			foreach (var Event in value.DomainEvents)
				events.Add(mediator.Publish(Event));

			value.ClearDomainEvents();
		}

		await Task.WhenAll(calls);
		await Task.WhenAll(events);
	}

	public async Task<List<PersonAggregate>> GetByIdAsync(Guid[] ids)
	{
		string sql = " SELECT p.Id, p.Displayname, p.Deleted"
					+" FROM main.Persons p"
					+" WHERE p.ID IN [";

		foreach (Guid id in ids)
			sql += $" '{id}',";

		sql += "'0'];";

		return await callDb(sql);
	}

	public async Task<List<PersonAggregate>> GetBySQL(string WhereSQL, NpgsqlParameter[] Params)
	{
		string sql = " SELECT p.Id, p.Displayname, p.Deleted"
					+ " FROM main.Persons p"
					+ WhereSQL
					+ " ORDER BY p.ID;";

		return await callDb(sql, Params);
	}

	public async Task UpdateAsync(PersonAggregate[] aggregates)
	{
		List<Task> calls = [];
		List<Task> events = [];
		string sql = "CALL main.spUpdatePerson(@ID, @Displayname, @Deleted)";
		NpgsqlParameter[] Params = new NpgsqlParameter[3];

		foreach (var value in aggregates)
		{
			Params[0] = new NpgsqlParameter<Guid>("ID", value.Id);
			Params[1] = new NpgsqlParameter<string>("Displayname", value.Displayname);
			Params[2] = new NpgsqlParameter<bool>("Deleted", value.Deleted);

			calls.Add(executeDb(sql, Params));
			foreach (var Event in value.DomainEvents)
				events.Add(mediator.Publish(Event));

			value.ClearDomainEvents();
		}
		await Task.WhenAll(calls);
	}
}