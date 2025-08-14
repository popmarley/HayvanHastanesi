using System;
using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("HayvanHastanesi");

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// MVC, Session, Authentication servisleri
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Giris/Giris"; // Giriþ yapýlmadýðýnda yönlendirme yapýlacak path
        options.LogoutPath = "/Giris/Cikis";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Varsayýlan oturum süresi
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Middleware sýralamasý
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();   // CookiePolicy önce
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Anasayfa}/{action=Anasayfa}/{id?}");

app.Run();
