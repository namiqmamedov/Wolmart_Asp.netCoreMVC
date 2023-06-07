using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.ViewModels.AccountViewModels
{
    public class ProfileVM
    {
        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string Surname { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
    }
}
