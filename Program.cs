using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(name:"default", pattern:"{controller=Home}/{action=Index}");

app.Run();