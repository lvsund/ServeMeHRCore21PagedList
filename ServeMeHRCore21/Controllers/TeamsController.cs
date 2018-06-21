using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ServeMeHRCore21.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Microsoft.Extensions.FileProviders;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace ServeMeHRCore21.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public TeamsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        //// GET: Teams
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Teams.ToListAsync());
        //}

        // GET: teams
        public ViewResult Index( string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.tdSortParm = sortOrder == "TeamDescription" ? "TeamDescription" : "TeamDescription";
            ViewBag.IdSortParm = sortOrder == "Id" ? "Id_desc" : "Id";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;



            IQueryable<Teams> teams = _context.Teams;


            if (!String.IsNullOrEmpty(searchString))
            {
                teams = teams.Where(s => s.Id.ToString().ToLower().Contains(searchString.ToLower())
                || s.TeamDescription != null && s.TeamDescription.ToLower().Contains(searchString.ToLower())
                || s.TeamEmailAddress != null && s.TeamEmailAddress.ToLower().Contains(searchString.ToLower())
                );

            }

            switch (sortOrder)
            {
                case "Id":
                    teams = teams.OrderBy(s => s.Id);
                    break;

                case "TeamDescription":
                    teams = teams.OrderBy(s => s.TeamDescription);
                    break;



                default:
                    teams = teams.OrderBy(s => s.TeamDescription);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var onePageOfRequests = serviceRequests.ToPagedList(pageNumber, pageSize);
            //  ViewBag.OnePageOfRequests = onePageOfRequests;
            //  return View();
            return View(teams.ToPagedList(pageNumber, pageSize));

        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teams = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teams == null)
            {
                return NotFound();
            }

            return View(teams);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamDescription,TeamEmailAddress")] Teams teams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teams);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teams = await _context.Teams.SingleOrDefaultAsync(m => m.Id == id);
            if (teams == null)
            {
                return NotFound();
            }
            return View(teams);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamDescription,TeamEmailAddress")] Teams teams)
        {
            if (id != teams.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamsExists(teams.Id))
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
            return View(teams);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teams = await _context.Teams
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teams == null)
            {
                return NotFound();
            }

            return View(teams);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teams = await _context.Teams.SingleOrDefaultAsync(m => m.Id == id);
            _context.Teams.Remove(teams);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamsExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
