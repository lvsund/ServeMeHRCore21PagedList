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
    public class ServiceRequestNotesController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public ServiceRequestNotesController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestNotes
        public async Task<IActionResult> Index()
        {
            var serveMeHRCoreContext = _context.ServiceRequestNotes.Include(s => s.ServiceRequestNavigation);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: ServiceRequestNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestNotes = await _context.ServiceRequestNotes
                .Include(s => s.ServiceRequestNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (serviceRequestNotes == null)
            {
                return NotFound();
            }

            return View(serviceRequestNotes);
        }

        // GET: ServiceRequestNotes/Create
        public IActionResult Create(int ServiceRequest,string returncontroller)
        {
            TempData["rc"] = returncontroller;
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription");


            return View();
        }

        // POST: ServiceRequestNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoteDescription,LastUpdated,WrittenBy,ServiceRequest")] ServiceRequestNotes serviceRequestNotes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestNotes);

                serviceRequestNotes.LastUpdated = DateTime.Now;
                serviceRequestNotes.WrittenBy = User.Identity.Name;


                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                string returncontroller = TempData["rc"] as string;
                return RedirectToAction("Index", returncontroller);
            }
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", serviceRequestNotes.ServiceRequest);
            return View(serviceRequestNotes);
        }

        // GET: ServiceRequestNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestNotes = await _context.ServiceRequestNotes.SingleOrDefaultAsync(m => m.Id == id);
            if (serviceRequestNotes == null)
            {
                return NotFound();
            }
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", serviceRequestNotes.ServiceRequest);
            return View(serviceRequestNotes);
        }

        // POST: ServiceRequestNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoteDescription,LastUpdated,WrittenBy,ServiceRequest")] ServiceRequestNotes serviceRequestNotes)
        {
            if (id != serviceRequestNotes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequestNotes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestNotesExists(serviceRequestNotes.Id))
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
            ViewData["ServiceRequest"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", serviceRequestNotes.ServiceRequest);
            return View(serviceRequestNotes);
        }

        // GET: ServiceRequestNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestNotes = await _context.ServiceRequestNotes
                .Include(s => s.ServiceRequestNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (serviceRequestNotes == null)
            {
                return NotFound();
            }

            return View(serviceRequestNotes);
        }

        // POST: ServiceRequestNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestNotes = await _context.ServiceRequestNotes.SingleOrDefaultAsync(m => m.Id == id);
            _context.ServiceRequestNotes.Remove(serviceRequestNotes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestNotesExists(int id)
        {
            return _context.ServiceRequestNotes.Any(e => e.Id == id);
        }
    }
}
