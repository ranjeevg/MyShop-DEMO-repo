using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory : BaseEntity
    {
        public string Category { get; set; }

        // again, forgot the constructor
        public ProductCategory() { this.Id = Guid.NewGuid().ToString(); }
    }
}
