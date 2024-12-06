﻿using KhoaLuan_QLNhaTro.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;  // Tùy chọn cho cách viết tên thuộc tính trong JSON
    });


builder.Services.AddDbContext<NhaTroDbContext>( option => option.UseSqlServer(builder.Configuration.GetConnectionString("QLNTconnection")));
// Add services to the container.
builder.Services.AddControllersWithViews();

//Cấu jinhf sd Session
builder.Services.AddDistributedMemoryCache(); // Use a memory cache to store session data
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the timeout period for the session
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

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

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Asset}/{action=AssetMain}/{id?}");

app.Run();

