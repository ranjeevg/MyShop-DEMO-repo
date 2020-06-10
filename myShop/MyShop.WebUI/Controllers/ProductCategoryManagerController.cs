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
        ProductCategoryRepository context;
        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
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
        public ActionResult Edit(string ID)
        {
            ProductCategory productCategory = context.Find(ID);
            if (productCategory is null) return HttpNotFound();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string ID)
        {
            ProductCategory productCategoryToEdit = context.Find(ID);
            if (productCategoryToEdit is null) return HttpNotFound();
            if (!ModelState.IsValid) return View(productCategory);

            productCategoryToEdit.Category = productCategory.Category;

            context.Commit();
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete endpoints
        public ActionResult Delete(string ID)
        {
            ProductCategory productCategoryToDelete = context.Find(ID);
            if (productCategoryToDelete is null) return HttpNotFound();

            return View(productCategoryToDelete);
        }

        // remember that we renamed this one in the previous example
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string ID)
        {
            ProductCategory productCategoryToDelete = context.Find(ID);
            if (productCategoryToDelete is null) return HttpNotFound();

            context.Delete(ID);
            context.Commit(); // save these changes
            return RedirectToAction("Index");
        }
        #endregion
    }
}