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
    public class RequestTypeStepsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public RequestTypeStepsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        //// GET: RequestTypeSteps
        //public async Task<IActionResult> Index(string SelectedRequestType)
        //{
        //    IEnumerable<SelectListItem> requestTypeitems = _context.RequestTypes.Select(c => new SelectListItem
        //    {
        //        //Selected = c.Id == 1,
        //        Value = c.RequestTypeDescription,
        //        Text = c.RequestTypeDescription
        //    });
        //    ViewBag.SelectedRequestType = requestTypeitems;

        //    var serveMeHRCoreContext = _context.RequestTypeSteps
        //        .Include(r => r.RequestTypeNavigation)
        //        .Where(r => r.RequestTypeNavigation.RequestTypeDescription == SelectedRequestType);
        //    return View(await serveMeHRCoreContext.ToListAsync());
        //}

        public ViewResult Index(string SelectedRequestType, string sortOrder, string currentFilter, string searchString, int? page)

        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "Id" : "";
            ViewBag.sdSortParm = sortOrder == "StepDescription" ? "StepDescription" : "StepDescription";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            IEnumerable<SelectListItem> requestTypeitems = _context.RequestTypes.Select(c => new SelectListItem
            {
                //Selected = c.Id == 1,
                Value = c.RequestTypeDescription,
                Text = c.RequestTypeDescription
            });
            ViewBag.SelectedRequestType = requestTypeitems;





            IQueryable<RequestTypeSteps> requestTypeSteps = _context.RequestTypeSteps
                .Include(p => p.RequestTypeNavigation)
                .Where(p => p.RequestTypeNavigation.RequestTypeDescription == SelectedRequestType);
                ;

            if (!String.IsNullOrEmpty(searchString))
            {
                requestTypeSteps = requestTypeSteps.Where(s => s.Id.ToString().ToLower().Contains(searchString.ToLower())
                || s.StepDescription != null && s.StepDescription.ToString().ToLower().Contains(searchString.ToLower())
                || s.LastUpdated != null && s.LastUpdated.Value.ToString("yyyy-MM-dd").ToLower().Contains(searchString.ToLower())

                );

            }

            switch (sortOrder)
            {
                case "Id":
                    requestTypeSteps = requestTypeSteps.OrderBy(s => s.Id);
                    break;

                case "RequestTypeDescription":
                    requestTypeSteps = requestTypeSteps.OrderBy(s => s.StepDescription);
                    break;



                default:
                    requestTypeSteps = requestTypeSteps.OrderBy(s => s.Id);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var onePageOfRequests = serviceRequests.ToPagedList(pageNumber, pageSize);
            //  ViewBag.OnePageOfRequests = onePageOfRequests;
            //  return View();
            return View(requestTypeSteps.ToPagedList(pageNumber, pageSize));

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
