using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService BasketService;
        IOrderService OrderService;
        IRepository<Customer> Customers;

        public BasketController(IBasketService basketService, IOrderService orderService,
                                IRepository<Customer> customers) 
        { 
            this.BasketService = basketService;
            this.OrderService = orderService;
            this.Customers = customers;
        }

        // GET: Basket
        public ActionResult Index()
        {
            var model = BasketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            BasketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            BasketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = BasketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = Customers.Collection().FirstOrDefault
                                    (c => c.Email == User.Identity.Name);

            if (!(customer is null))
            {
                Order order = new Order()
                {
                    FirstName = customer.FirstName,
                    Surname = customer.LastName,
                    Email = customer.Email,
                    City = customer.City,
                    Street = customer.Street,
                    Province = customer.Province,
                    PostalCode = customer.PostalCode
                };
                return View(order);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout (Order order)
        {
            var basketItems = BasketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            // payment processing happens

            order.OrderStatus = "Processed Payment";
            OrderService.CreateOrder(order, basketItems);
            BasketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderID = order.Id });
        }

        public ActionResult ThankYou(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}