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
    public class ApplicConfsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public ApplicConfsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        // GET: ApplicConfs
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicConfs.ToListAsync());
        }

        // GET: ApplicConfs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicConfs = await _context.ApplicConfs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicConfs == null)
            {
                return NotFound();
            }

            return View(applicConfs);
        }

        // GET: ApplicConfs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicConfs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileSystemUpload,Adactive,EmailConfirmation,ModifiedBy,Modified,AppAdmin,BackAdmin,Ldapconn,Ldappath,ManageHremail,ManageHremailPass,Smtphost,Smtpport,EnableSsl")] ApplicConfs applicConfs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicConfs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicConfs);
        }

        // GET: ApplicConfs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicConfs = await _context.ApplicConfs.SingleOrDefaultAsync(m => m.Id == id);
            if (applicConfs == null)
            {
                return NotFound();
            }
            return View(applicConfs);
        }

        // POST: ApplicConfs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileSystemUpload,Adactive,EmailConfirmation,ModifiedBy,Modified,AppAdmin,BackAdmin,Ldapconn,Ldappath,ManageHremail,ManageHremailPass,Smtphost,Smtpport,EnableSsl")] ApplicConfs applicConfs)
        {
            if (id != applicConfs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicConfs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicConfsExists(applicConfs.Id))
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
            return View(applicConfs);
        }

        // GET: ApplicConfs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicConfs = await _context.ApplicConfs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicConfs == null)
            {
                return NotFound();
            }

            return View(applicConfs);
        }

        // POST: ApplicConfs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicConfs = await _context.ApplicConfs.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicConfs.Remove(applicConfs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicConfsExists(int id)
        {
            return _context.ApplicConfs.Any(e => e.Id == id);
        }
    }
}
