using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            IDictionary<string,string> settings = await _context.Settings.ToDictionaryAsync(x=>x.Key,x=>x.Value);

            List<CartVM> cartVMs = null;

            string cart = HttpContext.Request.Cookies["cart"];

            if (!string.IsNullOrWhiteSpace(cart)) 
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();

                foreach (CartVM cartVM in cartVMs)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == cartVM.ProductID);

                    cartVM.Image = product.MainImage;
                    cartVM.Name = product.Name;
                    cartVM.Price = product.DiscountedPrice > 0 ? product.DiscountedPrice : product.Price;
                }
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
