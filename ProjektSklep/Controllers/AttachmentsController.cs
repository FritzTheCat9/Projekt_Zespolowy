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
    public class AttachmentsController : Controller
    {
        private readonly ShopContext _context;

        public AttachmentsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Attachments
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Attachments.Include(a => a.Product);
            return View(await shopContext.ToListAsync());
        }

        // GET: Attachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .Include(a => a.Product)
                .FirstOrDefaultAsync(m => m.AttachmentID == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // GET: Attachments/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID");
            return View();
        }

        // POST: Attachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttachmentID,ProductID,Path,Description")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attachment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", attachment.ProductID);
            return View(attachment);
        }

        // GET: Attachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", attachment.ProductID);
            return View(attachment);
        }

        // POST: Attachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttachmentID,ProductID,Path,Description")] Attachment attachment)
        {
            if (id != attachment.AttachmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentExists(attachment.AttachmentID))
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
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", attachment.ProductID);
            return View(attachment);
        }

        // GET: Attachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .Include(a => a.Product)
                .FirstOrDefaultAsync(m => m.AttachmentID == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentExists(int id)
        {
            return _context.Attachments.Any(e => e.AttachmentID == id);
        }
    }
}
