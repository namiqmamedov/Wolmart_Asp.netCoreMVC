using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;
using Wolmart.Ecommerce.ViewModels.HeaderVM;

namespace Wolmart.Ecommerce.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> settings)
        {

            List<CartVM> cartVMs = null;

            string cart = HttpContext.Request.Cookies["cart"];

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

                if (User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager.Users.Include(u => u.Carts).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    if (appUser.Carts != null && appUser.Carts.Count() > 0)
                    {
                        foreach (var item in appUser.Carts)
                        {
                            if (!cartVMs.Any(c => c.ProductID == item.ProductID))
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

                        HttpContext.Response.Cookies.Append("cart", cart);
                    }

                    else if (appUser.Carts != null && appUser.Carts.Count() > 0)
                    {
                        foreach (CartVM cartVM in cartVMs)
                        {
                            if (cartVMs.Any(c => c.ProductID == cartVM.ProductID))
                            {
                                Cart existedCart = appUser.Carts.FirstOrDefault(c => c.ProductID == cartVM.ProductID);

                                appUser.Carts.Remove(existedCart);
                            }
                        }
                        await _context.SaveChangesAsync();

                        cart = JsonConvert.SerializeObject(cartVMs);

                        HttpContext.Response.Cookies.Append("cart", cart);
                    }
                }

                foreach (CartVM cartVM in cartVMs)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == cartVM.ProductID);

                    cartVM.Image = product.MainImage;
                    cartVM.Name = product.Name;
                    cartVM.Price = product.DiscountedPrice > 0 ? product.DiscountedPrice : product.Price;
                }
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            HeaderVM headerVM = new HeaderVM
            {
                Settings = settings,
                CartVMs = cartVMs
            };

            return View(await Task.FromResult(headerVM));
        }

    }
}
