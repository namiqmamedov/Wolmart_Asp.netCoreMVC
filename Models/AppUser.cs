using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.Ecommerce.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        //[StringLength(255)]
        //[Phone]
        //[Required]
        //public string AddrPhone { get; set; }
        //[StringLength(255)]
        //[Required]
        //public string PostCode { get; set; }
        //[StringLength(255)]
        //[Required]
        //public string City { get; set; }
        //[StringLength(255)]
        //[Required]
        //public string StreetAddrFirst { get; set; }
        //[StringLength(255)]
        //public string StreetAddrSecond { get; set; }
        //[StringLength(1000)]
        public string ProfilePicture { get; set; }
        [NotMapped]
        public IFormFile Picture { get; set; }

        public bool isAdmin { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public List<Cart> Carts { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }
}
