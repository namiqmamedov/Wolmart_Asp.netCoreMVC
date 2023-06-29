namespace Wolmart.Ecommerce.Models
{
    public class FeedbackImage : BaseEntity
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public int FeedbackID { get; set; }
        public Feedback Feedback { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserID { get; set; }
    }
}
