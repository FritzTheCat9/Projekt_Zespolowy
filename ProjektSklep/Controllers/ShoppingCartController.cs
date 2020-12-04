using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using ProjektSklep.Data;
using ProjektSklep.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektSklep.Models.ViewModels;

namespace ProjektSklep.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ShoppingCart _shoppingCart;

        private readonly ShopContext _context;

        private readonly ILogger<HomeController> _logger;

        public ShoppingCartController(ILogger<HomeController> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            using (var context = new ShopContext())
            {
                var paymentMethods = context.PaymentMethods.ToList();
                var shippingMethods = context.ShippingMethods.ToList();

                ViewData["PaymentMethods"] = new SelectList(paymentMethods, "PaymentMethodID", "Name");
                ViewData["ShippingMethods"] = new SelectList(shippingMethods, "ShippingMethodID", "Name");
            }

            _shoppingCart = CreateCart();
            return View(_shoppingCart);
        }

        private ShoppingCart CreateCart()
        {
            _shoppingCart = new ShoppingCart();
            _shoppingCart.ProductList = new List<ShoppingCartElement>();
            List<Product> products = _context.Products.Include(p => p.Category).Include(p => p.Expert).ToList();

            var cookie = Request.Cookies["ShoppingCart"];
            if (cookie != null)
            {
                string[] elements = cookie.Split('-');
                if (elements.Count() != 0)
                {
                    List<int> ids = new List<int>();
                    foreach (var item in elements)
                    {
                        if (item != "")
                            ids.Add(int.Parse(item));
                    }

                    foreach (int id in ids)
                    {
                        var elem = products.Find(x => x.ProductID == id);

                        if (elem != null)
                        {
                            var product = _shoppingCart.ProductList.Find(x => x.Product.ProductID == id);
                            if (product != null)
                            {
                                product.Count++;
                                product.Sum += elem.Price;
                            }
                            else
                            {
                                _shoppingCart.ProductList.Add(new ShoppingCartElement { Product = elem, Count = 1, Sum = elem.Price });
                            }
                        }
                    }
                }
            }
            return _shoppingCart;
        }

        [HttpGet("ShoppingCart/Index/{ProductID:int}")]
        public IActionResult Index(int? ProductID)
        {
            if (ProductID == null)
            {
                return NotFound();
            }

            using (var context = new ShopContext())
            {
                var paymentMethods = context.PaymentMethods.ToList();
                var shippingMethods = context.ShippingMethods.ToList();

                ViewData["PaymentMethods"] = new SelectList(paymentMethods, "PaymentMethodID", "Name");
                ViewData["ShippingMethods"] = new SelectList(shippingMethods, "ShippingMethodID", "Name");
            }

            _shoppingCart = CreateCart();
            var toRemove = _shoppingCart.ProductList.Find(x => x.Product.ProductID == ProductID);

            if(toRemove != null)
            {
                if (toRemove.Count > 1)
                {
                    toRemove.Count--;
                    toRemove.Sum -= toRemove.Product.Price;
                }
                else if (toRemove.Count == 1)
                {
                    toRemove.Count--;
                    toRemove.Sum -= toRemove.Product.Price;
                    _shoppingCart.ProductList.Remove(toRemove);
                }
            }

            UpdateCookies();
            return View(_shoppingCart);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("PaymentMethodID,ShippingMethodID,DiscountCode,ProductList,CartPrice")] ShoppingCart ShoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", order.CustomerID);
            ViewData["PaymentMethodID"] = new SelectList(_context.PaymentMethods, "PaymentMethodID", "PaymentMethodID", order.PaymentMethodID);
            ViewData["ShippingMethodID"] = new SelectList(_context.ShippingMethods, "ShippingMethodID", "ShippingMethodID", order.ShippingMethodID);
            return View(order);
        }*/

        [HttpPost("ShoppingCart/OrderCompleted")]
        public IActionResult OrderCompleted([Bind("PaymentMethodID,ShippingMethodID,DiscountCode")] ShoppingCart ShoppingCart)
        {
            var cart = CreateCart();
            ShoppingCart.ProductList = cart.ProductList;
            ShoppingCart.CartPrice = cart.countCartPrice();

            if (ModelState.IsValid)
            {
                if(ShoppingCart.ProductList.Count != 0)
                {
                    using (var context = new ShopContext())
                    {
                        var order = new Order
                        {
                            OrderStatus = State.Preparing,
                            PaymentMethodID = ShoppingCart.PaymentMethodID,
                            ShippingMethodID = ShoppingCart.ShippingMethodID,
                            CustomerID = 1                                                      // zmienić customera na tego zalogowanego
                        };
                        context.Orders.Add(order);
                        context.SaveChanges();              // dodanie OrderID przez EFCORE

                        foreach (var product in ShoppingCart.ProductList)
                        {
                            for (int i = 0; i < product.Count; i++)
                            {
                                var productOrder = new ProductOrder { OrderID = order.OrderID, ProductID = product.Product.ProductID };
                                context.ProductOrders.Add(productOrder);
                            }
                        }
                        context.SaveChanges();

                        var shippingMethod = context.ShippingMethods.Where(x => x.ShippingMethodID == ShoppingCart.ShippingMethodID).FirstOrDefault();
                        var paymentMethod = context.PaymentMethods.Where(x => x.PaymentMethodID == ShoppingCart.PaymentMethodID).FirstOrDefault();
                        var discountCode = context.DiscountCodes.Where(x => x.DiscoundCode == ShoppingCart.DiscountCode).FirstOrDefault();

                        ViewData["CenaBezRabatu"] = ShoppingCart.CartPrice;

                        if (shippingMethod != null)
                            ViewData["ShippingMethod"] = shippingMethod.Name;
                        if (paymentMethod != null)
                            ViewData["PaymentMethod"] = paymentMethod.Name;
                        if (discountCode != null)
                        {
                            ViewData["DiscountCode"] = discountCode.Percent;
                            decimal newPrice = ShoppingCart.CartPrice - (ShoppingCart.CartPrice * discountCode.Percent / 100);
                            ShoppingCart.CartPrice = newPrice;
                        }
                        else
                        {
                            ViewData["DiscountCode"] = 0;
                        }
                    }
                }
            }
            
            return View(ShoppingCart);
        }

        /*[HttpGet("ShoppingCart/OrderCompleted")]
        public IActionResult OrderCompleted()
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", order.CustomerID);
            ViewData["PaymentMethodID"] = new SelectList(_context.PaymentMethods, "PaymentMethodID", "PaymentMethodID", order.PaymentMethodID);
            ViewData["ShippingMethodID"] = new SelectList(_context.ShippingMethods, "ShippingMethodID", "ShippingMethodID", order.ShippingMethodID);

            
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel();
            shoppingCartViewModel.ShoppingCart = CreateCart();
            shoppingCartViewModel.PaymentMethod;
            return View(shoppingCartViewModel);

            _shoppingCart = CreateCart();
            return View(_shoppingCart);
        }*/

        public void UpdateCookies()
        {
            string cookiesvalue = "";

            foreach (var p in _shoppingCart.ProductList)
            {
                for (int i = 0; i < p.Count; i++)
                {
                    cookiesvalue += $"-{p.Product.ProductID}";
                }
            }

            Response.Cookies.Append("ShoppingCart", cookiesvalue);
        }
    }
}
