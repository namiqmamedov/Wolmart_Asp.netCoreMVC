using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.HomeViewModels;

namespace Wolmart.Ecommerce.Controllers
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
            List<Product> products = await _context.Products.ToListAsync();

            HomeVM homeVM = new HomeVM
            {
                Products = await _context.Products.ToListAsync(),
                Sliders = await _context.Sliders.ToListAsync(),
                BestSeller = products.Where(p=>p.IsBestSeller).ToList(),
                Featured = products.Where(p=>p.IsFeatured).ToList(),
                MostPopular = products.Where(p=>p.IsMostPopular).ToList(),
                NewArrivals = products.Where(p=>p.IsNewArrival).ToList(),
            };

            return View(homeVM);
        }
    }
}
