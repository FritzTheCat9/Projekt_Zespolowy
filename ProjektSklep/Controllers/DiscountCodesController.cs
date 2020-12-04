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
    public class DiscountCodesController : Controller
    {
        private readonly ShopContext _context;

        public DiscountCodesController(ShopContext context)
        {
            _context = context;
        }

        // GET: DiscountCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiscountCodes.ToListAsync());
        }

        // GET: DiscountCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCodes
                .FirstOrDefaultAsync(m => m.DiscountCodeID == id);
            if (discountCode == null)
            {
                return NotFound();
            }

            return View(discountCode);
        }

        // GET: DiscountCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiscountCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountCodeID,DiscoundCode,Percent")] DiscountCode discountCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discountCode);
        }

        // GET: DiscountCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCodes.FindAsync(id);
            if (discountCode == null)
            {
                return NotFound();
            }
            return View(discountCode);
        }

        // POST: DiscountCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountCodeID,DiscoundCode,Percent")] DiscountCode discountCode)
        {
            if (id != discountCode.DiscountCodeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountCodeExists(discountCode.DiscountCodeID))
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
            return View(discountCode);
        }

        // GET: DiscountCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCodes
                .FirstOrDefaultAsync(m => m.DiscountCodeID == id);
            if (discountCode == null)
            {
                return NotFound();
            }

            return View(discountCode);
        }

        // POST: DiscountCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discountCode = await _context.DiscountCodes.FindAsync(id);
            _context.DiscountCodes.Remove(discountCode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountCodeExists(int id)
        {
            return _context.DiscountCodes.Any(e => e.DiscountCodeID == id);
        }
    }
}
