using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        #region constructor
        IRepository<ProductCategory> context;
        // by dealing with interfaces, we can swap stuff out with ease. We pass interfaces 
        // to the constructor to make the compiler do the hard work of finding an appropriate class 
        // to load from
        public ProductCategoryManagerController(IRepository<ProductCategory> ProductCategoryContext)
        {
            this.context = ProductCategoryContext;
        }
        #endregion
        #region index
        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = context.Collection().ToList();
            return View(productCategories);
        }
        #endregion
        #region Create endpoints
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        // because we're posting data
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            // checking if requirements for model data validation are checked
            if (!ModelState.IsValid) return View(productCategory);

            context.Insert(productCategory);
            context.Commit();
            return RedirectToAction("Index");
        }
        #endregion
        #region Edit endpoints
        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = context.Find(Id);
            if (productCategory is null) return HttpNotFound();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            ProductCategory productCategoryToEdit = context.Find(Id);
            if (productCategoryToEdit is null) return HttpNotFound();
            if (!ModelState.IsValid) return View(productCategory);

            productCategoryToEdit.Category = productCategory.Category;

            context.Commit();
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete endpoints
        public ActionResult Delete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete is null) return HttpNotFound();

            return View(productCategoryToDelete);
        }

        // remember that we renamed this one in the previous example
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete is null) return HttpNotFound();

            context.Delete(Id);
            context.Commit(); // save these changes
            return RedirectToAction("Index");
        }
        #endregion
    }
}