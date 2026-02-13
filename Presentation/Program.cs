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
using Presentation.Options;
using Infrastructure.Token;
using Domain.Constants;
using Domain.Entities;

Env.Load(); //загрузка секретов из .env файла

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<EFCDbContext>();
builder.Services.AddSingleton<IPasswordEncrypterRepository, BCryptRepository>();
builder.Services.AddSingleton<ITokenRepository, JWTRepository>();
builder.Services.AddSingleton<ICacheKeyService, CacheKeyService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserControlService, UserControlService>();
builder.Services.AddScoped<IGoalControlService, GoalControlService>();
builder.Services.AddScoped<IReportService, ReportService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JWTOptions.Configure);
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Policies.RequireLogin, policy => policy.RequireClaim(Claims.Login))
    .AddPolicy(Policies.RequireAdminAccess, policy => policy.RequireRole(RoleType.Admin.ToString()));

var app = builder.Build();

app.UseExceptionHandler("/exception");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

await InitAdminAsync(app.Services);

app.Run();

async Task InitAdminAsync(IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<EFCDbContext>();
    var encrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypterRepository>();

    if (!await db.Users.AnyAsync(u => u.Login == DefaultAdmin.Login))
    {
        var adminPassword = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_PASSWORD");
        if (string.IsNullOrWhiteSpace(adminPassword)) adminPassword = DefaultAdmin.Password;

        var user = new User(
            DefaultAdmin.Login,
            encrypter.Encrypt(adminPassword),
            (int)DefaultAdmin.Role);
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }
}