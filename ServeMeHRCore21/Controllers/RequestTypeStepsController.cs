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
    public class RequestTypeStepsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public RequestTypeStepsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: RequestTypeSteps
        public async Task<IActionResult> Index(string SelectedRequestType)
        {
            IEnumerable<SelectListItem> requestTypeitems = _context.RequestTypes.Select(c => new SelectListItem
            {
                //Selected = c.Id == 1,
                Value = c.RequestTypeDescription,
                Text = c.RequestTypeDescription
            });
            ViewBag.SelectedRequestType = requestTypeitems;

            var serveMeHRCoreContext = _context.RequestTypeSteps
                .Include(r => r.RequestTypeNavigation)
                .Where(r => r.RequestTypeNavigation.RequestTypeDescription == SelectedRequestType);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: RequestTypeSteps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestTypeSteps = await _context.RequestTypeSteps
                .Include(r => r.RequestTypeNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requestTypeSteps == null)
            {
                return NotFound();
            }

            return View(requestTypeSteps);
        }

        // GET: RequestTypeSteps/Create
        public IActionResult Create()
        {
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription");
            var model = new RequestTypeSteps();
            model.LastUpdated = System.DateTime.Now;
            model.Active = true;

            return View(model);
        }

        // POST: RequestTypeSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StepDescription,StepNumber,LastUpdated,Active,RequestType")] RequestTypeSteps requestTypeSteps)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestTypeSteps);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription", requestTypeSteps.RequestType);
            return View(requestTypeSteps);
        }

        // GET: RequestTypeSteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestTypeSteps = await _context.RequestTypeSteps.SingleOrDefaultAsync(m => m.Id == id);
            if (requestTypeSteps == null)
            {
                return NotFound();
            }
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription", requestTypeSteps.RequestType);
            return View(requestTypeSteps);
        }

        // POST: RequestTypeSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StepDescription,StepNumber,LastUpdated,Active,RequestType")] RequestTypeSteps requestTypeSteps)
        {
            if (id != requestTypeSteps.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestTypeSteps);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestTypeStepsExists(requestTypeSteps.Id))
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
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription", requestTypeSteps.RequestType);
            return View(requestTypeSteps);
        }

        // GET: RequestTypeSteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestTypeSteps = await _context.RequestTypeSteps
                .Include(r => r.RequestTypeNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requestTypeSteps == null)
            {
                return NotFound();
            }

            return View(requestTypeSteps);
        }

        // POST: RequestTypeSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestTypeSteps = await _context.RequestTypeSteps.SingleOrDefaultAsync(m => m.Id == id);
            _context.RequestTypeSteps.Remove(requestTypeSteps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestTypeStepsExists(int id)
        {
            return _context.RequestTypeSteps.Any(e => e.Id == id);
        }
    }
}
