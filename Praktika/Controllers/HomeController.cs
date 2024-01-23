using Microsoft.AspNetCore.Mvc;

namespace Praktika.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
