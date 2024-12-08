using KhoaLuan_QLNhaTro.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON options
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;  // Preserve original casing
    });

// Configure DbContext with SQL Server
builder.Services.AddDbContext<NhaTroDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLNTconnection"))
);

// Register VnPayService


// Configure session storage
builder.Services.AddDistributedMemoryCache();  // In-memory cache for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;  // Make session cookie HTTP-only
    options.Cookie.IsEssential = true;  // Essential for the app
});
builder.Services.AddScoped<IVnPayService, VnPayService>();
var app = builder.Build();

// Middleware pipeline configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();  // Enable session middleware

// Define default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
