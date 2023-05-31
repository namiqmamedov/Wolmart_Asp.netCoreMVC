using System.Collections;
using System.Collections.Generic;

namespace Wolmart.Ecommerce.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string IsMain { get; set; }
        public int? ParentID { get; set; }
        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
    }
}
