using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Areas.admin.ViewModels.AccountViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "The email field is required.")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        public bool RemindMe { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }
    }
}
