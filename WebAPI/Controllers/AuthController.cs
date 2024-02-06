using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Users;
using Application.EFCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using WebAPI.Controllers.RequstBodies;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class authController : WebController<authController>
{
    public authController(ILogger<authController> logger, HpContext context, IConfiguration config) : base(logger, context, config)
    {
    }

    [HttpPost("login/{ID}")]
	public ActionResult Login(Guid ID, [FromBody]string password)
	{
		User? loginUser = Context.Users.SingleOrDefault(u => u.Id == ID);
		if (loginUser?.Password == password)
			return Ok(generateJasonWebToken(loginUser));
		else
			return ValidationProblem();
	}

	private string generateJasonWebToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(Config["JwtSettings:Key"]);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Role, user.Role.ToString()),
				new Claim(ClaimTypes.Name, user.Person is not null ? user.Person.Displayname : ""),
				new Claim(ClaimTypes.NameIdentifier, user.Username)
			}),
			IssuedAt = DateTime.UtcNow,
			Expires = DateTime.UtcNow.AddMinutes(30),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			Issuer = Config["JwtSettings:Issuer"]
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
};
