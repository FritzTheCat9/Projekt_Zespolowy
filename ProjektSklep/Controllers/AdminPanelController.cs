using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjektSklep.Data;
using ProjektSklep.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    public class AdminPanelController : Controller
    {
        private readonly ShopContext _context;
        private readonly ILogger<AdminPanelController> _logger;
        public AdminPanelController(ILogger<AdminPanelController> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
