using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjektSklep.Data;
using ProjektSklep.Models;
using ProjektSklep.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjektSklep.Controllers
{
    public class ClientPanelController : Controller
    {
        private readonly ShopContext _context;
        private readonly ILogger<ClientPanelController> _logger;
        private readonly UserManager<Customer> _userManager;
        public ClientPanelController(ILogger<ClientPanelController> logger, ShopContext context, UserManager<Customer> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _context.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.ShippingMethod)
                .Where(c => c.Customer.Id == userId);

            return View(order.ToList());
        }
        [HttpGet("ClientPanel/Details/{OrderID:int}")]
        public IActionResult Details(int OrderID)
        {
            var order = _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.PaymentMethod)
                    .Include(o => o.ShippingMethod)
                    .FirstOrDefault(m => m.OrderID == OrderID);

            OrderViewModel o = new OrderViewModel(order.OrderID, order.Customer, order.PaymentMethod, order.ShippingMethod, order.OrderStatus, order.Price);

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

            return View(o);
        }
    }
}
