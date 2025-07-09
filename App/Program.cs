// web程序入口
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 添加 Swagger 生成器服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddAuthentication();
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
    app.UseSwaggerUI();

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

    db.Telescopes.Add(new Telescope
    {
        Name = "Default Telescope",
        Latitude = 39.90,
        Longitude = 116.40,
        IsOnline = true,
        Description = "系统初始化创建的望远镜"
    });
    await db.SaveChangesAsync();

}

// 映射默认路由（如 /index.html）
app.MapFallbackToFile("index.html"); // 映射到 wwwroot/index.html

app.UseHttpsRedirection();

app.Run();

