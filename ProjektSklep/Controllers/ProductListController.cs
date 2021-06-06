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

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, 
            string SearchString_Description, int? SearchString_PriceFrom, int? SearchString_PriceTo, string SearchString_Promotion)
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
            ViewData["CurrentFilter2"] = SearchString_Description;
            ViewData["CurrentFilter3"] = SearchString_PriceFrom;
            ViewData["CurrentFilter4"] = SearchString_PriceTo;
            ViewData["CurrentFilter5"] = SearchString_Promotion;

            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(SearchString_Description))
            {
                products = products.Where(p => p.ProductDescription.Contains(SearchString_Description));
            }

            if(SearchString_PriceFrom.HasValue)
            {
                products = products.Where(p => p.Price >= SearchString_PriceFrom.Value);
            }

            if (SearchString_PriceTo.HasValue)
            {
                products = products.Where(p => p.Price <= SearchString_PriceTo.Value);
            }

            if (!string.IsNullOrEmpty(SearchString_Promotion))
            {
                if (SearchString_Promotion == "on")
                    products = products.Where(p => p.Promotion == true);
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
