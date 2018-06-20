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
    public class MembersController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public MembersController(ServeMeHRCoreContext context)
        {
            _context = context;
        }

        //// GET: Members
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Members.ToListAsync());
        //}

        // GET: Members
        public ViewResult Index(string StatusType, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.mlnSortParm = sortOrder == "MemberLastName" ? "MemberLastName" : "MemberLastName";
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



            IQueryable<Members> members = _context.Members;


            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(s => s.Id.ToString().ToLower().Contains(searchString.ToLower())
                || s.MemberUserid != null && s.MemberUserid.ToString().ToLower().Contains(searchString.ToLower())
                || s.MemberFirstName != null && s.MemberFirstName.ToLower().Contains(searchString.ToLower())
                || s.MemberLastName != null && s.MemberLastName.ToLower().Contains(searchString.ToLower())
                || s.MemberFullName != null && s.MemberFullName.ToLower().Contains(searchString.ToLower())
                || s.MemberEmail != null && s.MemberEmail.ToLower().Contains(searchString.ToLower())
                || s.MemberPhone != null && s.MemberPhone.ToLower().Contains(searchString.ToLower())

                );

            }

            switch (sortOrder)
            {
                case "Id":
                    members = members.OrderBy(s => s.Id);
                    break;

                case "MemberLastName":
                    members = members.OrderBy(s => s.MemberLastName);
                    break;



                default:
                    members = members.OrderBy(s => s.MemberLastName);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var onePageOfRequests = serviceRequests.ToPagedList(pageNumber, pageSize);
            //  ViewBag.OnePageOfRequests = onePageOfRequests;
            //  return View();
            return View(members.ToPagedList(pageNumber, pageSize));

        }


        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var members = await _context.Members
                .SingleOrDefaultAsync(m => m.Id == id);
            if (members == null)
            {
                return NotFound();
            }

            return View(members);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberUserid,MemberFirstName,MemberLastName,MemberFullName,MemberEmail,MemberPhone")] Members members)
        {
            if (ModelState.IsValid)
            {
                _context.Add(members);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(members);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var members = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            if (members == null)
            {
                return NotFound();
            }
            return View(members);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberUserid,MemberFirstName,MemberLastName,MemberFullName,MemberEmail,MemberPhone")] Members members)
        {
            if (id != members.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(members);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembersExists(members.Id))
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
            return View(members);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var members = await _context.Members
                .SingleOrDefaultAsync(m => m.Id == id);
            if (members == null)
            {
                return NotFound();
            }

            return View(members);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var members = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            _context.Members.Remove(members);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembersExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
