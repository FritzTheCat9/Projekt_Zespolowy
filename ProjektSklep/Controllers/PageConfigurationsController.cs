using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektSklep.Data;
using ProjektSklep.Models;

namespace ProjektSklep
{
    public class PageConfigurationsController : Controller
    {
        private readonly ShopContext _context;

        public PageConfigurationsController(ShopContext context)
        {
            _context = context;
        }

        // GET: PageConfigurations
        public async Task<IActionResult> Index()
        {
            return View(await _context.PageConfigurations.ToListAsync());
        }

        // GET: PageConfigurations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageConfiguration = await _context.PageConfigurations
                .FirstOrDefaultAsync(m => m.PageConfigurationID == id);
            if (pageConfiguration == null)
            {
                return NotFound();
            }

            return View(pageConfiguration);
        }

        // GET: PageConfigurations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PageConfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PageConfigurationID,CustomerID,SendingNewsletter,ShowNetPrices,ProductsPerPage,InterfaceSkin,Language,Currency")] PageConfiguration pageConfiguration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pageConfiguration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pageConfiguration);
        }

        // GET: PageConfigurations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageConfiguration = await _context.PageConfigurations.FindAsync(id);
            if (pageConfiguration == null)
            {
                return NotFound();
            }
            return View(pageConfiguration);
        }

        // POST: PageConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageConfigurationID,CustomerID,SendingNewsletter,ShowNetPrices,ProductsPerPage,InterfaceSkin,Language,Currency")] PageConfiguration pageConfiguration)
        {
            if (id != pageConfiguration.PageConfigurationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pageConfiguration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageConfigurationExists(pageConfiguration.PageConfigurationID))
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
            return View(pageConfiguration);
        }

        // GET: PageConfigurations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageConfiguration = await _context.PageConfigurations
                .FirstOrDefaultAsync(m => m.PageConfigurationID == id);
            if (pageConfiguration == null)
            {
                return NotFound();
            }

            return View(pageConfiguration);
        }

        // POST: PageConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageConfiguration = await _context.PageConfigurations.FindAsync(id);
            _context.PageConfigurations.Remove(pageConfiguration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageConfigurationExists(int id)
        {
            return _context.PageConfigurations.Any(e => e.PageConfigurationID == id);
        }
    }
}
