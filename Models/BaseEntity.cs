using System.ComponentModel.DataAnnotations;
using System;

namespace Wolmart.Ecommerce.Models
{
    public class BaseEntity
    {
        public bool IsDeleted { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public Nullable<DateTime> CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public string DeletedBy { get; set; }
    }
}
