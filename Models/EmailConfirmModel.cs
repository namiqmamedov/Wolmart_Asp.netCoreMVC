using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Wolmart.Ecommerce.Models
{
    public class EmailConfirmModel
    {
        [Required, Display(Name = "Registered email address")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
        public bool EmailVerified { get; set; }
    }
}
