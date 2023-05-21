using Microsoft.AspNetCore.Mvc;

namespace Wolmart.Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int? id)
        {
            return View();  
        }
    }
}
