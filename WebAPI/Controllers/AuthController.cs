using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Users;
using Application.EFCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private HpContext Context;
    private readonly IConfiguration Config;

    public AuthController(ILogger<AuthController> logger, HpContext context, IConfiguration config)
    {
        _logger = logger;
        Context = context;
        Config = config;
    }

    [HttpPost("login")]
    public ActionResult Login(string username, string password)
    {
        User? loginUser = getUserByUsername(username);
        if (loginUser?.Password == password)
            Ok(generateJasonWebToken(loginUser));
        else
            ValidationProblem();        
    }

    private User? getUserByUsername(string username)
    {
        return Context.Users.Where(u => u.Username == username).First();
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
                new Claim(ClaimTypes.Name, user.Person.Displayname),
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
