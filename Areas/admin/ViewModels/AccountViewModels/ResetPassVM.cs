using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Areas.admin.ViewModels.AccountViewModels
{
    public class ResetPassVM
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must be the same!")]
        public string ConfirmPassword { get; set; }
    }
}
