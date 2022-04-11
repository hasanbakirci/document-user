using core.Middleware;
using document_service.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddClients();
builder.Services.AddUtilities();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Override("Microsoft",LogEventLevel.Error)
        .WriteTo.Console()
        .WriteTo.File("log.txt")
        .Enrich.WithProperty("ApplicationName","document-service")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.Run();