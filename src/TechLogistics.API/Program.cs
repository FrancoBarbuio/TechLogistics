using Microsoft.EntityFrameworkCore;
using TechLogistics.Domain.Interfaces;
using TechLogistics.Infrastructure.Persistence;
using TechLogistics.Application.Interfaces;
using TechLogistics.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Aquí agregamos Swagger (La documentación visual)
builder.Services.AddSwaggerGen();

// --- CONFIGURACIÓN SQL SERVER ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- INYECCIÓN DE REPOSITORIOS (DI) ---
// Registramos el repositorio para que la API pueda usarlo
builder.Services.AddScoped<IShippingRepository, ShippingRepository>();

// INYECCIÓN DE RABBITMQ
builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();

// 4. CONFIGURACIÓN CORS (¡ESTO ES LO QUE FALTA!)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Permitir solo a Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- CONFIGURACIÓN DEL PIPELINE HTTP ---

if (app.Environment.IsDevelopment())
{
    // Activamos la interfaz visual de Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();