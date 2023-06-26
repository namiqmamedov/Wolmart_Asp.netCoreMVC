using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Wolmart.Ecommerce.Enums;

namespace Wolmart.Ecommerce.Models
{
    public class Order
    {
        public int ID { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        [Column(TypeName = "money")]
        public double TotalPrice { get; set; }
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
        [Phone(ErrorMessage = "Please enter a valid email address.")]
        public string Phone { get; set; }
        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(255)]
        public string AddressFirst { get; set; }
        [StringLength(255)]
        public string AddressSecond { get; set; }
        public string Country { get; set; }
        [Required]
        [StringLength(255)]
        public string TownCity { get; set; }
        [Required]
        [StringLength(255)]
        public string State { get; set; }
        [Required]
        [StringLength(255)]
        public string ZipCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Comment  { get; set; }

        public int CountryID { get; set; }
        public Countries Countries { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
       
        public IEnumerable<OrderItem> OrderItems { get; set; }

    }
}
