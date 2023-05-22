﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.Ecommerce.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255  )] 
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public double Price  { get; set; }
        [Column(TypeName = "money")]
        public double DiscountedPrice { get; set; }
        [StringLength(8)]
        [Required]
        public string Code { get; set; }
        public int Count { get; set; }

        public int BrandID { get; set; }
        public Brand Brand { get; set; }
        public string MainImage { get; set; }
        public string HoverImage { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }
    }
}
