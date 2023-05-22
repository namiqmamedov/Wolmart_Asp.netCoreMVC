namespace Wolmart.Ecommerce.Models
{
    public class ProductImage
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
