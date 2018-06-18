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
    public class StatusTypesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public StatusTypesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: StatusTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatusTypes.ToListAsync());
        }

        // GET: StatusTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusTypes = await _context.StatusTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (statusTypes == null)
            {
                return NotFound();
            }

            return View(statusTypes);
        }

        // GET: StatusTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StatusTypeDescription")] StatusTypes statusTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusTypes);
        }

        // GET: StatusTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusTypes = await _context.StatusTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (statusTypes == null)
            {
                return NotFound();
            }
            return View(statusTypes);
        }

        // POST: StatusTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StatusTypeDescription")] StatusTypes statusTypes)
        {
            if (id != statusTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusTypesExists(statusTypes.Id))
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
            return View(statusTypes);
        }

        // GET: StatusTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusTypes = await _context.StatusTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (statusTypes == null)
            {
                return NotFound();
            }

            return View(statusTypes);
        }

        // POST: StatusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusTypes = await _context.StatusTypes.SingleOrDefaultAsync(m => m.Id == id);
            _context.StatusTypes.Remove(statusTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusTypesExists(int id)
        {
            return _context.StatusTypes.Any(e => e.Id == id);
        }
    }
}
