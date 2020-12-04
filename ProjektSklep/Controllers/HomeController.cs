using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjektSklep.Data;
using ProjektSklep.Models;
using ProjektSklep.Models.ViewModels;

namespace ProjektSklep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Wyświetlenie wszystkich produktów i kategorii
        [HttpGet("")]
        [HttpGet("Home/Index")]
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
            return View(homeViewModel);
        }

        // Pobranie produktów danej kategorii
        [HttpGet("Home/Index/{CategoryID:int}")]
        public IActionResult Index(int? CategoryID)
        {
            if (CategoryID == null)
            {
                return NotFound();
            }

            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Where(p => p.Category.CategoryID == CategoryID);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);

            if (homeViewModel == null)
            {
                return NotFound();
            }

            return View(homeViewModel);
        }

        // Pobranie produktów danej kategorii
        [HttpGet("Home/ShoppingCartAddProduct/{ProductID:int}")]
        public IActionResult ShoppingCartAddProduct(int? ProductID)
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

            return Redirect("~/Home/Index");       // zmienic na poprzednią ścieżke
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
