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
    public class IndividualAssignmentHistoriesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public IndividualAssignmentHistoriesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: IndividualAssignmentHistories
        public async Task<IActionResult> Index()
        {
            var serveMeHRCoreContext = _context.IndividualAssignmentHistories.Include(i => i.AssignedToNavigation).Include(i => i.ServiceRequestNavigation);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: IndividualAssignmentHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualAssignmentHistories = await _context.IndividualAssignmentHistories
                .Include(i => i.AssignedToNavigation)
                .Include(i => i.ServiceRequestNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (individualAssignmentHistories == null)
            {
                return NotFound();
            }

            return View(individualAssignmentHistories);
        }

        // GET: IndividualAssignmentHistories/Create
        public IActionResult Create()
        {
            ViewData["AssignedTo"] = new SelectList(_context.Members, "Id", "MemberEmail");
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription");
            return View();
        }

        // POST: IndividualAssignmentHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssignedTo,AssignedBy,DateAssigned,ServiceRequest")] IndividualAssignmentHistories individualAssignmentHistories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(individualAssignmentHistories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedTo"] = new SelectList(_context.Members, "Id", "MemberEmail", individualAssignmentHistories.AssignedTo);
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", individualAssignmentHistories.ServiceRequest);
            return View(individualAssignmentHistories);
        }

        // GET: IndividualAssignmentHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualAssignmentHistories = await _context.IndividualAssignmentHistories.SingleOrDefaultAsync(m => m.Id == id);
            if (individualAssignmentHistories == null)
            {
                return NotFound();
            }
            ViewData["AssignedTo"] = new SelectList(_context.Members, "Id", "MemberEmail", individualAssignmentHistories.AssignedTo);
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", individualAssignmentHistories.ServiceRequest);
            return View(individualAssignmentHistories);
        }

        // POST: IndividualAssignmentHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssignedTo,AssignedBy,DateAssigned,ServiceRequest")] IndividualAssignmentHistories individualAssignmentHistories)
        {
            if (id != individualAssignmentHistories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(individualAssignmentHistories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndividualAssignmentHistoriesExists(individualAssignmentHistories.Id))
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
            ViewData["AssignedTo"] = new SelectList(_context.Members, "Id", "MemberEmail", individualAssignmentHistories.AssignedTo);
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", individualAssignmentHistories.ServiceRequest);
            return View(individualAssignmentHistories);
        }

        // GET: IndividualAssignmentHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualAssignmentHistories = await _context.IndividualAssignmentHistories
                .Include(i => i.AssignedToNavigation)
                .Include(i => i.ServiceRequestNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (individualAssignmentHistories == null)
            {
                return NotFound();
            }

            return View(individualAssignmentHistories);
        }

        // POST: IndividualAssignmentHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var individualAssignmentHistories = await _context.IndividualAssignmentHistories.SingleOrDefaultAsync(m => m.Id == id);
            _context.IndividualAssignmentHistories.Remove(individualAssignmentHistories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndividualAssignmentHistoriesExists(int id)
        {
            return _context.IndividualAssignmentHistories.Any(e => e.Id == id);
        }
    }
}
