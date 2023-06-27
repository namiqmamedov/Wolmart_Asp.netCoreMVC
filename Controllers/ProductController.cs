using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;
using Wolmart.Ecommerce.ViewModels.ProductViewModels;

namespace Wolmart.Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ProductVM productVM = new ProductVM
            {
                Product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.ID == id)
            };

            if (productVM == null)
            {
                return BadRequest();
            }

            return View(productVM);

        }


        public async Task<IActionResult> DetailForModal(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return BadRequest();
            }
            return PartialView("_ProductModalPartial", product);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products
           .Where(p => p.Name.ToLower().Contains(search.Trim().ToLower())).ToListAsync();

            return PartialView("_SearchPartial", products);
        }

        [HttpPost]
        public async Task<IActionResult> Feedback(int? id,Feedback feedback)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (appUser == null) { return BadRequest(); }

            //Product dbProduct = await _context.Products.Include(p=>p.Feedbacks).FirstOrDefaultAsync(p => p.ID == id);

            //if (dbProduct == null) { return BadRequest(); }

            feedback.IsDeleted = true;
            feedback.CreatedAt = DateTime.Now;
            feedback.AppUserId = appUser.Id;

            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction("index","product");
        }
    }
}
