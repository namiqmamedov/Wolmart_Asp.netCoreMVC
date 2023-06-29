using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;
        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Feedback> query = _context.Feedbacks;

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            //ViewBag.PageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount); 
            //ViewBag.Page = page;
            //List<Brand> brands = await query.Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
            //ViewBag.ItemCount = itemCount;
            return View(PagenationList<Feedback>.Create(query, page, itemCount));
        }
    }
}
