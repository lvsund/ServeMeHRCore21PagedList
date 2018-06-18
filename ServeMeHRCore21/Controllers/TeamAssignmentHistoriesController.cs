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
    public class TeamAssignmentHistoriesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public TeamAssignmentHistoriesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: TeamAssignmentHistories
        public async Task<IActionResult> Index()
        {
            var serveMeHRCoreContext = _context.TeamAssignmentHistories.Include(t => t.ServiceRequestNavigation).Include(t => t.TeamNavigation);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: TeamAssignmentHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamAssignmentHistories = await _context.TeamAssignmentHistories
                .Include(t => t.ServiceRequestNavigation)
                .Include(t => t.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamAssignmentHistories == null)
            {
                return NotFound();
            }

            return View(teamAssignmentHistories);
        }

        // GET: TeamAssignmentHistories/Create
        public IActionResult Create()
        {
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription");
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription");
            return View();
        }

        // POST: TeamAssignmentHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssignedBy,DateAssigned,ServiceRequest,Team")] TeamAssignmentHistories teamAssignmentHistories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamAssignmentHistories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", teamAssignmentHistories.ServiceRequest);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", teamAssignmentHistories.Team);
            return View(teamAssignmentHistories);
        }

        // GET: TeamAssignmentHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamAssignmentHistories = await _context.TeamAssignmentHistories.SingleOrDefaultAsync(m => m.Id == id);
            if (teamAssignmentHistories == null)
            {
                return NotFound();
            }
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", teamAssignmentHistories.ServiceRequest);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", teamAssignmentHistories.Team);
            return View(teamAssignmentHistories);
        }

        // POST: TeamAssignmentHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssignedBy,DateAssigned,ServiceRequest,Team")] TeamAssignmentHistories teamAssignmentHistories)
        {
            if (id != teamAssignmentHistories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamAssignmentHistories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamAssignmentHistoriesExists(teamAssignmentHistories.Id))
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
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", teamAssignmentHistories.ServiceRequest);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", teamAssignmentHistories.Team);
            return View(teamAssignmentHistories);
        }

        // GET: TeamAssignmentHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamAssignmentHistories = await _context.TeamAssignmentHistories
                .Include(t => t.ServiceRequestNavigation)
                .Include(t => t.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamAssignmentHistories == null)
            {
                return NotFound();
            }

            return View(teamAssignmentHistories);
        }

        // POST: TeamAssignmentHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamAssignmentHistories = await _context.TeamAssignmentHistories.SingleOrDefaultAsync(m => m.Id == id);
            _context.TeamAssignmentHistories.Remove(teamAssignmentHistories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamAssignmentHistoriesExists(int id)
        {
            return _context.TeamAssignmentHistories.Any(e => e.Id == id);
        }
    }
}
