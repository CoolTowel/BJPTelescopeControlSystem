using App.Data;
using App.Helpers;
using App.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// web程序入口
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string"
    + "'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString)); // SQLite 文件

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// 添加 Swagger 生成器服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 启用身份验证中间件
app.UseAuthentication();
app.UseAuthorization();

// 启用静态文件服务（wwwroot）
app.UseDefaultFiles(); // 自动寻找 index.html 等默认页面
app.MapStaticAssets();  // 允许访问 wwwroot 中的文件

// 启用路由中间件（必须有）
app.UseRouting();

// 映射 API 控制器（如 /api/xxx）
app.MapControllers();

// 启用 Swagger 中间件
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //删除旧的数据库文件（如果存在）
    if (File.Exists("Data/WebServer.db"))
    { 
        File.Delete("Data/WebServer.db");
        File.Delete("Data/WebServer.db-shm");
        File.Delete("Data/WebServer.db-wal");
        Console.WriteLine("已删除旧的数据库文件 WebServer.db"); 
    }

    //    // 注册调试用admin账户
    //    using (var scope = app.Services.CreateScope())
    //    {
    //        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //        db.Database.EnsureCreated();

    //        // 如果没有用户，则添加一个默认管理员账号
    //        if (!db.ApplicationUser.Any())
    //        {
    //            var salt = PasswordHelper.GenerateSalt();
    //            var hash = PasswordHelper.HashPassword("password", salt);
    //            db.ApplicationUser.Add(new ApplicationUser
    //            {
    //                Username = "admin",
    //                PasswordHash = hash, // ⚠️ 实际项目必须用哈希存储
    //                Salt = salt,
    //                Role = "Admin"
    //            });
    //            db.SaveChanges();
    //            Console.WriteLine("已创建默认管理员账号 admin / password");
    //        }
    //    }
}

// 映射默认路由（如 /index.html）
app.MapFallbackToFile("index.html"); // 映射到 wwwroot/index.html

app.UseHttpsRedirection();

app.Run();

