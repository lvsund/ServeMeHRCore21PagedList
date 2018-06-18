using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServeMeHRCore21.Models;

namespace ServeMeHRCore21.Controllers
{
    public class StatusSetsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public StatusSetsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: StatusSets
        public async Task<IActionResult> Index()
        {
            var serveMeHRCoreContext = _context.StatusSets.Include(s => s.StatusTypeNavigation);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: StatusSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusSets = await _context.StatusSets
                .Include(s => s.StatusTypeNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (statusSets == null)
            {
                return NotFound();
            }

            return View(statusSets);
        }

        // GET: StatusSets/Create
        public IActionResult Create()
        {
            ViewData["StatusType"] = new SelectList(_context.StatusTypes, "Id", "StatusTypeDescription");
            var model = new StatusSets();
            model.LastUpdated = System.DateTime.Now;
            model.Active = true;

            return View();
        }

        // POST: StatusSets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StatusDescription,LastUpdated,Active,StatusType")] StatusSets statusSets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusSets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusType"] = new SelectList(_context.StatusTypes, "Id", "StatusTypeDescription", statusSets.StatusType);
            return View(statusSets);
        }

        // GET: StatusSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusSets = await _context.StatusSets.SingleOrDefaultAsync(m => m.Id == id);
            if (statusSets == null)
            {
                return NotFound();
            }
            ViewData["StatusType"] = new SelectList(_context.StatusTypes, "Id", "StatusTypeDescription", statusSets.StatusType);
            return View(statusSets);
        }

        // POST: StatusSets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StatusDescription,LastUpdated,Active,StatusType")] StatusSets statusSets)
        {
            if (id != statusSets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusSets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusSetsExists(statusSets.Id))
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
            ViewData["StatusType"] = new SelectList(_context.StatusTypes, "Id", "StatusTypeDescription", statusSets.StatusType);
            return View(statusSets);
        }

        // GET: StatusSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusSets = await _context.StatusSets
                .Include(s => s.StatusTypeNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (statusSets == null)
            {
                return NotFound();
            }

            return View(statusSets);
        }

        // POST: StatusSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusSets = await _context.StatusSets.SingleOrDefaultAsync(m => m.Id == id);
            _context.StatusSets.Remove(statusSets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusSetsExists(int id)
        {
            return _context.StatusSets.Any(e => e.Id == id);
        }
    }
}
