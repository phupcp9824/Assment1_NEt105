using Data.Repository.IRepository;
using Data.Repository.Repository;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Register the IRepUser with its implementation
builder.Services.AddScoped<IRepUser, RepUser>();
builder.Services.AddScoped<IRepCombo, RepCombo>();
builder.Services.AddScoped<IRepFood, RepFood>();

builder.Services.AddControllers();
// configuration xác thực Authen / JwtBearerDefaults.AuthenticationScheme sử dụng xác thực Jwt bearer (mã thông báo jwt)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // Đk config mã thông báo xác thực jwt
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Check nhà phát hành
            ValidateAudience = true, // check đối tượng (audience) mã thông báo có khớp vs ứng dụng ko
            ValidateLifetime = true, // check token expiretime
            ValidateIssuerSigningKey = true, // check tính hợp lệ key nhà phát hành(help token trành bị giả mạo)

            ValidIssuer = builder.Configuration["JwtIssuer"], //  mô tả token cung cấp bên nào
            ValidAudience = builder.Configuration["JwtAudience"], // ai là người dùng autience
            //JwtSecurityKey server giải mã và mã hóa token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]))
        };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //chỉ định rằng trình tuần tự hóa JSON phải bảo toàn/ enable giải tuần tự hóa trở lại cấu trúc đối tượng ban đầu.
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
//builder.Services.AddControllers(options =>
//{
//    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
//    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
//    {
//        ReferenceHandler = ReferenceHandler.Preserve,
//    }));
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
