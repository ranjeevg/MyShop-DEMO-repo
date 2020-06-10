using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories is null) productCategories = new List<ProductCategory>();
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory p)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(x => x.ID == p.ID);
            if (productCategoryToUpdate is null) throw new Exception("Product not found");

            productCategoryToUpdate = p;
        }

        public ProductCategory Find(string ID)
        {
            ProductCategory productCategory = productCategories.FirstOrDefault(x => x.ID == ID);
            if (productCategory is null) throw new Exception("Product not found");

            return productCategory;
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string ID)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(x => x.ID == ID);
            if (productCategoryToDelete is null) throw new Exception("Product not found");

            productCategories.Remove(productCategoryToDelete);
        }
    }
}