using System.Collections.Generic;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;

namespace Wolmart.Ecommerce.ViewModels.HomeViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<CartVM> CartVMs { get; set; }

    }
}
