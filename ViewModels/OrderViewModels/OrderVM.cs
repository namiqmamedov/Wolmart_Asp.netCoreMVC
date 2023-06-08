using System.Collections.Generic;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
