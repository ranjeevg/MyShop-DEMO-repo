using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {

        #region constructor
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        // adding these interfaces in the controller 'hooks them up' to our properties,
        // and the dependency injection containter (AKA DI container) does the hard work 
        // of connecting them to a class that implements said interface
        public HomeController(IRepository<Product> ProductContext,
            IRepository<ProductCategory> ProductCategoryContext)
        {
            this.context = ProductContext;
            this.productCategories = ProductCategoryContext;
        }
        #endregion
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product is null) return HttpNotFound();

            return View(product);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}