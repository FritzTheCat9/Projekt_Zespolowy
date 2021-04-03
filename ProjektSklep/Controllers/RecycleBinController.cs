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
    public class RecycleBinController : Controller
    {
        private RecycleBin _recycleBin;

        private readonly ShopContext _context;

        private readonly ILogger<HomeController> _logger;

        public RecycleBinController(ILogger<HomeController> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            _recycleBin = CreateRecycleBin();
            return View(_recycleBin);
        }

        // Usuwanie produktu z kosza.
        [HttpGet("RecycleBin/Index/{ProductID:int}")]
        public IActionResult Index(int? ProductID)
        {
            if (ProductID == null)
            {
                return NotFound();
            }

            _recycleBin = CreateRecycleBin();
            var toRemove = _recycleBin.ShoppingCartElementList.Find(x => x.Product.ProductID == ProductID);

            if (toRemove != null)
            {
                _recycleBin.ShoppingCartElementList.Remove(toRemove);
            }

            UpdateCookies();
            return View(_recycleBin);
        }

        // Dodawanie produktu z kosza z powrotem do koszyka.
        [HttpGet("RecycleBin/RestoreProduct/{ProductID:int}")]
        public IActionResult RestoreProduct(int? ProductID)
        {
            if (ProductID == null)
            {
                return NotFound();
            }

            var cookie = Request.Cookies["ShoppingCart"];
            if (cookie == null)
            {
                Response.Cookies.Append("ShoppingCart", ProductID.ToString());
            }
            else
            {
                Response.Cookies.Append("ShoppingCart", $"{cookie}-{ProductID}");
            }

            return RedirectToAction("Index", "RecycleBin");
        }

        // Przekierowanie do strony z produktem.
        [HttpGet("RecycleBin/RedirectToProduct/{ProductID:int}")]
        public IActionResult RedirectToProduct(int? ProductID)
        {
            if (ProductID == null)
            {
                return NotFound();
            }

            return RedirectToAction("Product", "Home", new { @id = ProductID });
        }

        private RecycleBin CreateRecycleBin()
        {
            _recycleBin = new RecycleBin();
            _recycleBin.ShoppingCartElementList = new List<ShoppingCartElement>();
            List<Product> products = _context.Products.Include(p => p.Category).Include(p => p.Expert).ToList();

            var cookie = Request.Cookies["RecycleBin"];
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
                            var product = _recycleBin.ShoppingCartElementList.Find(x => x.Product.ProductID == id);
                            if (product == null)
                            {
                                _recycleBin.ShoppingCartElementList.Add(new ShoppingCartElement { Product = elem, Count = 1, Sum = elem.Price });
                            }
                        }
                    }
                }
            }
            return _recycleBin;
        }

        private void UpdateCookies()
        {
            string cookiesValue = "";

            foreach (var p in _recycleBin.ShoppingCartElementList)
            {
                cookiesValue += $"-{p.Product.ProductID}";
            }

            Response.Cookies.Append("RecycleBin", cookiesValue);
        }
    }
}
