using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container
builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

Env.Load();
var connectionString = 
    $"Host={Environment.GetEnvironmentVariable("POSTGRESQL_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("POSTGRESQL_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE")};" +
    $"Username={Environment.GetEnvironmentVariable("POSTGRESQL_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")}";
builder.Services.AddDbContext<PostgreSQLDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
