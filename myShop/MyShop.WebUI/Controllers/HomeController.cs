using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
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
        // the purpose of 'string s = null' is to declare the string input as optional.
        // if it's present, then great; if it isn't, then that's fine too.
        public ActionResult Index(string Category = null)
        {
            List<Product> products = context.Collection().ToList();
            List<ProductCategory> categories = productCategories.Collection().ToList();

            if (Category is null)
                context.Collection().ToList();
            else
            {
                // the reason we defined the product list as an IQueryable is so 
                // that we can sort its values using the Where() method and lambda syntax.
                products = context.Collection().Where
                    (p => p.Category == Category).ToList(); ;
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;
            return View(model);
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