using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraktikaSon.DAL;
using PraktikaSon.Models;

namespace PraktikaSon.Controllers
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
			List<Team> teams = await _context.Teams.ToListAsync();
			return View(teams);
		}


	}
}
