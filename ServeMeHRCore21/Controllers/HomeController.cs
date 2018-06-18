using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServeMeHRCore21.Models;

namespace ServeMeHRCore21.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServeMeHRCoreContext _context;

        public HomeController(ServeMeHRCoreContext context)
        {
            _context = context;
          
        }

        public IActionResult Index()
        {
     

            var tMems = from mb in _context.Members
                    where mb.MemberUserid == User.Identity.Name
                    select mb;
            if (tMems.FirstOrDefault() == null)
            { ViewBag.IsMember = false; }
            else
            { ViewBag.IsMember = true; }
           

            string admin = _context.ApplicConfs.Select(s => s.AppAdmin).FirstOrDefault();
            string badmin = _context.ApplicConfs.Select(s => s.BackAdmin).FirstOrDefault();
            ViewBag.uadmin = admin;
            ViewBag.ubadmin = badmin;

            
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
