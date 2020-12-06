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
using Microsoft.AspNetCore.Identity;

namespace ProjektSklep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ShopContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddUsers()
        {
            var user = new IdentityUser { UserName = "bartlomiejuminski1999@gmail.com", Email = "bartlomiejuminski1999@gmail.com", EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, "Uminski123!");
            var user2 = new IdentityUser { UserName = "kacpersiegienczuk@gmail.com", Email = "kacpersiegienczuk@gmail.com", EmailConfirmed = true };
            var result2 = await _userManager.CreateAsync(user2, "Siegienczuk123!");
            var user3 = new IdentityUser { UserName = "michalkozikowski@gmail.com", Email = "michalkozikowski@gmail.com", EmailConfirmed = true };
            var result3 = await _userManager.CreateAsync(user3, "Kozikowski123!");
            var user4 = new IdentityUser { UserName = "jakubkozlowski@gmail.com", Email = "jakubkozlowski@gmail.com", EmailConfirmed = true };
            var result4 = await _userManager.CreateAsync(user4, "Kozlowski123!");
            var user5 = new IdentityUser { UserName = "klientklientowski@gmail.com", Email = "klientklientowski@gmail.com", EmailConfirmed = true };
            var result5 = await _userManager.CreateAsync(user5, "Klient123!");

            return Index();
        }

        // Wyświetlenie wszystkich produktów i kategorii
        [HttpGet("")]
        [HttpGet("Home/Index")]
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);

            AddUsers();

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
