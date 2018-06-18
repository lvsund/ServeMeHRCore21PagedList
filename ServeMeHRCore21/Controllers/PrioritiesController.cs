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
    public class PrioritiesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public PrioritiesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: Priorities
        public async Task<IActionResult> Index(string SelectedTeam)
        {
            IEnumerable<SelectListItem> teamitems = _context.Teams.Select(c => new SelectListItem
            {
                //Selected = c.Id == 1,
                Value = c.TeamDescription,
                Text = c.TeamDescription
            });
            ViewBag.SelectedTeam = teamitems;

            var serveMeHRCoreContext = _context.Priorities
                .Include(p => p.TeamNavigation)
                .Where(p => p.TeamNavigation.TeamDescription == SelectedTeam)
                ;
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: Priorities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priorities = await _context.Priorities
                .Include(p => p.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (priorities == null)
            {
                return NotFound();
            }
            return View(priorities);
        }

        // GET: Priorities/Create
        public IActionResult Create()
        {
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription");
            var model = new Priorities();
            model.LastUpdated = System.DateTime.Now;
            model.Active = true;


            return View(model);
        }

        // POST: Priorities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PriorityDescription,LastUpdated,Active,Team")] Priorities priorities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priorities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", priorities.Team);
            return View(priorities);
        }

        // GET: Priorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priorities = await _context.Priorities.SingleOrDefaultAsync(m => m.Id == id);
            if (priorities == null)
            {
                return NotFound();
            }
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", priorities.Team);
            return View(priorities);
        }

        // POST: Priorities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PriorityDescription,LastUpdated,Active,Team")] Priorities priorities)
        {
            if (id != priorities.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priorities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrioritiesExists(priorities.Id))
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
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", priorities.Team);
            return View(priorities);
        }

        // GET: Priorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priorities = await _context.Priorities
                .Include(p => p.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (priorities == null)
            {
                return NotFound();
            }

            return View(priorities);
        }

        // POST: Priorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priorities = await _context.Priorities.SingleOrDefaultAsync(m => m.Id == id);
            _context.Priorities.Remove(priorities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrioritiesExists(int id)
        {
            return _context.Priorities.Any(e => e.Id == id);
        }
    }
}
