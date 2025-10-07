using ApiContaCorrente.Interfaces;
using ApiContaCorrente.Repository;
using ApiContaCorrente.Services;
using System.Data;
using System.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDbConnection>(serviceProvider =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SQLiteConnection(connectionString);
});
builder.Services.AddTransient<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddTransient<IContaCorrenteService, ContaCorrenteService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
