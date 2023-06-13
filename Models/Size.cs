using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class Size
    {
        public int ID { get; set; }
        [StringLength(255), Required]
        public string Name { get; set; }
        public IEnumerable<ProductColorSize> ProductColorSizes { get; set; }

    }
}
