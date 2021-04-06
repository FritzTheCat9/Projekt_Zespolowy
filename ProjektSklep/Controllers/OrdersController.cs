using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektSklep.Data;
using ProjektSklep.Models;
using ProjektSklep.Models.ViewModels;

namespace ProjektSklep
{
    public class OrdersController : Controller
    {
        private readonly ShopContext _context;

        public OrdersController(ShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.ShippingMethod);
            return View(await shopContext.ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.PaymentMethod)
                    .Include(o => o.ShippingMethod)
                    .FirstOrDefaultAsync(m => m.OrderID == id);
                if (order == null)
                {
                    return NotFound();
                }

                OrderViewModel o = new OrderViewModel(order.OrderID, order.Customer, order.PaymentMethod, order.ShippingMethod, order.OrderStatus, order.Price);
                //List<int> ordersProductIDs = new List<int>();
                //IEnumerable<ProductOrder> pairs =
                //from Order in _context.ProductOrders
                //where Order.OrderID == order.OrderID
                //select Product;

                var productOrders = _context.ProductOrders.Include(o => o.Product);
                foreach (var p in productOrders)
                {
                    if (p.OrderID == order.OrderID)
                    {
                        o.addProduct(p.Product);
                    }
                }

                foreach (var p in productOrders)
                {
                    if (p.OrderID == order.OrderID)
                    {
                        o.addProductIdQuantity(p.Product, p.Quantity);
                    }
                }


                //var ordersProductIDs = _context.ProductOrders.Include(p => p.Product).Where(p => p.OrderID == order.OrderID);
                //foreach (Product p in ordersProductIDs.Product)
                //{

                //}
                // o.Order = ordersProductIDs

                return View(o);
            }

            
        }

        // GET: Orders/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["PaymentMethodID"] = new SelectList(_context.PaymentMethods, "PaymentMethodID", "Name");
            ViewData["ShippingMethodID"] = new SelectList(_context.ShippingMethods, "ShippingMethodID", "Name");
            ViewData["OrderStatus"] = new SelectList(Enum.GetValues(typeof(State)).Cast<State>());
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,CustomerID,ShippingMethodID,PaymentMethodID,OrderStatus,Price")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "Id", "CustomerID", order.CustomerID);
            ViewData["PaymentMethodID"] = new SelectList(_context.PaymentMethods, "PaymentMethodID", "PaymentMethodID", order.PaymentMethodID);
            ViewData["ShippingMethodID"] = new SelectList(_context.ShippingMethods, "ShippingMethodID", "ShippingMethodID", order.ShippingMethodID);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "Id", "Email", order.CustomerID);
            ViewData["PaymentMethodID"] = new SelectList(_context.PaymentMethods, "PaymentMethodID", "Name", order.PaymentMethodID);
            ViewData["ShippingMethodID"] = new SelectList(_context.ShippingMethods, "ShippingMethodID", "Name", order.ShippingMethodID);
            ViewData["OrderStatus"] = new SelectList(Enum.GetValues(typeof(State)).Cast<State>());
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,CustomerID,ShippingMethodID,PaymentMethodID,OrderStatus,Price")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();

                    // Wysłanie emaila o stanie zamówienia
                    var order1 = await _context.Orders
                       .Include(o => o.Customer)
                       .Include(o => o.PaymentMethod)
                       .Include(o => o.ShippingMethod)
                       .FirstOrDefaultAsync(o => o.OrderID == id);

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("klientklientowski123@gmail.com");
                        mail.To.Add(order1.Customer.Email);
                        mail.Subject = $"Zmieniono stan zamówienia na {order1.OrderStatus}";
                        mail.Body = $"Zmieniono stan zamówienia na {order1.OrderStatus}";
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("klientklientowski123@gmail.com", "Klient123!");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "Id", "CustomerID", order.CustomerID);
            ViewData["PaymentMethodID"] = new SelectList(_context.PaymentMethods, "PaymentMethodID", "PaymentMethodID", order.PaymentMethodID);
            ViewData["ShippingMethodID"] = new SelectList(_context.ShippingMethods, "ShippingMethodID", "ShippingMethodID", order.ShippingMethodID);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ShippingMethod)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmailAboutOrder(int id)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.PaymentMethod)
                    .Include(o => o.ShippingMethod)
                    .FirstOrDefaultAsync(o => o.OrderID == id);

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("klientklientowski123@gmail.com");
                    mail.To.Add(order.Customer.Email);
                    mail.Subject = $"Zmieniono stan zamówienia na {order.OrderStatus}";
                    mail.Body = $"Zmieniono stan zamówienia na {order.OrderStatus}";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("klientklientowski123@gmail.com", "Klient123!");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            return RedirectToAction("Index");
        }*/
    }
}
