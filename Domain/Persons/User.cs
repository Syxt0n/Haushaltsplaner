using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DDD_Base.Domain;

namespace Domain.Persons;
public class User : Entity<Guid>
{
	public Guid ID { get; private set; }
	public string Username { get; private set; }
	public string Password { get; private set; }
	public JsonObject UserSettings { get; private set; }

	public User(string username, string password, JsonObject usersettings)
	{
		Username = username;
		Password = password;
		UserSettings = usersettings;
	}

	public User(Guid id, string username, string password, JsonObject usersettings)
	{
		ID = id;
		Username = username;
		Password = password;
		UserSettings = usersettings;
	}

	public bool ChangeUsername(string username)
	{
		if (String.IsNullOrEmpty(username))
			return false;

		Username = username;
		return true;
	}

	public bool ChangePassword(string password)
	{
		if (String.IsNullOrEmpty(password))
			return false;

		Password = password;
		return true;
	}

	public void SetUserSettings(JsonObject usersettings)
	{
		UserSettings = usersettings;
	}
}
