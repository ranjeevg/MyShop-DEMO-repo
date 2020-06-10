﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory
    {
        public string ID { get; set; }
        public string Category { get; set; }

        // the constructor to set a GUID when created
        public ProductCategory() { this.ID = Guid.NewGuid().ToString(); }
    }
}
