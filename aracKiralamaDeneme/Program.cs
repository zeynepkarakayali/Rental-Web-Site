using aracKiralamaDeneme.Areas.IdentityStores;
using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Repositories;
using aracKiralamaDeneme.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<CarRentalContext>(options =>
//    options.UseSqlServer(connectionString)
//);
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionString));


// Identity ayarları
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, DapperUserStore>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, DapperRoleStore>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddDefaultTokenProviders();


builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<CurrencyService>();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<RentalRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//    DbInitializer.InitializeAsync(scope.ServiceProvider).GetAwaiter().GetResult();
//}

app.Run();
