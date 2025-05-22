using NewsManagerAPI.Models;
using NewsManagerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Конфігурація ключа API з appsettings.json -> NewsApiOptions
builder.Services.Configure<NewsApiOptions>(
    builder.Configuration.GetSection("NewsApi")
);

// 📡 HTTP-клієнт + DI сервісу з інжекцією ключа
builder.Services.AddHttpClient<INewsService, NewsService>();

// 📦 Контролери + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🌐 CORS для бота або фронту
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🧪 Swagger лише для девелопменту
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();