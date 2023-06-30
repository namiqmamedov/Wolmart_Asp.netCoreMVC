using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Wolmart.Ecommerce.Models
{
    public class Feedback : BaseEntity
    {
        public int ID { get; set; }
        [StringLength(1000,ErrorMessage = "Sorry,you have exceeded the word count,please use fewer words.")]
        [Required(ErrorMessage = "Please do not leave this blank.")]
        public string Text { get; set; }

        public int Rating { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(255)]
        public string Lastname { get; set; }
        [Required]
        [StringLength(1000)]
        public string Image { get; set; }
        [AllowNull]
        public IEnumerable<FeedbackImage> FeedbackImages { get; set; }

        [NotMapped]
        [AllowNull]

        public IEnumerable<IFormFile> Files { get; set; }
    }
}
