using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SchoolLibrary.Application;
using SchoolLibrary.Application.Shared;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;
using SchoolLibrary.Infrastructure.Common;
using System.Text;

var reactAppOrigins = "ReactApp";
var builder = WebApplication.CreateBuilder(args);

// Выдача разрешений веб-сайту на выполнение запросов 
// Allow query execution to website with origins
#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(reactAppOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});
#endregion


builder.Services.AddControllers();
// Добавление сервиса пользователей / Add user identity
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Добавление сервиса логов / Add log service
builder.Services.AddLogging();

#region DependencyInjection
// Инверсия зависимостей слоев / Dependency injection 
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
#endregion

builder.Services.AddOpenApi();

#region AuthServices
builder.Services.AddAuthorization();
builder.Services
    .AddAuthentication(options =>
    {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]!)),
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies[CookieHeaderNames.CookieHeaderNameAccessToken];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });
    builder.Services.AddHttpContextAccessor();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.UseHttpsRedirection();
app.UseCors(reactAppOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Первичное добавление данных в базу данных / Initial data seeding
DataSeeding();
app.Run();

// Инициализия базы данных / Data base initialization
async void DataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.Initialize();
    }
}
