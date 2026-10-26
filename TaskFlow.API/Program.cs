using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.Middlewares;
using TaskFlow.API.Repositories;
using TaskFlow.API.Repositories.Interfaces;
using TaskFlow.API.Services;
using TaskFlow.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware global de erros (primeiro da pipeline)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
