using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class SKU
    {
        public int ID { get; set; }
        [StringLength(8)]
        [Required]
        public string Code { get; set; }
    }
}
