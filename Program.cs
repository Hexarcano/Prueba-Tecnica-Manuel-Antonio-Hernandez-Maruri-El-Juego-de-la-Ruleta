using Microsoft.EntityFrameworkCore;
using Ruleta.Application.Interfaces;
using Ruleta.Application.Services;
using Ruleta.Domain.Interfaces;
using Ruleta.Domain.Repository;
using Ruleta.Infraestructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with PostgreSQL
builder.Services.AddDbContext<RuletaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddScoped<IGamblerService, GamblerService>();
builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<IGamblerRepository, GamblerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();