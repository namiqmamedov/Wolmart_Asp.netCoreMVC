using Microsoft.AspNetCore.Mvc;

namespace Wolmart.Ecommerce.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
