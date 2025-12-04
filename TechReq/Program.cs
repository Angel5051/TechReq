using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore; // Необходимо добавить для метода UseNpgsql
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure; // Необходим для доступа к опциям Npgsql
using TeachRed.Domain;
using TechReq;
using TechReq.DAL; // Пространство имен, где находится ApplicationDbContext

// ... возможно, другие using, если ApplicationDbContext требует их

var builder = WebApplication.CreateBuilder(args);

// --- 1. Конфигурация базы данных PostgreSQL ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Проверка: Убедитесь, что строка подключения найдена
if (string.IsNullOrEmpty(connectionString))
{
    // В случае отсутствия строки подключения (для отладки)
    throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена. Проверьте appsettings.json.");
}

// Регистрация ApplicationDbContext с использованием Npgsql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, b =>
    {
        // ⬇️ ВАЖНО: Явно указываем сборку (проект), где EF Core должен создавать миграции.
        // Это устраняет ошибку "Target project doesn't match migrations assembly".
        b.MigrationsAssembly("TechReq.DAL");
    });
});
// --- Конец конфигурации базы данных ---

// Add services to the container.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/Account/Logout");
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
        options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
        options.Scope.Add("profile");
        options.ClaimActions.MapJsonKey("picture", "picture");
    });


builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();