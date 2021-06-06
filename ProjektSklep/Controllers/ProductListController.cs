using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektSklep.Data;
using ProjektSklep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ShopContext _context;

        public ProductListController(ShopContext context)
        {

            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 5;
            string LoggedUserEmail = User.Identity.Name;
            var customer = _context.Customers.Where(x => x.Email == LoggedUserEmail).Include(x => x.PageConfiguration).FirstOrDefault();
            if(customer != null)
            {
                pageSize = customer.PageConfiguration.ProductsPerPage;
            }

            // Widoczność
            products = products.Where(x => x.Visibility == true);
            products = products.Include(x => x.Category).Where(x => x.Category.Visibility == true);

            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
