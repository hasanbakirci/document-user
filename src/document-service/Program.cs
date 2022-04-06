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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
           new OpenApiSecurityScheme
           {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                } 
           },
           new List<string>()
        }
    });
});

builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Override("Microsoft",LogEventLevel.Error)
        .WriteTo.Console()
        .WriteTo.File("log.txt")
        .Enrich.WithProperty("ApplicationName","document-service")
        //.CreateLogger()
    //.WriteTo.Seq("http://localhost:5341/") //nugettten seq kurulu olmalı 
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