using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;

namespace Wolmart.Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
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

            Product product = await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return BadRequest();
            }

            return View(product);
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

        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(cart != null)
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            if (cartVMs.Exists(p=>p.ProductID == id)) 
            {
                cartVMs.Find(p => p.ProductID == id).Count++; // the same product just increment  not the duplicate
            }
            else
            {
                CartVM cartVM = new CartVM
                {
                    ProductID = product.ID,
                    Count = 1,
                    Image = product.MainImage,
                    Name = product.Name,
                    Price = product.DiscountedPrice > 0 ? product.DiscountedPrice : product.Price,
                };
                cartVMs.Add(cartVM);
            }


            cart = JsonConvert.SerializeObject(cartVMs);  // yeniden stringe cevirmek

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartPartial", cartVMs);

        }
    }
}
