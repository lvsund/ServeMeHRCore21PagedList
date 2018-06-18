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
    public class TeamMembersController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public TeamMembersController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string SelectedTeam)
        {

            IEnumerable<SelectListItem> teamitems = _context.Teams.Select(c => new SelectListItem
            {
                //Selected = c.Id == 1,
                Value = c.TeamDescription,
                Text = c.TeamDescription
            });
            ViewBag.SelectedTeam = teamitems;

            var serveMeHRCoreContext = _context.TeamMembers
                .Include(t => t.MemberNavigation)
                .Include(t => t.TeamNavigation)
                .Where(t => t.TeamNavigation.TeamDescription == SelectedTeam); 
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.MemberNavigation)
                .Include(t => t.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamMembers == null)
            {
                return NotFound();
            }

            return View(teamMembers);
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail");
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription");
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Member,Team")] TeamMembers teamMembers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamMembers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail", teamMembers.Member);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", teamMembers.Team);
            return View(teamMembers);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers.SingleOrDefaultAsync(m => m.Id == id);
            if (teamMembers == null)
            {
                return NotFound();
            }
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail", teamMembers.Member);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", teamMembers.Team);
            return View(teamMembers);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Member,Team")] TeamMembers teamMembers)
        {
            if (id != teamMembers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamMembers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamMembersExists(teamMembers.Id))
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
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail", teamMembers.Member);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", teamMembers.Team);
            return View(teamMembers);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.MemberNavigation)
                .Include(t => t.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamMembers == null)
            {
                return NotFound();
            }

            return View(teamMembers);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMembers = await _context.TeamMembers.SingleOrDefaultAsync(m => m.Id == id);
            _context.TeamMembers.Remove(teamMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMembersExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
        }
    }
}
