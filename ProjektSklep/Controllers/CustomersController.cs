using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektSklep.Data;
using ProjektSklep.Models;

namespace ProjektSklep
{
    public class CustomersController : Controller
    {
        private readonly ShopContext _context;

        public CustomersController(ShopContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Verify(Customer acc)
        {
            //List<Customer> indentification = _context.Customers.FromSqlRaw("SELECT * FROM Customers WHERE Login='" + acc.Login + "' AND Password='" + acc.Password + "' ").ToList();
            return View();
        }

        // GET: Customers
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Customers.Include(c => c.Address).Include(c => c.PageConfiguration);
            return View(await shopContext.ToListAsync());
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.PageConfiguration)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        /*
        // GET: Customers/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "AddressID");
            ViewData["PageConfigurationID"] = new SelectList(_context.PageConfigurations, "PageConfigurationID", "PageConfigurationID");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,AddressID,PageConfigurationID,FirstName,LastName,Login,Password,Email,AdminRights")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "AddressID", customer.AddressID);
            ViewData["PageConfigurationID"] = new SelectList(_context.PageConfigurations, "PageConfigurationID", "PageConfigurationID", customer.PageConfigurationID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "AddressID", customer.AddressID);
            ViewData["PageConfigurationID"] = new SelectList(_context.PageConfigurations, "PageConfigurationID", "PageConfigurationID", customer.PageConfigurationID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AddressID,PageConfigurationID,FirstName,LastName")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "AddressID", customer.AddressID);
            ViewData["PageConfigurationID"] = new SelectList(_context.PageConfigurations, "PageConfigurationID", "PageConfigurationID", customer.PageConfigurationID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.PageConfiguration)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }*/
    }
}
