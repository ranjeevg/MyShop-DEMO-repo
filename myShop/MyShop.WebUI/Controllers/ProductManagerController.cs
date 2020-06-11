using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using Unity.Injection;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        #region constructor
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        // adding these interfaces in the controller 'hooks them up' to our properties,
        // and the dependency injection containter (AKA DI container) does the hard work 
        // of connecting them to a class that implements said interface
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
        public ActionResult Create (Product product, HttpPostedFileBase file)
        {
            // checking if requirements for model data validation are checked
            if (!ModelState.IsValid) return View(product);

            if (!(file is null))
            {
                product.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
            }

            context.Insert(product);
            context.Commit();
            return RedirectToAction("Index");
        }
        #endregion
        #region Edit endpoints
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product is null) return HttpNotFound();

            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = product;
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit is null) return HttpNotFound();
            else
            {
                if (!ModelState.IsValid) return View(product);

                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                productToEdit.Name = product.Name;
                productToEdit.Description = product.Description;
                productToEdit.Category = product.Category;
                productToEdit.Price = product.Price;


                context.Commit();
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Delete endpoints
        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete is null) return HttpNotFound();

            return View(productToDelete);
        }

        // remember that we renamed this one in the previous example
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete is null) return HttpNotFound();

            context.Delete(Id);
            context.Commit(); // save these changes
            return RedirectToAction("Index");
        }
        #endregion
    }
}