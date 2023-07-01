using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
        {
            return View(await Task.FromResult(products));
        }
    }
}
