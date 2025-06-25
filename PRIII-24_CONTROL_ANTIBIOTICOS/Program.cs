
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


builder.Services.AddDbContext<BdProa1Context>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

//var jwtConfig = builder.Configuration.GetSection("Jwt:Key");

//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(80);
        options.AccessDeniedPath = "/Login/AccessDenied";

    }
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
    builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();
app.UseCors("AllowAllOrigins");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//Este no muestra con login
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//Este primero muestra el login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
