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
using Domain.Users;
using Domain.Shared;
using MediatR;
using Npgsql;
using System.Runtime.InteropServices.JavaScript;
using System.Data;

namespace Infrastructure.Repositories;

public struct UserRow
{
	public Guid ID;
	public string Username;
	public string Password;
	public JsonObject Settings;
	public bool UserDeleted;
	public Guid PersonID;
	public string Displayname;
	public bool PersonDeleted;
}

public class UserRepository: BaseRepository
{
	public UserRepository(string ConnectionString, Mediator Mediator) : base(ConnectionString, Mediator){}

	private async Task<List<UserAggregate>> callDb(string SQL)
	{
		List<UserAggregate> result = [];
		UserRow[] Rows = [];
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
						Rows.Append(new UserRow
						{
							ID = reader.GetGuid(0),
							Username = reader.GetString(1),
							Password = reader.GetString(2),
							Settings = reader.GetFieldValue<JsonObject>(3),
							UserDeleted = reader.GetBoolean(4),
							PersonID = reader.GetGuid(5),
							Displayname = reader.GetString(6),
							PersonDeleted = reader.GetBoolean(7)
						});
					}
				}
			}
		}

		for (int iCount = 0; iCount < Rows.Count(); iCount++)
		{
			Guid Id = Rows[iCount].ID;
			string Username = Rows[iCount].Username;
			string Password = Rows[iCount].Password;
			JsonObject Settings = Rows[iCount].Settings;
			bool UserDeleted = Rows[iCount].UserDeleted;
			Guid PersonID = Rows[iCount].PersonID;
			string Displayname = Rows[iCount].Displayname;
			bool PersonDeleted = Rows[iCount].PersonDeleted;

			result.Add(new UserAggregate(Id, new PersonAggregate(PersonID, Displayname, PersonDeleted), Username, Password, Settings, UserDeleted));
		}

		return result;
	}

	private async Task<List<UserAggregate>> callDb(string SQL, NpgsqlParameter[] Params)
	{
		List<UserAggregate> result = [];
		UserRow[] Rows = [];
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
						Rows.Append(new UserRow
						{
							ID = reader.GetGuid(0),
							Username = reader.GetString(1),
							Password = reader.GetString(2),
							Settings = reader.GetFieldValue<JsonObject>(3),
							UserDeleted = reader.GetBoolean(4),
							PersonID = reader.GetGuid(5),
							Displayname = reader.GetString(6),
							PersonDeleted = reader.GetBoolean(7)
						});
					}
				}
			}
		}

		for (int iCount = 0; iCount < Rows.Count(); iCount++)
		{
			Guid Id = Rows[iCount].ID;
			string Username = Rows[iCount].Username;
			string Password = Rows[iCount].Password;
			JsonObject Settings = Rows[iCount].Settings;
			bool UserDeleted = Rows[iCount].UserDeleted;
			Guid PersonID = Rows[iCount].PersonID;
			string Displayname = Rows[iCount].Displayname;
			bool PersonDeleted = Rows[iCount].PersonDeleted;

			result.Add(new UserAggregate(Id, new PersonAggregate(PersonID, Displayname, PersonDeleted), Username, Password, Settings, UserDeleted));
		}

		return result;
	}

	public async Task AddAsync(UserAggregate[] aggregates)
	{
		List<Task> calls = [];
		List<Task> events = [];
		string sql = "";
		NpgsqlParameter[] Params = new NpgsqlParameter[4];

		foreach (var value in aggregates)
		{
			sql = "CALL main.spInsertUser(@Username, @Password, @UserSettings, @Displayname)";
			Params = [
				new NpgsqlParameter<string>("Username", value.Username),
				new NpgsqlParameter<string>("Password", value.Password),
				new NpgsqlParameter<JsonObject>("UserSettings", value.UserSettings),
				new NpgsqlParameter<string>("Displayname", value.Person.Displayname)
			];

			calls.Add(executeDb(sql, Params));
			foreach (var Event in value.DomainEvents)
				events.Add(mediator.Publish(Event));

			value.ClearDomainEvents();
		}

		await Task.WhenAll(calls);
		await Task.WhenAll(events);
	}

	public async Task<List<UserAggregate>> GetByIdAsync(Guid[] ids)
	{
		string sql = " SELECT u.Id, u.Username, u.Password, u.Settings, u.Deleted p.ID, p.Displayname, p.Deleted"
					+" FROM main.Users u"
					+" JOIN main.Persons p ON u.ID_Person = p.ID"
					+" WHERE u.ID IN [";

		foreach (Guid id in ids)
			sql += $" '{id}',";

		sql += "'0'];";

		return await callDb(sql);
	}

	public async Task<List<UserAggregate>> GetBySQL(string WhereSQL, NpgsqlParameter[] Params)
	{
		string sql = " SELECT u.Id, u.Username, u.Password, u.Settings, u.Deleted, p.ID, p.Displayname, p.Deleted"
					+" FROM main.Users u"
					+" JOIN main.Persons p ON u.ID_Person = p.ID"
					+ WhereSQL
					+" ORDER BY p.ID;";

		return await callDb(sql, Params);
	}

	public async Task UpdateAsync(UserAggregate[] aggregates)
	{
		List<Task> calls = [];
		List<Task> events = [];
		string sql = "CALL main.spUpdateUser(@UserID, @Username @Password, @Deleted)";
		NpgsqlParameter[] Params = new NpgsqlParameter[4];

		foreach (var value in aggregates)
		{
			Params[0] = new NpgsqlParameter<Guid>("ID", value.Id);
			Params[1] = new NpgsqlParameter<string>("Username", value.Username);
			Params[2] = new NpgsqlParameter<string>("Password", value.Password);
			Params[3] = new NpgsqlParameter<bool>("Deleted", value.Deleted);

			calls.Add(executeDb(sql, Params));
			foreach (var Event in value.DomainEvents)
				events.Add(mediator.Publish(Event));

			value.ClearDomainEvents();
		}
		await Task.WhenAll(calls);
	}
}