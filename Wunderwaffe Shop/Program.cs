using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wunderwaffe_Shop.Models;
using Wunderwaffe_Shop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicjalizacja danych po uruchomieniu aplikacji
using (var scope = app.Services.CreateScope())
{
    // Pobranie kontekstu bazy danych
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Uruchomienie migracji i stworzenie bazy danych (jeœli nie istnieje)
    await context.Database.MigrateAsync(); // Migracje bazy danych

    // Sprawdzenie, czy w tabeli 'Products' istniej¹ ju¿ dane
    if (!await context.Products.AnyAsync()) // Jeœli tabela Products jest pusta
    {
        // Wykonanie skryptu SQL tylko jeœli tabela jest pusta
        var seedDataPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SeedData.sql");
        if (File.Exists(seedDataPath))
        {
            var sql = await File.ReadAllTextAsync(seedDataPath);
            await context.Database.ExecuteSqlRawAsync(sql);  // Asynchroniczne wykonanie zapytania SQL
        }
    }
}

app.Run();
