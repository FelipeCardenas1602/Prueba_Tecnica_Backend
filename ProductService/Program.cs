using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Servicios del contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexión a SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Swagger solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de autenticación con API Key (global)
app.UseMiddleware<ApiKeyMiddleware>();

// Middleware de autorización
app.UseAuthorization();

// Mapea controladores
app.MapControllers();

// Ejecuta la app
app.Run();

