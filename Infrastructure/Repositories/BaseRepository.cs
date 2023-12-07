using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Infrastructure.Repositories;
public abstract class BaseRepository
{
	protected string connectionString;
	protected NpgsqlConnection? dbCon;
	protected NpgsqlCommand? command;
}