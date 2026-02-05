using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Security ayarları appsettings.json içinde bulunamadı!");;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var tokenOptions = builder.Configuration.GetSection("Security").Get<TokenOptions>() 
                   ?? throw new InvalidOperationException("Security ayarları appsettings.json içinde bulunamadı!");

// builder.Services.AddAuthentication(options => 
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = tokenOptions.Issuer,
//         ValidAudience = tokenOptions.Audience,
//         IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecretKey),
//         ClockSkew = TimeSpan.Zero // Süre aşımını tam vaktinde kontrol et
//     };

//     options.Events = new JwtBearerEvents
//     {
//         OnMessageReceived = context =>
// {
//     var cookieToken = context.Request.Cookies["jwtToken"];
//     if (!string.IsNullOrEmpty(cookieToken))
//     {
//         // 1. URL Decode işlemi (Kritik: %3D gibi karakterleri temizler)
//         string decodedToken = System.Net.WebUtility.UrlDecode(cookieToken);

//         // 2. Tırnak işaretlerini temizle (Bazı tarayıcılar cookie'yi "token" şeklinde saklar)
//         decodedToken = decodedToken.Replace("\"", "");

//         // 3. Varsa Bearer kısmını temizle ve Trim yap
//         if (decodedToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//         {
//             decodedToken = decodedToken.Substring(7);
//         }

//         context.Token = decodedToken.Trim();
//     }
//     return Task.CompletedTask;
// },
//         OnChallenge = context =>
//         {
//             // Eğer 401 alırsa tarayıcıyı Login sayfasına yönlendir
//             context.HandleResponse();
//             context.Response.Redirect("/Authentication/Login");
//             return Task.CompletedTask;
//         },
        
//      OnAuthenticationFailed = context =>
//     {
//         Console.WriteLine("Doğrulama hatası: " + context.Exception.Message);
//         return Task.CompletedTask;
//     },
//     };
// });


builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecretKey),
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
    };

    options.Events = new JwtBearerEvents
    {
       OnMessageReceived = context =>
{
        var token = context.Request.Cookies["jwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            string cleanToken = System.Net.WebUtility.UrlDecode(token);

            cleanToken = cleanToken.Trim('"').Trim();

            if (cleanToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                cleanToken = cleanToken.Substring(7).Trim();
            }

            context.Token = cleanToken;
            
            var parts = cleanToken.Split('.');
            Console.WriteLine($"[JWT DEBUG] Parça Sayısı: {parts.Length} | Temizlenmiş Uzunluk: {cleanToken.Length}");
        }
        return Task.CompletedTask;
    },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.Redirect("/Authentication/Login");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserDal, UserDal>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<ISliderDal, SliderDal>();

builder.Services.AddScoped<ISliderService, SliderService>();

builder.Services.AddScoped<IProductDal, ProductDal>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IProductCategoryDal, ProductCategoryModelDal>();

builder.Services.AddScoped<IProductCategoryModelService, ProductCategoryService>();

builder.Services.AddScoped<IImageDal, ImageDal>();

builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IFormDal, FormDal>();

builder.Services.AddScoped<IFormService, FormService>();

builder.Services.AddScoped<IJwtProvider, JwtHelper>();

builder.Services.AddScoped<IAuthService, AuthenticationService>();

// builder.Services.ConfigureApplicationCookie( options =>
// {
//     options.LoginPath = "/Authentication/Login";
//     options.AccessDeniedPath = "/Home/";
// });


var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name:"default", pattern:"{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
