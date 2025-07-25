using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            // add to User role
            await _userManager.AddToRoleAsync(user, "User");
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
        var user = await _userManager.FindByNameAsync(req.Username);
        if (user == null)
        {
            return Unauthorized("用户名或密码错误");
        }

        var result = await _signInManager.PasswordSignInAsync(req.Username, req.Password, false, false);
        if (result.Succeeded)
        {
            //return Ok("登录成功");
            return Ok(new { username = user.UserName });
        }
        else
        { return Unauthorized("用户名或密码错误"); }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    [HttpGet("check-login")]
    public IActionResult CheckLogin()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var isAdmin = User.IsInRole("Admin");

            return Ok(new { user = User.Identity.Name, isAdmin });
        }

        return Unauthorized();
    }
}