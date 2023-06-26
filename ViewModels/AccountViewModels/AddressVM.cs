using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.ViewModels.AccountViewModels
{
    public class AddressVM
    {
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(255)]
        [Phone]
        [Required]
        public string AddrPhone { get; set; }
        [StringLength(255)]
        [Required]
        public string PostCode { get; set; }
        [StringLength(255)]
        [Required]
        public string City { get; set; }
        [StringLength(255)]
        [Required]
        public string StreetAddrFirst { get; set; }
        [StringLength(255)]
        public string StreetAddrSecond { get; set; }

    }
}
