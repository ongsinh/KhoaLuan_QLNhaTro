using KhoaLuan_QLNhaTro.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NhaTroDbContext>( option => option.UseSqlServer(builder.Configuration.GetConnectionString("QLNTconnection")));
// Add services to the container.
builder.Services.AddControllersWithViews();

//Cấu jinhf sd Session
builder.Services.AddDistributedMemoryCache(); // Cấu hình bộ nhớ đệm cho session

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session sống
    options.Cookie.HttpOnly = true; // Đảm bảo bảo mật cho cookie
    options.Cookie.IsEssential = true; // Đảm bảo cookie cần thiết
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
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
