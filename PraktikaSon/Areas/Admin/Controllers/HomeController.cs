using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraktikaSon.Areas.Admin.ViewModels.Team;
using PraktikaSon.DAL;
using PraktikaSon.Models;
using PraktikaSon.Utilities.Extension;

namespace PraktikaSon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
   

        public HomeController(AppDbContext context)
        {
            _context = context;
   
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams= await _context.Teams.ToListAsync();
            return View(teams);
        }


    }
}
