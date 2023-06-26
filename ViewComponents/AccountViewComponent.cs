using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.ViewModels.AccountViewModels;

namespace Wolmart.Ecommerce.ViewComponents
{
    public class AccountViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public AccountViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<ProfileVM> profileVMs)
        {
            return View(await Task.FromResult(profileVMs));
        }
    }
}
