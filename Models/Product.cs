using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.Ecommerce.Models
{
    public class Product : BaseEntity
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255)] 
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public double Price  { get; set; }
        [Column(TypeName = "money")]
        public double DiscountedPrice { get; set; }
        [StringLength(8)]
        [Required]
        public string Code { get; set; }
        public int Count { get; set; }

        public int BrandID { get; set; }

        public int? CategoryID { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public string MainImage { get; set; }
        public string HoverImage { get; set; }

        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsMostPopular { get; set; }
        public bool IsFeatured { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<FeedbackImage> FeedbackImages { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<ProductColorSize> ProductColorSizes { get; set; }

        [NotMapped]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        public IFormFile HoverFile { get; set; }
        [NotMapped]
        [MaxLength(5)]
        public IEnumerable<IFormFile> Files { get; set; }
        [NotMapped]
        public List<int> ColorIDs { get; set; } 
        [NotMapped]
        public List<int> SizeIDs { get; set; } 
        [NotMapped]
        public List<int> Counts { get; set; } 


    }
}
