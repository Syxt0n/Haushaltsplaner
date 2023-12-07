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
	protected string connectionString = "Username = Admin; Password=Lindach1210;Host=federlein.website;Port=5432;Database=Haushaltsplaner;Pooling=true;Connection Lifetime = 0;Trust Server Certificate = true";
	protected IDbConnection? dbCon;
	protected NpgsqlCommand? command;


	

}
