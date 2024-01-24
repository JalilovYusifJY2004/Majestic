using Microsoft.AspNetCore.Mvc;

namespace PraktikaSon2.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
