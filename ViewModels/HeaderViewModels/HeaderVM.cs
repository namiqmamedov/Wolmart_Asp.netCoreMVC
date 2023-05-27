using System.Collections;
using System.Collections.Generic;
using Wolmart.Ecommerce.ViewModels.CartViewModels;

namespace Wolmart.Ecommerce.ViewModels.HeaderVM
{
    public class HeaderVM
    {
        public IDictionary<string,string> Settings { get; set; }
        public List<CartVM> CartVMs  { get; set; }
    }
}
