using DevEncurtaUrlApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DevEncurtaUrlDbContext>(x =>
                                        x.UseInMemoryDatabase("DevEncurtaDb"));
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().
               AllowAnyHeader().
               AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevEncurtaUrl.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Jhow",
            Email = "jhonatasrcorrea@gmail.com",
            Url = new Uri("https://github.com/CorreaJhow")
        }
    });
});

builder.Host.ConfigureAppConfiguration((hosting, config) =>
{
    Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
   .WriteTo.Console().CreateLogger();
}).UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
