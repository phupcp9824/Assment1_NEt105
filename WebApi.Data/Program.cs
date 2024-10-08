using Data.Repository.IRepository;
using Data.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OrderDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IRepUser, RepUser>();
builder.Services.AddScoped<IRepCombo, RepCombo>();
builder.Services.AddScoped<IRepFood, RepFood>();


builder.Logging.AddConsole(); // Add logging


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
