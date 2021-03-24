using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using ProjektSklep.Data;
using ProjektSklep.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektSklep.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        /*private bool checkIsAdmin()
        {
            using (var context = new ShopContext())
            {
                string LoggedUserEmail = User.Identity.Name;
                if (LoggedUserEmail.Length == 0)
                {
                    return false;
                }
                var customer = context.Customers.Where(x => x.Email == LoggedUserEmail).FirstOrDefault();
                return customer.AdminRights;
            }
            return false;
        }*/

        [HttpPost("ShoppingCart/OrderCompleted")]
        public async Task<IActionResult> OrderCompleted([Bind("PaymentMethodID,ShippingMethodID,DiscountCode")] ShoppingCart shoppingCart)
        {
            var cart = CreateCart();
            shoppingCart.ProductList = cart.ProductList;
            shoppingCart.CartPrice = cart.countCartPrice();

            if (ModelState.IsValid)
            {
                if(shoppingCart.ProductList.Count != 0)
                {
                    using (var context = new ShopContext())
                    {
                        string LoggedUserEmail = User.Identity.Name;
                        var customer = context.Customers.Where(x => x.Email == LoggedUserEmail).FirstOrDefault();

                        var order = new Order
                        {
                            OrderStatus = State.New,
                            PaymentMethodID = shoppingCart.PaymentMethodID,
                            ShippingMethodID = shoppingCart.ShippingMethodID,
                            CustomerID = customer.Id,                                                     // id zalogowanego customera
                            Price = shoppingCart.CartPrice
                        };
                        context.Orders.Add(order);
                        context.SaveChanges();              // dodanie OrderID przez EFCORE

                        List<Product> addedProducts = new List<Product>();          // zeby dodawac zamowiony produkt do bazy tylko raz (jak jest wieksza jego ilość)

                        foreach (var product in shoppingCart.ProductList)
                        {                          
                            try
                            {
                                product.Product.Amount -= product.Count;
                                product.Product.SoldProducts += product.Count;

                                //TODO: UJEMNE STOCKI: DODAĆ WIDOK, ŻE NIEDASIEZAMOWIĆ

                                _context.Update(product.Product);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                throw;
                            }
                           
                            /*for (int i = 0; i < product.Count; i++)
                            {
                                var productOrder = new ProductOrder { OrderID = order.OrderID, ProductID = product.Product.ProductID };

                                context.ProductOrders.Add(productOrder);
                            }*/

                            if(!addedProducts.Contains(product.Product))
                            {
                                var productOrder = new ProductOrder { OrderID = order.OrderID, ProductID = product.Product.ProductID, Quantity = product.Count };
                                context.ProductOrders.Add(productOrder);
                            }
                        }
                        context.SaveChanges();

                        var shippingMethod = context.ShippingMethods.Where(x => x.ShippingMethodID == shoppingCart.ShippingMethodID).FirstOrDefault();
                        var paymentMethod = context.PaymentMethods.Where(x => x.PaymentMethodID == shoppingCart.PaymentMethodID).FirstOrDefault();
                        var discountCode = context.DiscountCodes.Where(x => x.DiscoundCode == shoppingCart.DiscountCode).FirstOrDefault();

                        ViewData["CenaBezRabatu"] = shoppingCart.CartPrice;

                        if (shippingMethod != null)
                            ViewData["ShippingMethod"] = shippingMethod.Name;
                        if (paymentMethod != null)
                            ViewData["PaymentMethod"] = paymentMethod.Name;
                        if (discountCode != null)
                        {
                            ViewData["DiscountCode"] = discountCode.Percent;
                            decimal newPrice = shoppingCart.CartPrice - (shoppingCart.CartPrice * discountCode.Percent / 100);
                            shoppingCart.CartPrice = newPrice;

                            // zmiana ceny na cene po rabacie
                            var newOrder = context.Orders.Where(x => x.OrderID == order.OrderID).FirstOrDefault();
                            if(newOrder != null)
                            {
                                newOrder.Price = newPrice;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            ViewData["DiscountCode"] = 0;
                        }
                    }

                    // czyszczenie ciasteczka
                    var cookie = Request.Cookies["ShoppingCart"];
                    Response.Cookies.Delete("ShoppingCart");

                    // zwiekszanie licznika odwiedzin - odczyt
                    string path = "Content/Resources/visit_counter.txt";
                    FileStream fs = System.IO.File.OpenRead(path);
                    byte[] b = new byte[64];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    int result = 0;
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        Int32.TryParse(temp.GetString(b), out result);
                    }
                    fs.Close();
                    result++;
                    // zwiekszanie licznika odwiedzin - zapis
                    fs = System.IO.File.Create(path);
                    b = new UTF8Encoding(true).GetBytes(result.ToString());
                    fs.Write(b, 0, b.Length);
                    fs.Close();
                    Statistics.GetInstance().SetVisitors(result);
                }
            }
            return View(shoppingCart);
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
