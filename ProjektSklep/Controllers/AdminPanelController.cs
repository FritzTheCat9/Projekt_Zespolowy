using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjektSklep.Data;
using ProjektSklep.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    public class AdminPanelController : Controller
    {
        private readonly ShopContext _context;
        private readonly ILogger<AdminPanelController> _logger;
        private readonly IConfiguration _config;
        public AdminPanelController(ILogger<AdminPanelController> logger, ShopContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            string savedEmail = _config.GetSection("Settings").GetSection("ContactFormEmail").Value;
            ViewBag.savedEmail = savedEmail;

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult SendNewsletters()
        {
            var pageConfigurations = _context.PageConfigurations.ToListAsync().Result;
            List<Customer> klienci = new List<Customer>();

            foreach (var pageConf in pageConfigurations)
            {
                if(pageConf.SendingNewsletter)
                {
                    var customer = _context.Customers.Where(x => x.PageConfigurationID == pageConf.PageConfigurationID).FirstAsync().Result;
                    klienci.Add(customer);
                }
            }

            var promocje = _context.Products.Include(p => p.Category).Where(p => p.Promotion == true);
            var nowosci = _context.Products.Include(p => p.Category).Where(p => p.DateAdded <= DateTime.Now && p.DateAdded >= DateTime.Now.AddDays(-30));

            foreach (var client in klienci)
            {
                // Wysłanie mejla z promocjami i nowościami
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("klientklientowski123@gmail.com");
                    mail.To.Add(client.Email);
                    mail.Subject = $"Newsletter - nowości i promocje w sklepie!";

                    var body = $"Newsletter - nowości i promocje w sklepie! <br>" +
                    $"<br>Promocje: <br>";

                    foreach (var p in promocje)
                    {
                        body += $"Nazwa: {p.Name}, Cena: {p.Price} <br>";
                    }

                    body += $"<br>Nowości: <br>";
                    foreach (var n in nowosci)
                    {
                        body += $"Nazwa: {n.Name}, Cena: {n.Price} <br>";
                    }

                    mail.Body = body;
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
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ChangeFormEmail([Bind("Email")] AdminPanelViewModel adminPanel)
        {
            _config.GetSection("Settings").GetSection("ContactFormEmail").Value = adminPanel.Email;
            return RedirectToAction("Index");
        }
    }

}
