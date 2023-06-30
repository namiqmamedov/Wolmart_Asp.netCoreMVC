using Microsoft.AspNetCore.Mvc;

namespace Wolmart.Ecommerce.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
