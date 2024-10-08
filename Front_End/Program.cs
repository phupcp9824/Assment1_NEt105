using Data.Repository.IRepository;
using Data.Repository.Repository;
using Front_End.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<UserController>();

//builder.Services.AddDbContext<OrderDbContext>(option =>
//{
//    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

//builder.Services.AddScoped<IRepUser, RepUser>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=LoginFirm}/{id?}");

app.Run();
