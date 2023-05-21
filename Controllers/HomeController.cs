using Microsoft.AspNetCore.Mvc;

namespace Wolmart.Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
