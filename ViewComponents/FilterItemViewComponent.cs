using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.ViewComponents
{
    public class FilterItemViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FilterItemViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Product> products)
        {
            return View(await Task.FromResult(products));
        }
    }
}
