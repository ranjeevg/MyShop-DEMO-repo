using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products is null) products = new List<Product>();
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update (Product p)
        {
            Product productToUpdate = products.Find(x => x.ID == p.ID);
            if (productToUpdate is null) throw new Exception("Product not found");

            productToUpdate = p;
        }

        public Product Find(string ID)
        {
            Product product = products.Find(x => x.ID == ID);
            if (product is null) throw new Exception("Product not found");

            return product;
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string ID)
        {
            Product productToDelete = products.Find(x => x.ID == ID);
            if (productToDelete is null) throw new Exception("Product not found");

            products.Remove(productToDelete);
        }
    }
}
