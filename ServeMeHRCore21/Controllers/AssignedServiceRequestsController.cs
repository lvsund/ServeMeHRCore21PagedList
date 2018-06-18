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
    public class AssignedServiceRequestsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;
        private IHostingEnvironment _environment;

        public AssignedServiceRequestsController(ServeMeHRCoreContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        //// GET: ServiceRequests
        //public async Task<IActionResult> Index(string StatusType, string searchString)
        //{
        //    IEnumerable<SelectListItem> statusitems = _context.StatusTypes.Select(c => new SelectListItem
        //    {
        //        Selected = c.Id == 1,
        //        Value = c.StatusTypeDescription,
        //        Text = c.StatusTypeDescription
        //    });
        //    ViewBag.StatusType = statusitems;

        //    ViewBag.CurrentFilter = searchString;
        //    if (StatusType == null)
        //        StatusType = "Open";

        //    var serveMeHRCoreContext = _context.ServiceRequests.Include(s => s.MemberNavigation).Include(s => s.PriorityNavigation).Include(s => s.RequestTypeNavigation).Include(s => s.RequestTypeStepNavigation).Include(s => s.StatusNavigation).Include(s => s.TeamNavigation).Where(s => s.StatusNavigation.StatusTypeNavigation.StatusTypeDescription == StatusType);

        //    serveMeHRCoreContext = from sr in _context.ServiceRequests
        //                      join ss in _context.StatusSets on sr.Status equals ss.Id
        //                      join st in _context.StatusTypes on ss.StatusType equals st.Id
        //                      join m in _context.Members on sr.Member equals m.Id
        //                      orderby sr.DateTimeSubmitted descending
        //                      //where t.TeamDescription.Contains("HR Technology Solutions") & st.StatusTypeDescription == StatusType
        //                      where m.MemberUserid.Contains(User.Identity.Name) & st.StatusTypeDescription == StatusType

        //                      select sr;


        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        serveMeHRCoreContext = serveMeHRCoreContext.Where(s => s.RequestHeading.Contains(searchString) || s.RequestDescription.Contains(searchString));
        //    }

        //    return View(await serveMeHRCoreContext.ToListAsync());
        //}

        // GET: ServiceRequests
        public ViewResult Index(string StatusType, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DTSSortParm = String.IsNullOrEmpty(sortOrder) ? "DateTimeSubmitted_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "RequestorLastName" ? "RequestorLastName_desc" : "RequestorLastName";
            ViewBag.RequestHeadingSortParm = sortOrder == "RequestHeading" ? "RequestHeading_desc" : "RequestHeading";
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

            IEnumerable<SelectListItem> statusitems = _context.StatusTypes.Select(c => new SelectListItem
            {
                Selected = c.Id == 1,
                Value = c.StatusTypeDescription,
                Text = c.StatusTypeDescription
            });
            ViewBag.StatusType = statusitems;

            ViewBag.CurrentFilter = searchString;
            if (StatusType == null)
                StatusType = "Open";

            var serviceRequests = _context.ServiceRequests
                .Include(s => s.MemberNavigation)
                .Include(s => s.PriorityNavigation)
                .Include(s => s.RequestTypeNavigation)
                .Include(s => s.RequestTypeStepNavigation)
                .Include(s => s.StatusNavigation)
                .Include(s => s.TeamNavigation)
                .Include(s => s.FileDetails)
            //.OrderByDescending(s => s.DateTimeSubmitted)



           .Where(s => s.StatusNavigation.StatusTypeNavigation.StatusTypeDescription == StatusType & s.MemberNavigation.MemberUserid.Contains(User.Identity.Name));

            if (!String.IsNullOrEmpty(searchString))
            {
                serviceRequests = serviceRequests.Where(s => s.Id.ToString().ToLower().Contains(searchString.ToLower())
                || s.DateTimeSubmitted != null && s.DateTimeSubmitted.Value.ToString("yyyy-MM-dd").ToLower().Contains(searchString.ToLower())
                || s.RequestHeading != null && s.RequestHeading.ToLower().Contains(searchString.ToLower())
                || s.RequestDescription != null && s.RequestDescription.ToLower().Contains(searchString.ToLower())
                || s.RequestorId != null && s.RequestorId.ToLower().Contains(searchString.ToLower())
                || s.RequestorFirstName != null && s.RequestorFirstName.ToLower().Contains(searchString.ToLower())
                || s.RequestorLastName != null && s.RequestorLastName.ToLower().Contains(searchString.ToLower())
                               );

            }

            switch (sortOrder)
            {
                case "Id":
                    serviceRequests = serviceRequests.OrderBy(s => s.Id);
                    break;

                case "DateTimeSubmitted":
                    serviceRequests = serviceRequests.OrderByDescending(s => s.DateTimeSubmitted);
                    break;



                default:
                    serviceRequests = serviceRequests.OrderByDescending(s => s.DateTimeSubmitted);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var onePageOfRequests = serviceRequests.ToPagedList(pageNumber, pageSize);
            //  ViewBag.OnePageOfRequests = onePageOfRequests;
            //  return View();
            return View(serviceRequests.ToPagedList(pageNumber, pageSize));

        }


        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _context.ServiceRequests
                .Include(s => s.MemberNavigation)
                .Include(s => s.PriorityNavigation)
                .Include(s => s.RequestTypeNavigation)
                .Include(s => s.RequestTypeStepNavigation)
                .Include(s => s.StatusNavigation)
                .Include(s => s.TeamNavigation)
                .Include(s => s.FileDetails)
                .Include(s => s.ServiceRequestNotes)
                .Include(s => s.StepHistories)
                .Include(s => s.TeamAssignmentHistories)
                .Include(s => s.IndividualAssignmentHistories)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (serviceRequests == null)
            {
                return NotFound();
            }

            return View(serviceRequests);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail");
            ViewData["Priority"] = new SelectList(_context.Priorities, "Id", "PriorityDescription");
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription");
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription");
            ViewData["Status"] = new SelectList(_context.StatusSets, "Id", "StatusDescription");
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription");

            string homePhone;
            string givenName;
            string surname;
            string email;

            var model = new ServiceRequests();
            model.RequestorId = User.Identity.Name;

            ViewBag.FileUp = _context.ApplicConfs.Select(s => s.FileSystemUpload).FirstOrDefault();

            Boolean ADconf = _context.ApplicConfs.Select(s => s.Adactive).FirstOrDefault();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) & (ADconf))
            {
                GetADinfo(out givenName, out surname, out homePhone, out email);

                model.RequestorFirstName = givenName;
                model.RequestorLastName = surname;
                model.RequestorEmail = email;
                model.RequestorPhone = homePhone;
                model.RequestType = 1;
            }

            return View(model);
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestHeading,RequestDescription,RequestorId,RequestorFirstName,RequestorLastName,RequestorPhone,RequestorEmail,DateTimeSubmitted,DateTimeStarted,DateTimeCompleted,Priority,RequestType,RequestTypeStep,Member,Status,Team")] ServiceRequests serviceRequests, ICollection<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                string homePhone;
                string givenName;
                string surname;
                string email;

                Boolean ADconf = _context.ApplicConfs.Select(s => s.Adactive).FirstOrDefault();

                if (ADconf)
                {
                    GetADinfo(out givenName, out surname, out homePhone, out email);
                    ViewBag.RequestorFirstName = givenName;
                    ViewBag.RequestorLastName = surname;
                    ViewBag.RequestorPhone = homePhone;
                    ViewBag.RequestorEmail = email;
                }




                _context.ServiceRequests.Include(i => i.IndividualAssignmentHistories).Include(i => i.TeamAssignmentHistories).Include(i => i.FileDetails);
                _context.ServiceRequests.Add(serviceRequests);
                serviceRequests.DateTimeSubmitted = DateTime.Now;
                serviceRequests.Status = 1;
                serviceRequests.RequestorId = User.Identity.Name;

                //================================================================================
                //Create a history record for team assignment

                serviceRequests.TeamAssignmentHistories.Add(new TeamAssignmentHistories()
                {
                    DateAssigned = DateTime.Now,
                    AssignedBy = User.Identity.Name,
                    ServiceRequest = serviceRequests.Id,
                    Team = serviceRequests.Team
                });
                //================================================================================

                //================================================================================
                //Create history record for individual assignment
                serviceRequests.IndividualAssignmentHistories.Add(new IndividualAssignmentHistories()
                {
                    DateAssigned = DateTime.Now,
                    AssignedBy = User.Identity.Name,
                    //AssignedTo = "A3HR.Lyndon",
                    AssignedTo = serviceRequests.Member.Value,
                    ServiceRequest = serviceRequests.Id
                });
                //================================================================================

                //Create request step history record===============================================
                serviceRequests.StepHistories.Add(new StepHistories()
                {
                    LastUpdated = DateTime.Now,
                    RequestTypeStep = serviceRequests.RequestTypeStep.Value,
                    ServiceRequest = serviceRequests.Id
                });

                //=================================================================================




                _context.Add(serviceRequests);
                await _context.SaveChangesAsync();

                //Check and see if in application configuration email confirmation is set on========
                Boolean emailconf = _context.ApplicConfs.Select(s => s.EmailConfirmation).FirstOrDefault();

                if (emailconf)
                {
                    SendStatusConfirmation(1, serviceRequests.RequestorEmail);
                }


                return RedirectToAction(nameof(Index));
            }
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail", serviceRequests.Member);
            ViewData["Priority"] = new SelectList(_context.Priorities, "Id", "PriorityDescription", serviceRequests.Priority);
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription", serviceRequests.RequestType);
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription", serviceRequests.RequestTypeStep);
            ViewData["Status"] = new SelectList(_context.StatusSets, "Id", "StatusDescription", serviceRequests.Status);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", serviceRequests.Team);
            return View(serviceRequests);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var serviceRequests = await _context.ServiceRequests.SingleOrDefaultAsync(m => m.Id == id);
            var serviceRequests = await _context.ServiceRequests
                 .Include(s => s.FileDetails)
               //   .AsNoTracking()
                 .SingleOrDefaultAsync(m => m.Id == id);
            if (serviceRequests == null)
            {
                return NotFound();
            }
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail", serviceRequests.Member);
            ViewData["Priority"] = new SelectList(_context.Priorities, "Id", "PriorityDescription", serviceRequests.Priority);
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription", serviceRequests.RequestType);
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription", serviceRequests.RequestTypeStep);
            ViewData["Status"] = new SelectList(_context.StatusSets, "Id", "StatusDescription", serviceRequests.Status);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", serviceRequests.Team);
            return View(serviceRequests);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestHeading,RequestDescription,RequestorId,RequestorFirstName,RequestorLastName,RequestorPhone,RequestorEmail,DateTimeSubmitted,DateTimeStarted,DateTimeCompleted,Priority,RequestType,RequestTypeStep,Member,Status,Team")] ServiceRequests serviceRequests)
        {
            if (id != serviceRequests.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // If Status changes then populate the datetime started and completed fields and send email confirmation=========

                    if (serviceRequests.Status == 2 && serviceRequests.DateTimeStarted == null)
                    {
                        serviceRequests.DateTimeStarted = DateTime.Now;
                        SendStatusConfirmation(2, serviceRequests.RequestorEmail);
                        _context.Entry(serviceRequests).State = EntityState.Modified;
                    }

                    if (serviceRequests.Status == 3 && serviceRequests.DateTimeCompleted == null)
                    {
                        serviceRequests.DateTimeCompleted = DateTime.Now;
                        SendStatusConfirmation(3, serviceRequests.RequestorEmail);
                        _context.Entry(serviceRequests).State = EntityState.Modified;

                    }


                    ////===================================================================================



                    // If team is modified create new team history record==========================

                    //var lastTeam = _context.TeamAssignmentHistories
                    //    .Include(t => t.ServiceRequestNavigation)
                    //    .Include(t => t.TeamNavigation)
                    //    .Where(t => t.ServiceRequestNavigation.Id == serviceRequests.Id);
                    var lastTeam = from tah in _context.TeamAssignmentHistories
                                   where tah.ServiceRequest == serviceRequests.Id
                                   orderby tah.DateAssigned descending
                                   select tah;
                    int lteam;
                    lteam = lastTeam.FirstOrDefault().Team;

                    if (serviceRequests.Team != lteam)

                    {
                        serviceRequests.TeamAssignmentHistories.Add(new TeamAssignmentHistories()
                        {
                            DateAssigned = DateTime.Now,
                            AssignedBy = User.Identity.Name,
                            ServiceRequest = serviceRequests.Id,
                            Team = serviceRequests.Team
                        });

                    }

                    //=========================================================================================



                    //=========================================================================================

                    // if individual assigned has changed add individual history record========================
                    //var lastMember = _context.IndividualAssignmentHistories
                    //    .Include(t => t.ServiceRequestNavigation)
                    //    .Where(t => t.ServiceRequestNavigation.Id == serviceRequests.Id)
                    //    ;
                    var lastMember = from lm in _context.IndividualAssignmentHistories
                                     where lm.ServiceRequest == serviceRequests.Id
                                     orderby lm.DateAssigned descending
                                     select lm;
                    int lmember;
                    lmember = lastMember.FirstOrDefault().AssignedTo;

                    if (serviceRequests.Member != lmember)
                    {
                        serviceRequests.IndividualAssignmentHistories.Add(new IndividualAssignmentHistories()
                        {
                            DateAssigned = DateTime.Now,
                            AssignedBy = User.Identity.Name,
                            ServiceRequest = serviceRequests.Id,
                            AssignedTo = serviceRequests.Member.Value,
                        });
                    }


                    //===========================================================================================


                    //===== If request step changes then  create request step history record=====================

                    //var lastStep = _context.StepHistories
                    //    .Include(t => t.ServiceRequestNavigation)
                    //    .Where(t => t.ServiceRequestNavigation.Id == serviceRequests.Id)
                    //    ;
                   var lastStep = from ls in _context.StepHistories
                               where ls.ServiceRequest == serviceRequests.Id
                               orderby ls.LastUpdated descending
                               select ls;
                    int lstep;
                    lstep = lastStep.FirstOrDefault().RequestTypeStep;

                    if (serviceRequests.RequestTypeStep != lstep)
                    {
                        serviceRequests.StepHistories.Add(new StepHistories()
                        {
                            LastUpdated = DateTime.Now,
                            RequestTypeStep = serviceRequests.RequestTypeStep.Value,
                            ServiceRequest = serviceRequests.Id
                        });
                    }

                    //===========================================================================================



                    _context.Update(serviceRequests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestsExists(serviceRequests.Id))
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
            ViewData["Member"] = new SelectList(_context.Members, "Id", "MemberEmail", serviceRequests.Member);
            ViewData["Priority"] = new SelectList(_context.Priorities, "Id", "PriorityDescription", serviceRequests.Priority);
            ViewData["RequestType"] = new SelectList(_context.RequestTypes, "Id", "RequestTypeDescription", serviceRequests.RequestType);
            ViewData["RequestTypeStep"] = new SelectList(_context.RequestTypeSteps, "Id", "StepDescription", serviceRequests.RequestTypeStep);
            ViewData["Status"] = new SelectList(_context.StatusSets, "Id", "StatusDescription", serviceRequests.Status);
            ViewData["Team"] = new SelectList(_context.Teams, "Id", "TeamDescription", serviceRequests.Team);
            return View(serviceRequests);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _context.ServiceRequests
                .Include(s => s.MemberNavigation)
                .Include(s => s.PriorityNavigation)
                .Include(s => s.RequestTypeNavigation)
                .Include(s => s.RequestTypeStepNavigation)
                .Include(s => s.StatusNavigation)
                .Include(s => s.TeamNavigation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (serviceRequests == null)
            {
                return NotFound();
            }

            return View(serviceRequests);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequests = await _context.ServiceRequests.SingleOrDefaultAsync(m => m.Id == id);
            _context.ServiceRequests.Remove(serviceRequests);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestsExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.Id == id);
        }

        public void GetADinfo(out string givenName, out string surname, out string homePhone, out string email)
        {
            //===========================================================
            //Go and get AD info for the current user or equivalent
            var components = User.Identity.Name.Split('\\');
            var username = components.Last();
            // create LDAP connection object
            DirectoryEntry myLdapConnection = createDirectoryEntry();
            DirectorySearcher search = new DirectorySearcher(myLdapConnection);

            search.Filter = "(cn=" + username + ")";
            SearchResult result = search.FindOne();
            DirectoryEntry dsresult = result.GetDirectoryEntry();
            givenName = dsresult.Properties["givenName"][0].ToString();
            surname = dsresult.Properties["sn"][0].ToString();
            email = dsresult.Properties["mail"][0].ToString();
            homePhone = dsresult.Properties["homePhone"][0].ToString();

            //=============================================================================
        }

        public DirectoryEntry createDirectoryEntry()
        {
            // create and return new LDAP connection with desired settings

            string ADconn = _context.ApplicConfs.Select(s => s.Ldapconn).FirstOrDefault();
            string LDAPConn = _context.ApplicConfs.Select(s => s.Ldappath).FirstOrDefault();

            //string ADconn;
            //ADconn = "SERVER.A3HR.local";
            //string LDAPConn;
            //LDAPConn = "LDAP://SERVER.A3HR.local";
            //DirectoryEntry ldapConnection = new DirectoryEntry("SERVER.A3HR.local");

            //ldapConnection.Path = "LDAP://OU=staffusers,DC=leeds-art,DC=ac,DC=uk";
            //ldapConnection.Path = "LDAP://SERVER.A3HR.local";

            DirectoryEntry ldapConnection = new DirectoryEntry(ADconn);

            ldapConnection.Path = LDAPConn;

            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

            return ldapConnection;
        }



        public void SendStatusConfirmation(int statusstep, string email)
        {
            var message = new MimeMessage();

            string mailFrom = _context.ApplicConfs.Select(s => s.ManageHremail).FirstOrDefault();
            string fromPass = _context.ApplicConfs.Select(s => s.ManageHremailPass).FirstOrDefault();
            string smtpHost = _context.ApplicConfs.Select(s => s.Smtphost).FirstOrDefault();
            int smtpPort = _context.ApplicConfs.Select(s => s.Smtpport).FirstOrDefault().Value;
            Boolean enabSSL = _context.ApplicConfs.Select(s => s.EnableSsl).FirstOrDefault().Value;


            message.To.Add(new MailboxAddress(email));
            message.From.Add(new MailboxAddress(mailFrom));
            switch (statusstep)
            {
                case 1:
                    message.Subject = "Request received";
                    message.Body = new TextPart("plain") { Text = @"Your request has been received" };
                    break;

                case 2:
                    message.Subject = "Request started";
                    message.Body = new TextPart("plain") { Text = @"Your request has been started" };
                    break;

                case 3:
                    message.Subject = "Request completed";
                    message.Body = new TextPart("plain") { Text = @"Your request has been completed" };
                    break;
            }
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;



                    client.Connect(smtpHost, smtpPort, false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(mailFrom, fromPass);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }

        }

        public IActionResult UpLoadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpLoadFiles(ICollection<IFormFile> files,ServiceRequests serviceRequests,int id)


        {


            ////Attached File Processing========================================================

            string path = null;

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
                try
                {
                    if (file == null || file.Length == 0)

                        return Content("file not selected");
                    else
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        var fileName = Path.GetFileName(file.FileName);
                        path = uploads + "\\" + fileName;

                        //_context.FileDetails.Add(new FileDetails()
                        //{
                        //    FileName = fileName,
                        //    Extension = Path.GetExtension(fileName),
                        //    Id = Guid.NewGuid(),
                        //    ServiceRequestId = serviceRequests.Id


                        //    //    fileDetails.Add(fileDetail);
                        //    //    serviceRequests.FileDetails = fileDetails;
                        //    //}
                        //});

                        serviceRequests.FileDetails.Add(new FileDetails()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid(),
                            ServiceRequestId = id
                        }


                            );

                        // _context.Update(serviceRequests);
                        //_context.Entry(serviceRequests.FileDetails).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        ViewBag.Message = " Files Uploaded";
                        return RedirectToAction("Index");

                    }

                }

                catch
                {
                    ViewBag.Message = "Upload Failed";
                }

            return View();
        }

        //    //================================================================================

        public JsonResult GetRequestTypeByTeam(int id)
        {
            List<RequestTypes> requestTypes = new List<RequestTypes>();
            if (id > 0)
            {
                requestTypes = _context.RequestTypes.Where(p => p.Team == id).ToList();
            }
            else
            {
                requestTypes.Insert(0, new RequestTypes { Id = 0, RequestTypeDescription = "--Select a Team first--" });
            }
            var result = (from r in requestTypes
                          select new
                          {
                              id = r.Id,
                              name = r.RequestTypeDescription
                          }).ToList();

            return Json(result);
        }

        public JsonResult GetPrioritiesByTeam(int id)
        {
            List<Priorities> priorities = new List<Priorities>();
            if (id > 0)
            {
                priorities = _context.Priorities.Where(p => p.Team == id).ToList();
            }
            else
            {
                priorities.Insert(0, new Priorities { Id = 0, PriorityDescription = "--Select a Team first--" });
            }
            var result = (from r in priorities
                          select new
                          {
                              id = r.Id,
                              name = r.PriorityDescription
                              //}).ToList().OrderBy(x=> x.name);
                          }).ToList();

            return Json(result);
        }

        public JsonResult GetMembersByTeam(int id)
        {
            List<Members> members = new List<Members>();

            if (id > 0)
            {
                //members = db.Teams.Where(p => p.Id == id)
                //    .SelectMany(e => e.TeamMembers)
                //    .Select(e => e.Member)
                //    .ToList();

                members = _context.TeamMembers.Where(p => p.Team == id)
    .Select(e => e.MemberNavigation)
    .ToList();
            }
            else
            {
                members.Insert(0, new Members { Id = 0, MemberFullName = "--Select a Team first--" });
            }
            var result = (from m in members

                          select new
                          {
                              id = m.Id,
                              name = m.MemberFullName
                          }).ToList();

            return Json(result);
        }

        public JsonResult GetRequestTypeStepsByRequestType(int id)
        {
            List<RequestTypeSteps> requestTypeSteps = new List<RequestTypeSteps>();
            if (id > 0)
            {
                requestTypeSteps = _context.RequestTypeSteps.Where(p => p.RequestType == id).ToList();
            }
            else
            {
                requestTypeSteps.Insert(0, new RequestTypeSteps { Id = 0, StepDescription = "--Select a Request Type first--" });
            }
            var result = (from r in requestTypeSteps
                          select new
                          {
                              id = r.Id,
                              name = r.StepDescription
                              //}).ToList().OrderBy(x=> x.name);
                          }).ToList();

            return Json(result);
        }


        
    }
}
