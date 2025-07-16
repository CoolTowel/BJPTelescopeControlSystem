// web程序入口
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 添加 Swagger 生成器服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "望远镜控制系统 API",
        Version = "v1",
        Description = "望远镜控制系统的API文档"
    });
});

// 添加数据库上下文
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string"
    + "'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString)); // SQLite 文件
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 配置身份验证
//builder.Services.AddAuthentication();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/login.html";
    options.Cookie.Name = "auth_cookie";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    //options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "auth_cookie";
    options.LoginPath = "/login.html";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    //options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// 添加 CORS 策略
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("ProductionCors", policy =>
//    {
//        policy.WithOrigins("https://portal.example.com") // ✅ 必须指定前端站点
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials(); // ✅ 必须启用才能让 cookie 生效
//    });
//    options.AddPolicy("DevCors", policy =>
//    {
//        policy.WithOrigins("http://localhost:5173") // 前端 dev 服务器地址
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials(); // 允许携带 Cookie
//    });
//});



var app = builder.Build();

//if (app.Environment.IsDevelopment()) // 注意顺序：要在 app.UseAuthorization() 之前
//{
//    app.UseCors("DevCors"); // 允许 localhost
//}
//else
//{
//    app.UseCors("ProductionCors"); // 只允许正式前端域名
//}


// 启用路由中间件（必须有）
app.UseRouting();

// 启用身份验证中间件
app.UseAuthentication();
app.UseAuthorization();

// 启用静态文件服务（wwwroot）
app.UseDefaultFiles(); // 自动寻找 index.html 等默认页面
app.UseStaticFiles();  // 允许访问 wwwroot 中的文件

// 映射 API 控制器（如 /api/xxx）
app.MapControllers();

// 若为开发环境，启用 Swagger 中间件，并创建数据库，新建admin用户，新建telescope
if (app.Environment.IsDevelopment())
{
    // 启用 Swagger 中间件
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "望远镜控制系统 API v1");
        c.RoutePrefix = "swagger";
    });

    //删除旧的数据库文件（如果存在）
    if (File.Exists("Data/WebServer.db"))
    {
        File.Delete("Data/WebServer.db");
        File.Delete("Data/WebServer.db-shm");
        File.Delete("Data/WebServer.db-wal");
        Console.WriteLine("已删除旧的数据库文件 WebServer.db");
    }

    // 创建新数据库并添加管理员用户
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string adminEmail = "admin@example.com";
    string adminUserName = "admin";
    string adminPassword = "Admin123!"; // ✅ 强密码

    // 创建角色（如无）
    await roleManager.CreateAsync(new IdentityRole("Admin"));
    await roleManager.CreateAsync(new IdentityRole("User"));


    // 检查管理员是否存在
    var adminUser = await userManager.FindByNameAsync(adminUserName);
    var newAdmin = new ApplicationUser
    {
        UserName = adminUserName,
        Email = adminEmail,
        EmailConfirmed = true,
    };

    await userManager.CreateAsync(newAdmin, adminPassword);
    await userManager.AddToRoleAsync(newAdmin, "Admin");
    Console.WriteLine("管理员账户已创建");

    var testUserPassword = "Test123!"; // 测试用户密码
    var testUser = new ApplicationUser
    {
        UserName = "test",
        Email = "test@test.com",
        EmailConfirmed = true
    };
    await userManager.CreateAsync(testUser, testUserPassword);
    await userManager.AddToRoleAsync(newAdmin, "User");
    Console.WriteLine("测试账户已创建");

    db.Telescopes.Add(new Telescope
    {
        Name = "Dream 16",
        Latitude = 29.011925,
        Longitude = 100.227752,
        IsOnline = true,
        Description = "稻城Dream 16英寸牛反，ASI6200MM相机"
    });
    db.Telescopes.Add(new Telescope
    {
        Name = "EF400 2.8",
        Latitude = 26.722592,
        Longitude = 100.029570,
        IsOnline = true,
        Description = "丽江EF400，ASI2600MM相机"
    });
    await db.SaveChangesAsync();
}

// 映射默认路由（如 /index.html）
app.MapFallbackToFile("index.html"); // 映射到 wwwroot/index.html


// 只在生产环境启用HTTPS重定向
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}


app.Run();

