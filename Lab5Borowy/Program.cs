using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lab5Borowy.Data;
using Lab5Borowy.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Lab5BorowyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Lab5BorowyContext") ?? throw new InvalidOperationException("Connection string 'Lab5BorowyContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas trwania sesji (np. 30 minut)
    options.Cookie.HttpOnly = true; // Zabezpieczenie ciasteczek sesji
    options.Cookie.IsEssential = true; // Sesja jest wymagana do dzia³ania aplikacji
});


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// W³¹czenie obs³ugi sesji
app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    // Jeœli œcie¿ka prowadzi do admina, sprawdŸ rolê
    if (path.StartsWithSegments("/Admin"))
    {
        var userRole = context.Session.GetString("UserRole");
        if (userRole != "Admin")
        {
            context.Response.Redirect("/Account/Login");
            return;
        }
    }

    await next();
});



app.Run();
