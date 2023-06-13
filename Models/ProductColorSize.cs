namespace Wolmart.Ecommerce.Models
{
    public class ProductColorSize
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int ColorID { get; set; }
        public Color Color { get; set; }
        public int SizeID { get; set; }
        public Size Size { get; set; }
        public int Count { get; set; }
    }
}
