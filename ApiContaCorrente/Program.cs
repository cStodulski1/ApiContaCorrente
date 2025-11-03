using ApiContaCorrente.Authentication;
using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Database.Repositories;
using ApiContaCorrente.Extensions;
using ApiContaCorrente.IdempotenciaUtils;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Data;
using System.Data.SQLite;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDbConnection>(serviceProvider =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SQLiteConnection(connectionString);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = "host.docker.internal:6379";
    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddScoped<IDatabase>(sp =>
{
    var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
    return multiplexer.GetDatabase();
});

builder.Services.AddTransient<IContaCorrenteRepository, ContaCorrenteRepository>();

builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


builder.Services.AddScoped<TokenProvider>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                if(context.AuthenticateFailure != null)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"error\": \"Invalid or expired token - Forbidden\"}");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
