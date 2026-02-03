using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DotNetEnv;
using Application.Interfaces.RepositoryInterfaces;
using Infrastructure.Database.Repositories;
using Infrastructure.PasswordEncrypter;
using Infrastructure.Token;
using Application.Interfaces.ServiceInterfaces;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container
builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Env.Load();
var connectionString =
    $"Host={Environment.GetEnvironmentVariable("POSTGRESQL_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("POSTGRESQL_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE")};" +
    $"Username={Environment.GetEnvironmentVariable("POSTGRESQL_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")}";
builder.Services.AddDbContext<PostgreSQLDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordEncrypterRepository, BCryptRepository>();
builder.Services.AddScoped<ITokenRepository, JWTRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserControlService, UserControlService>();
builder.Services.AddScoped<IGoalControlService, GoalControlService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("testkeytestkeytestkeytestkeytestkey"))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("jwt", out var token)) context.Token = token;
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
