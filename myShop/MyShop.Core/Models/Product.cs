using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Product
    {
        public Product()
        {
            this.ID = Guid.NewGuid().ToString();
        }
        public string ID { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        // this will contain a URL to the image
        public string Image { get; set; }
    }
}
