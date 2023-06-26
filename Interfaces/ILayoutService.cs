using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.AccountViewModels;
using Wolmart.Ecommerce.ViewModels.CartViewModels;

namespace Wolmart.Ecommerce.Interfaces
{
    public interface ILayoutService
    {
        Task<List<CartVM>> GetCart();
        //Task<List<AppUser>> GetUser();
        Task<IDictionary<string, string>> GetSetting();
    }
}
