using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Rotas.Repository.Interfaces;
using Rotas.Repository.Repositories;
using Rotas.Service.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Rotas de Viagem",
        Version = "v1"
    });
});

// Dependências
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRotaRepository, RotaRepository>();
builder.Services.AddScoped<RotaService>();

var app = builder.Build();

// Habilitar Swagger para qualquer ambiente (ou use if dev)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Rotas v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
