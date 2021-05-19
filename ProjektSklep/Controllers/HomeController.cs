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
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace ProjektSklep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Customer> _userManager;
        private readonly IConfiguration _config;

        //dodanie stringa potrzebnego do wyszukiwania
        public string SearchTerm { get; set; }

        public HomeController(ILogger<HomeController> logger, ShopContext context, UserManager<Customer> userManager, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _config = config;
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

        [HttpPost]
        public IActionResult Index(string? productSearch)
        {
            var homeViewModel = new HomeViewModel();
            if (!String.IsNullOrEmpty(productSearch))
            {
                homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Where(p => p.Name.Contains(productSearch));
                homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
                return View(homeViewModel);
            }

            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
            return View(homeViewModel);
        }

        public IActionResult ContactForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactForm([Bind("Name,Email,Topic,Message")] ContactFormViewModel contact)
        {
            if (ModelState.IsValid)
            {
                if(contact.Name != "" && contact.Email != null && contact.Topic != null && contact.Message != null)
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("klientklientowski123@gmail.com");
                        string savedEmail = _config.GetSection("Settings").GetSection("ContactFormEmail").Value;
                        mail.To.Add(savedEmail);
                        mail.Subject = $"ContactForm - " + contact.Topic;

                        var body = $"<br><b>ContactForm</b><br>";
                        body += $"Name: {contact.Name}<br>";
                        body += $"Email: {contact.Email}<br>";
                        body += $"Topic: {contact.Topic}<br>";
                        body += $"Topic: {contact.Message}<br>";

                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("klientklientowski123@gmail.com", "Klient123!");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("ContactForm");
        }

        [HttpGet("Home/Product/{ProductID:int}")]
        public IActionResult Product(int ProductID)
        {
            var p = new Product();
            //if (ProductID != null)
            //{ 
                p = _context.Products.Include(c => c.Category).Include(e => e.Expert).Where(p => p.ProductID == ProductID).FirstOrDefault();
            //}      
            //p.Visibility = false;
            //tutaj chyba trzeba dodać walidację na wypadek gdyby coś się zepsuło;
            //na razie zakładam że klient klika link do produktu i go zawsze poprawnie przenosi do widoku
            ViewData["Categories"] = _context.Categories.Include(c => c.Parent);
            return View(p);
 
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

        // Metoda zwiazana z wyswietleniem strony z konfiguracja
        [HttpGet("Home/Configuration")]
        public IActionResult Configuration()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Include(p => p.Attachments);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
            return View(homeViewModel);
        }

        [HttpGet("Home/Promotion")]
        public IActionResult Promotion()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Include(p => p.Attachments)
                .Where(p => p.Promotion == true);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
            return View(homeViewModel);
        }

        [HttpGet("Home/NewProducts")]
        public IActionResult NewProducts()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Include(p => p.Attachments)
                .Where(p => p.DateAdded <= DateTime.Now && p.DateAdded >= DateTime.Now.AddDays(-30));
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
            return View(homeViewModel);
        }

        [HttpGet("Home/Bestsellers")]
        public IActionResult Bestsellers()
        {
            var homeViewModel = new HomeViewModel();
            //homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Include(p => p.Attachments)
            //    .Where(p => p.SoldProducts > 100);
            homeViewModel.Products = _context.Products.Include(p => p.Category).Include(p => p.Expert).Include(p => p.Attachments)
                .OrderByDescending(p => p.SoldProducts).Take(10);
            homeViewModel.Categories = _context.Categories.Include(c => c.Parent);
            return View(homeViewModel);
        }

        [HttpGet("Home/RecycleBin")]
        public IActionResult RecycleBin()
        {
            var homeViewModel = new HomeViewModel();
            
            return View();
        }

        // Dodanie produktu do koszyka
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

            return RedirectToAction("Product", "Home", new { @id = ProductID });       // zmienic na poprzednią ścieżke
        }

        // Dodanie produktu do koszyka ale 2 parametry
        //[HttpGet("Home/ShoppingCartAddProduct/{ProductID:int}/{Quantity:int}")]
        public IActionResult ShoppingCartAddProduct(int? ProductID, int Quantity)
        {
            if (ProductID == null)
            {
                return NotFound();
            }

            var cookie = Request.Cookies["ShoppingCart"];
            string itemsToAdd = $"";
            if (cookie == null)
            {
                
                for (int i = 0; i < Quantity; i++)
                {
                    if (i>0)
                    {
                        itemsToAdd += $"-";
                    }
                    itemsToAdd += $"{ProductID}";
                }
                Response.Cookies.Append("ShoppingCart", $"{itemsToAdd}");

            }
            else
            {
                for (int i=0; i<Quantity; i++)
                {
                    itemsToAdd += $"-{ProductID}";
                }
                Response.Cookies.Append("ShoppingCart", $"{cookie}{itemsToAdd}");
            }

            return RedirectToAction("Product", "Home", new { @id = ProductID });       // zmienic na poprzednią ścieżke
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
