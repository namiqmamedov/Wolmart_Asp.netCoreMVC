using Microsoft.AspNetCore.Mvc;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
