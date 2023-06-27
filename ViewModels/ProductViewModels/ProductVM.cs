using System.Collections.Generic;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.ViewModels.ProductViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public Feedback Feedback { get; set; }
        //public List<Feedback> Feedbacks { get; set; }
    }
}
