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
    public class AdinformationsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public AdinformationsController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        //// GET: Adinformations
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Adinformations.ToListAsync());
        //}

        // GET: Adinformations
        public ViewResult Index(string StatusType, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SnSortParm = sortOrder == "Sn" ? "Sn" : "Sn";
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



            IQueryable<Adinformations> adInformations = _context.Adinformations;


            if (!String.IsNullOrEmpty(searchString))
            {
                adInformations = adInformations.Where(s => s.Id.ToString().ToLower().Contains(searchString.ToLower())
                || s.SAmaccountName != null && s.SAmaccountName.ToString().ToLower().Contains(searchString.ToLower())
                || s.DisplayName != null && s.DisplayName.ToLower().Contains(searchString.ToLower())
                || s.Mail != null && s.Mail.ToLower().Contains(searchString.ToLower())
                || s.Title != null && s.Title.ToLower().Contains(searchString.ToLower())
                || s.TelephoneNumber != null && s.TelephoneNumber.ToLower().Contains(searchString.ToLower())
                || s.GivenName != null && s.GivenName.ToLower().Contains(searchString.ToLower())
                || s.Sn != null && s.Sn.ToLower().Contains(searchString.ToLower())
                || s.Company != null && s.Company.ToLower().Contains(searchString.ToLower())
                || s.WwWhomePage != null && s.WwWhomePage.ToLower().Contains(searchString.ToLower())
                || s.Mobile != null && s.Mobile.ToLower().Contains(searchString.ToLower())
                || s.Cn != null && s.Cn.ToLower().Contains(searchString.ToLower())
                || s.Appusername != null && s.Appusername.ToLower().Contains(searchString.ToLower())

                );

            }

            switch (sortOrder)
            {
                case "Id":
                   adInformations = adInformations.OrderBy(s => s.Id);
                    break;

                case "Sn":
                    adInformations = adInformations.OrderBy(s => s.Sn);
                    break;



                default:
                    adInformations = adInformations.OrderBy(s => s.Sn);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var onePageOfRequests = serviceRequests.ToPagedList(pageNumber, pageSize);
            //  ViewBag.OnePageOfRequests = onePageOfRequests;
            //  return View();
            return View(adInformations.ToPagedList(pageNumber, pageSize));

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
