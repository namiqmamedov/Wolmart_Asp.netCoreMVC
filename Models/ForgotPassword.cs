using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Wolmart.Ecommerce.Models
{
    public class ForgotPassword
    {
        [Required, EmailAddress,StringLength(255), Display(Name = "Registered email address")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
