using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        #region constructor
        ProductRepository context;
        public ProductManagerController()
        {
            context = new ProductRepository();
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
            Product product = new Product();
            return View(product);
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
            return View(product);
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