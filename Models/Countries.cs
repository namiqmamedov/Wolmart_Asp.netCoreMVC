using System.Collections.Generic;

namespace Wolmart.Ecommerce.Models
{
    public class Countries
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
