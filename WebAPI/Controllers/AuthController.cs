using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Users;
using Application.EFCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private HpContext Context;

    public AuthController(ILogger<AuthController> logger, HpContext context)
    {
        _logger = logger;
        Context = context;
    }

    [HttpPost("login")]
    public ActionResult Login(string username, string password)
    {
        if (getUserByUsername(username)?.Password == password)
            Ok(generateJasonWebToken());
        else
            ValidationProblem();        
    }

    private User? getUserByUsername(string username)
    {
        return Context.Users.Where(u => u.Username == username).First();
    }

    private string generateJasonWebToken()
    {
        List<Claim> claims = new List<Claim> 
        {
            new Claim(ClaimTypes.Name, "test")
        };
    }
};
