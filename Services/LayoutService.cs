using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IHttpContextAccessor httpContextAccessor,AppDbContext context, UserManager<AppUser> userManager)
        {
             _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
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

            if  (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Carts).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                if (appUser.Carts != null && appUser.Carts.Count() > 0)
                {
                    foreach (var item in appUser.Carts)
                    {
                        if (!cartVMs.Any(c=>c.ProductID == item.ProductID))
                        {
                            CartVM cartVM = new CartVM
                            {
                                ProductID = item.ProductID,
                                Count = item.Count

                            };

                            cartVMs.Add(cartVM);
                        }
                    }

                    cart = JsonConvert.SerializeObject(cartVMs);

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("cart", cart);
                }
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

        public async Task<IDictionary<string, string>> GetSetting()
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(x=>x.Key,x=>x.Value);

            return settings;
        }
    }
}
