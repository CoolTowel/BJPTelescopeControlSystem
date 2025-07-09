
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace App.Controllers;
public record RegisterRequest(string Username, string Password, string Email, string? DisplayName);
public record LoginRequest(string Username, string Password);


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var user = new ApplicationUser
            {
            UserName = req.Username,
            Email = req.Email,
            DisplayName = req.DisplayName,
            CreatedAt = DateTime.UtcNow
        }; 
        var result = await _userManager.CreateAsync(user, req.Password);
        if (result.Succeeded)
        {
            // 可以在这里添加角色分配等逻辑
            return Ok("注册成功");
        }
        return BadRequest("注册失败: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
    //[HttpPost("login")]
    //public async Task<IActionResult> Login([FromBody] LoginRequest req)
    //{
    //    var user = await _userManager.FindByNameAsync(req.Username);
    //    if (user == null || !await _userManager.CheckPasswordAsync(user, req.Password))
    //        return Unauthorized("用户名或密码错误");

    //    await _signInManager.SignInAsync(user, isPersistent: true);

    //    var roles = await _userManager.GetRolesAsync(user);
    //    return Ok(new { username = user.UserName, role = roles.FirstOrDefault() ?? "User" });
    //}

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await _signInManager.PasswordSignInAsync(req.Username, req.Password, true, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(req.Username);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Admin");
            if (isAdmin)
            {
                Console.WriteLine("是管理员");
            }
            
            //return Ok("登录成功");
            return Ok(new { username = user.UserName, isAdmin });

        }

        return Unauthorized("登录失败");
    }
}