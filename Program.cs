using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserDal, UserDal>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<ISliderDal, SliderDal>();

builder.Services.AddScoped<ISliderService, SliderService>();

builder.Services.AddScoped<IProductDal, ProductDal>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IProductCategoryModelService, ProductCategoryService>();

builder.Services.AddScoped<IProductCategoryDal, ProductCategoryModelDal>();


var app = builder.Build();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(name:"default", pattern:"{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
