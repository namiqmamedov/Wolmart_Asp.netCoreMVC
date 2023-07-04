using System.Collections.Generic;
using Wolmart.Ecommerce.ViewModels.AccountViewModels;

namespace Wolmart.Ecommerce.ViewModels.CartViewModels
{
    public class CartVM
    {
        public int ProductID { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int ColorID { get; set; }
        public string Color { get; set; }

    }
}
