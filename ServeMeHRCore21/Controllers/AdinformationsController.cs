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
    public class AdinformationsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public AdinformationsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: Adinformations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Adinformations.ToListAsync());
        }

        // GET: Adinformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adinformations = await _context.Adinformations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (adinformations == null)
            {
                return NotFound();
            }

            return View(adinformations);
        }

        // GET: Adinformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adinformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SAmaccountName,DisplayName,Mail,Title,TelephoneNumber,GivenName,Sn,Company,WwWhomePage,Mobile,Cn,Appusername")] Adinformations adinformations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adinformations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adinformations);
        }

        // GET: Adinformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adinformations = await _context.Adinformations.SingleOrDefaultAsync(m => m.Id == id);
            if (adinformations == null)
            {
                return NotFound();
            }
            return View(adinformations);
        }

        // POST: Adinformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SAmaccountName,DisplayName,Mail,Title,TelephoneNumber,GivenName,Sn,Company,WwWhomePage,Mobile,Cn,Appusername")] Adinformations adinformations)
        {
            if (id != adinformations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adinformations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdinformationsExists(adinformations.Id))
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
            return View(adinformations);
        }

        // GET: Adinformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adinformations = await _context.Adinformations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (adinformations == null)
            {
                return NotFound();
            }

            return View(adinformations);
        }

        // POST: Adinformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adinformations = await _context.Adinformations.SingleOrDefaultAsync(m => m.Id == id);
            _context.Adinformations.Remove(adinformations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdinformationsExists(int id)
        {
            return _context.Adinformations.Any(e => e.Id == id);
        }
    }
}
