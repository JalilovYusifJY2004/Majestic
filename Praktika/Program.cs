using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Praktika.DAL;
using Praktika.Models;
using Praktika.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<LayoutService>();
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute(
    "Default",
    "{area:exists}/{controller=home}/{action=index}/{id?}");
app.MapControllerRoute(
    "Default",
    "{controller=home}/{action=index}/{id?}");

app.Run();
