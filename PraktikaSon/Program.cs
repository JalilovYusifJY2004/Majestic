using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PraktikaSon.DAL;
using PraktikaSon.Models;
using PraktikaSon.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<LayoutService>();
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllerRoute(
    "default",
"{area:exists}/{controller=home}/{action=index}/{id?}"
);
app.MapControllerRoute(
    "default",
"{controller=home}/{action=index}/{id?}"
);


app.Run();
