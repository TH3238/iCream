using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ICream.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using ICream.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ICreamContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ICreamContext") ?? throw new InvalidOperationException("Connection string 'ICreamContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient to the services container
builder.Services.AddHttpClient();


// Register ImaggaService and configure it to use HttpClient
builder.Services.AddScoped<ImaggaService>();


// Add authorization services
builder.Services.AddAuthorization(options =>
{
    // Configure authorization policies here if needed
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=index}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
