using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PraktikaSon.Models;

namespace PraktikaSon.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<Team> Teams { get; set; }
		public DbSet<Setting> Settings { get; set; }
	}
}
