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
    public class RequestTypesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public RequestTypesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        //// GET: RequestTypes
        //public async Task<IActionResult> Index(string SelectedTeam)
        //{
        //    IEnumerable<SelectListItem> teamitems = _context.Teams.Select(c => new SelectListItem
        //    {
        //        //Selected = c.Id == 1,
        //        Value = c.TeamDescription,
        //        Text = c.TeamDescription
        //    });
        //    ViewBag.SelectedTeam = teamitems;

        //    var serveMeHRCoreContext = _context.RequestTypes
        //        .Include(r => r.TeamNavigation)
        //        .Where(r => r.TeamNavigation.TeamDescription == SelectedTeam);

        //    return View(await serveMeHRCoreContext.ToListAsync());
        //}


        // GET: requestTypes
        public ViewResult Index(string SelectedTeam, string sortOrder, string currentFilter, string searchString, int? page)

        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "Id" : "";
            ViewBag.rtdSortParm = sortOrder == "RequestTypeDescription" ? "RequestTypeDescription" : "RequestTypeDescription";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            IEnumerable<SelectListItem> teamitems = _context.Teams.Select(c => new SelectListItem
            {
                //Selected = c.Id == 1,
                Value = c.TeamDescription,
                Text = c.TeamDescription
            });
            ViewBag.SelectedTeam = teamitems;



            IQueryable<RequestTypes> requestTypes = _context.RequestTypes
                .Include(p => p.TeamNavigation)
                .Where(p => p.TeamNavigation.TeamDescription == SelectedTeam)
                ;

            if (!String.IsNullOrEmpty(searchString))
            {
                requestTypes = requestTypes.Where(s => s.Id.ToString().ToLower().Contains(searchString.ToLower())
                || s.RequestTypeDescription != null && s.RequestTypeDescription.ToString().ToLower().Contains(searchString.ToLower())
                || s.LastUpdated != null && s.LastUpdated.Value.ToString("yyyy-MM-dd").ToLower().Contains(searchString.ToLower())

                );

            }

            switch (sortOrder)
            {
                case "Id":
                    requestTypes = requestTypes.OrderBy(s => s.Id);
                    break;

                case "RequestTypeDescription":
                    requestTypes = requestTypes.OrderBy(s => s.RequestTypeDescription);
                    break;



                default:
                    requestTypes = requestTypes.OrderBy(s => s.Id);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var onePageOfRequests = serviceRequests.ToPagedList(pageNumber, pageSize);
            //  ViewBag.OnePageOfRequests = onePageOfRequests;
            //  return View();
            return View(requestTypes.ToPagedList(pageNumber, pageSize));

        }

        // GET: RequestTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestTypes = await _context.RequestTypes
                .Include(r => r.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requestTypes == null)
            {
                return NotFound();
            }

            return View(requestTypes);
        }

        // GET: RequestTypes/Create
        public IActionResult Create()
        {
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription");
            var model = new RequestTypes();
            model.LastUpdated = System.DateTime.Now;
            model.Active = true;

            return View(model);
        }

        // POST: RequestTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestTypeDescription,LastUpdated,Active,Team")] RequestTypes requestTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", requestTypes.Team);
            return View(requestTypes);
        }

        // GET: RequestTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestTypes = await _context.RequestTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (requestTypes == null)
            {
                return NotFound();
            }
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", requestTypes.Team);
            return View(requestTypes);
        }

        // POST: RequestTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestTypeDescription,LastUpdated,Active,Team")] RequestTypes requestTypes)
        {
            if (id != requestTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestTypesExists(requestTypes.Id))
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
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", requestTypes.Team);
            return View(requestTypes);
        }

        // GET: RequestTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestTypes = await _context.RequestTypes
                .Include(r => r.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requestTypes == null)
            {
                return NotFound();
            }

            return View(requestTypes);
        }

        // POST: RequestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestTypes = await _context.RequestTypes.SingleOrDefaultAsync(m => m.Id == id);
            _context.RequestTypes.Remove(requestTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestTypesExists(int id)
        {
            return _context.RequestTypes.Any(e => e.Id == id);
        }
    }
}
