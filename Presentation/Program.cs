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

builder.Services.AddScoped<IAuthService,AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
