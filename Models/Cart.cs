namespace Wolmart.Ecommerce.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public int ColorID { get; set; }
        public string Color { get; set; }
    }
}
