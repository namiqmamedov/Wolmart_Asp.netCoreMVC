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
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> Index()
        {
            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            return View(await _getCartItemAsync(cartVMs));
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

            if (cart != null)
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            if (cartVMs.Exists(p => p.ProductID == id))
            {
                cartVMs.Find(p => p.ProductID == id).Count++; // the same product just increment  not the create duplicate
            }
            else
            {
                CartVM cartVM = new CartVM
                {
                    ProductID = product.ID,
                    Count = 1,
                };
                cartVMs.Add(cartVM);
            }


            cart = JsonConvert.SerializeObject(cartVMs);  // yeniden stringe cevirmek

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));

        }
        
        public async Task<IActionResult> DeleteCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if(!await _context.Products.AnyAsync(p=>p.ID == id))
            {
                return NotFound();    
            }

            string cart = HttpContext.Request.Cookies["cart"];

            if (string.IsNullOrWhiteSpace(cart)) { return BadRequest(); }

            List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

            CartVM cartVM = cartVMs.Find(p => p.ProductID == id);

            if(cartVM == null) { return NotFound(); }

            cartVMs.Remove(cartVM);

            cart = JsonConvert.SerializeObject(cartVMs);
            HttpContext.Response.Cookies.Append("cart",cart);

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));
        }

        public async Task<IActionResult> UpdateCount(int? id,int count)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p=>p.ID == id))
            {
                return NotFound();
            }

            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            
                CartVM cartVM = cartVMs.FirstOrDefault(p=>p.ProductID == id);

                if (cartVM == null) return BadRequest();
                 
                cartVM.Count = count <= 0 ? 1 : count;

                cart = JsonConvert.SerializeObject(cartVMs);

                HttpContext.Response.Cookies.Append("cart", cart);
            }
            else
            {
                return BadRequest(); 
            }

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }

        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (!await _context.Products.AnyAsync(p => p.ID == id))
            {
                return NotFound();
            }

            string cart = HttpContext.Request.Cookies["cart"];

            if (string.IsNullOrWhiteSpace(cart)) { return BadRequest(); }

            List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

            CartVM cartVM = cartVMs.Find(p => p.ProductID == id);

            if (cartVM == null) { return NotFound(); }

            cartVMs.Remove(cartVM);

            cart = JsonConvert.SerializeObject(cartVMs);
            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }
        private async  Task<List<CartVM>> _getCartItemAsync(List<CartVM> cartVMs)
        {
            foreach (CartVM item in cartVMs) // mes: sekil falan dbda deyisilende saytda reski gorsenmesi ucun
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.ID == item.ProductID);

                item.Name = dbProduct.Name;
                item.Price = dbProduct.DiscountedPrice > 0 ? dbProduct.DiscountedPrice : dbProduct.Price;
                item.Image = dbProduct.MainImage;
            }

            return cartVMs;

        }
    }
}
