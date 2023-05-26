using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Interfaces;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;

namespace Wolmart.Ecommerce.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public LayoutService(IHttpContextAccessor httpContextAccessor,AppDbContext context)
        {
             _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<List<CartVM>> GetCart()
        {
            string cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart)) { 
                
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>(); // eger hecne yoxdursa bos bir obyekt gonderilsin, cunki null olduqda error verir
            }

            foreach (CartVM item in cartVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == item.ProductID);

                item.Name = product.Name;
                item.Price = product.DiscountedPrice > 0 ? product.DiscountedPrice : product.Price;
                item.Image = product.MainImage;
            }

            return cartVMs;
        }

        public Task<List<CartVM>> GetCarts()
        {
            throw new System.NotImplementedException();
        }
    }
}
