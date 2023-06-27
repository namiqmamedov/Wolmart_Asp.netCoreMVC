using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class Feedback : BaseEntity
    {
        public int ID { get; set; }
        [StringLength(1000,ErrorMessage = "Sorry,you have exceeded the word count,please use fewer words.")]
        [Required(ErrorMessage = "Please do not leave this blank.")]
        public string Text { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
