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
    public class StepHistoriesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public StepHistoriesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: StepHistories
        public async Task<IActionResult> Index()
        {
            var serveMeHRCoreContext = _context.StepHistories.Include(s => s.RequestTypeStepNavigation).Include(s => s.ServiceRequestNavigation);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: StepHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stepHistories = await _context.StepHistories
                .Include(s => s.RequestTypeStepNavigation)
                .Include(s => s.ServiceRequestNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stepHistories == null)
            {
                return NotFound();
            }

            return View(stepHistories);
        }

        // GET: StepHistories/Create
        public IActionResult Create()
        {
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription");
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription");
            return View();
        }

        // POST: StepHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastUpdated,RequestTypeStep,ServiceRequest")] StepHistories stepHistories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stepHistories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription", stepHistories.RequestTypeStep);
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", stepHistories.ServiceRequest);
            return View(stepHistories);
        }

        // GET: StepHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stepHistories = await _context.StepHistories.SingleOrDefaultAsync(m => m.Id == id);
            if (stepHistories == null)
            {
                return NotFound();
            }
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription", stepHistories.RequestTypeStep);
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", stepHistories.ServiceRequest);
            return View(stepHistories);
        }

        // POST: StepHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastUpdated,RequestTypeStep,ServiceRequest")] StepHistories stepHistories)
        {
            if (id != stepHistories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stepHistories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StepHistoriesExists(stepHistories.Id))
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
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription", stepHistories.RequestTypeStep);
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", stepHistories.ServiceRequest);
            return View(stepHistories);
        }

        // GET: StepHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stepHistories = await _context.StepHistories
                .Include(s => s.RequestTypeStepNavigation)
                .Include(s => s.ServiceRequestNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stepHistories == null)
            {
                return NotFound();
            }

            return View(stepHistories);
        }

        // POST: StepHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stepHistories = await _context.StepHistories.SingleOrDefaultAsync(m => m.Id == id);
            _context.StepHistories.Remove(stepHistories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StepHistoriesExists(int id)
        {
            return _context.StepHistories.Any(e => e.Id == id);
        }
    }
}
