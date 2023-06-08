using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.Ecommerce.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        [Column(TypeName = "money")]
        public double Price { get; set; }
        public int Count { get; set; }
        [Column(TypeName = "money")]
        public double TotalPrice { get; set; }
        public Order Order { get; set; }
        public Product Product  { get; set; }
    }
}
