using System.CodeDom.Compiler;
using Application.EFCore;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.RequstBodies;

namespace WebAPI;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : WebController<UsersController>
{
    public UsersController(ILogger<UsersController> logger, HpContext context, IConfiguration config) : base(logger, context, config)
    {
    }

	[HttpPost("/Users")]
	public ActionResult<User> CreateUsers(CreateUser[] input)
	{
		try
		{
			User[] results = [];
			foreach	(CreateUser data in input)
			{
				if (!data.Validate())
					continue;

				User newData = new User(
					data.Username, 
					data.Password, 
					data.Person, 
					data.userrole);

				Context.Add(newData);
				results.Append(newData);
			}
		
			Context.SaveChanges();
			return Ok(results);
		}
		catch (System.Exception)
		{
			return BadRequest();
		}
		
	}

    [HttpGet]
	public ActionResult<List<User>> GetUsers()
	{
		return Ok(Context.Users.ToList());
	}

	[HttpGet("{ID}")]
	public ActionResult<User> GetUsers(Guid ID)
	{
		return Ok(Context.Users.SingleOrDefault(u => u.Id == ID));
	}

	[HttpPut("{ID}")]
	public ActionResult<User?> UpdateUser(Guid ID, [FromBody]CreateUser input)
	{
		User? dbUser = Context.Users.SingleOrDefault(u => u.Id == ID);
		if (input.Validate() && dbUser is not null)
		{
			if (updateUser(dbUser, input))
			{
				Context.SaveChanges();
				return Ok(dbUser);
			}
			else
				return Conflict();
		}
		else
			return BadRequest();
	}

	[HttpDelete("{ID}")]
	public ActionResult DeleteUser(Guid ID)
	{
		Context.Users.Remove(Context.Users.Single(u => u.Id == ID));
		Context.SaveChanges();
		return Ok();
	}


	private bool updateUser(User user, CreateUser input)
	{
		try
		{
			user.ChangeUsername(input.Username);
			user.ChangePassword(input.Password);
			user.Person.ChangeDisplayName(input.Person.Displayname);
			user.ChangeUserRole(input.userrole);
			return true;
		}
		catch (System.Exception)
		{
			return false;
		}
	}
}
