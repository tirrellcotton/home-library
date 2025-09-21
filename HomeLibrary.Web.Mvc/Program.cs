using HomeLibrary.BusinessLogic.Managers;
using HomeLibrary.Core.Interfaces;
using HomeLibrary.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, true)
    .AddJsonFile($"appsettings.{env}.json", true, true)
    .AddEnvironmentVariables()
    .Build();

builder.Configuration.AddConfiguration(configuration);

// Add DB Contexts
builder.Services.AddDbContext<HomeLibrarySqlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HomeLibrarySql")));

// Add Transient Services
builder.Services.AddTransient<IGenreManager,GenreManager>();
builder.Services.AddTransient<IAuthorManager,AuthorManager>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();