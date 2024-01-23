using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Praktikaimt.DAL;
using Praktikaimt.Models;

namespace Praktikaimt.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
           _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _context.Departments.ToListAsync();

            return View(departments);
        }
    }
}
