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
    public class ExpertsController : Controller
    {
        private readonly ShopContext _context;

        public ExpertsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Experts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Experts.ToListAsync());
        }

        // GET: Experts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expert = await _context.Experts
                .FirstOrDefaultAsync(m => m.ExpertID == id);
            if (expert == null)
            {
                return NotFound();
            }

            return View(expert);
        }

        // GET: Experts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Experts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpertID,FirstName,LastName,Email")] Expert expert)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expert);
        }

        // GET: Experts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expert = await _context.Experts.FindAsync(id);
            if (expert == null)
            {
                return NotFound();
            }
            return View(expert);
        }

        // POST: Experts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpertID,FirstName,LastName,Email")] Expert expert)
        {
            if (id != expert.ExpertID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpertExists(expert.ExpertID))
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
            return View(expert);
        }

        // GET: Experts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expert = await _context.Experts
                .FirstOrDefaultAsync(m => m.ExpertID == id);
            if (expert == null)
            {
                return NotFound();
            }

            return View(expert);
        }

        // POST: Experts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expert = await _context.Experts.FindAsync(id);
            _context.Experts.Remove(expert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpertExists(int id)
        {
            return _context.Experts.Any(e => e.ExpertID == id);
        }
    }
}
