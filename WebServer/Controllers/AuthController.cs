using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using WebServer.Data;
using WebServer.Helpers;
using WebServer.Models;

namespace WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly WebServerDbContext _db;

    public AuthController(WebServerDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public IActionResult Register([FromForm] string username, [FromForm] string password)
    {
        if (_db.Users.Any(u => u.Username == username))
            return BadRequest("Username already exists.");

        var salt = PasswordHelper.GenerateSalt();
        var hash = PasswordHelper.HashPassword(password, salt);

        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            Salt = salt
        };

        _db.Users.Add(user);
        _db.SaveChanges();
        return Ok("Registered.");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null) return Unauthorized();

        var inputHash = PasswordHelper.HashPassword(password, user.Salt);

        if (inputHash != user.PasswordHash)
            return Unauthorized();

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return Ok("Logged in");
    }


    [HttpGet("me")]
    public IActionResult Me()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized();

        return Ok(new
        {
            Username = User.Identity.Name,
            Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("Logged out");
    }
}
