using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Application.Interfaces.RepositoryInterfaces;
using Infrastructure.Database.Repositories;
using Infrastructure.PasswordEncrypter;
using Application.Interfaces.ServiceInterfaces;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Token.JWT;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container
builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EFCDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordEncrypterRepository, BCryptRepository>();
builder.Services.AddScoped<ITokenRepository, JWTRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserControlService, UserControlService>();
builder.Services.AddScoped<IGoalControlService, GoalControlService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JWTOptions.Configure);
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