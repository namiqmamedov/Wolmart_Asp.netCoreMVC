using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class Brand
    {
        public int ID { get; set; }
        [StringLength(1000)]
        public string Image { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
