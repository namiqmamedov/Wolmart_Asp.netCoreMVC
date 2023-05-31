using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class Category : BaseEntity
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(1024)]
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int? ParentID { get; set; }
        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
