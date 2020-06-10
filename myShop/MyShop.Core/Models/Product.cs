﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Product
    {
        public string ID { get; set; }

        [Required]
        [DisplayName("Product Name")]
        [StringLength(20, ErrorMessage = "The product's name can be 20 " +
            "characters long, at most.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}