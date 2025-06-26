// web程序入口
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// 添加 Swagger 生成器服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 启用静态文件服务（wwwroot）
app.UseDefaultFiles(); // 自动寻找 index.html 等默认页面
app.UseStaticFiles();  // 允许访问 wwwroot 中的文件

// 启用路由中间件（必须有）
app.UseRouting();

// 映射 API 控制器（如 /api/xxx）
app.MapControllers();

// 启用 Swagger 中间件
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 映射默认路由（如 /index.html）
app.MapFallbackToFile("index.html"); // 映射到 wwwroot/index.html

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();