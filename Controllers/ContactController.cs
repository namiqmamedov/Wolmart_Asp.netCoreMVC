using Microsoft.AspNetCore.Mvc;

namespace Wolmart.Ecommerce.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
