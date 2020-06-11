using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        #region constructor
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        // adding these interfaces in the controller 'hooks them up' to our properties,
        // and the computer does the hard work of connecting them to a class 
        // that implements said interface
        public ProductManagerController(IRepository<Product> ProductContext, 
            IRepository<ProductCategory> ProductCategoryContext)
        {
            this.context = ProductContext;
            this.productCategories = ProductCategoryContext;
        }
        #endregion
        #region index
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        #endregion
        #region Create endpoints
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }

        // because we're posting data
        [HttpPost]
        public ActionResult Create (Product product)
        {
            // checking if requirements for model data validation are checked
            if (!ModelState.IsValid) return View(product);

            context.Insert(product);
            context.Commit();
            return RedirectToAction("Index");
        }
        #endregion
        #region Edit endpoints
        public ActionResult Edit(string ID)
        {
            Product product = context.Find(ID);
            if (product is null) return HttpNotFound();

            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = product;
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string ID)
        {
            Product productToEdit = context.Find(ID);
            if (productToEdit is null) return HttpNotFound();
            else
            {
                if (!ModelState.IsValid) return View(product);

                productToEdit.Name = product.Name;
                productToEdit.Description = product.Description;
                productToEdit.Category = product.Category;
                productToEdit.Price = product.Price;
                productToEdit.Image = product.Image;

                context.Commit();
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Delete endpoints
        public ActionResult Delete(string ID)
        {
            Product productToDelete = context.Find(ID);
            if (productToDelete is null) return HttpNotFound();

            return View(productToDelete);
        }

        // remember that we renamed this one in the previous example
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string ID)
        {
            Product productToDelete = context.Find(ID);
            if (productToDelete is null) return HttpNotFound();

            context.Delete(ID);
            context.Commit(); // save these changes
            return RedirectToAction("Index");
        }
        #endregion
    }
}