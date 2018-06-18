using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServeMeHRCore21.Models;

namespace ServeMeHRCore21.Controllers
{
    public class FileDetailsController : Controller
    {
        private readonly ServeMeHRCoreContext _context;
        private IHostingEnvironment _environment;

        public FileDetailsController(ServeMeHRCoreContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: FileDetails
        public async Task<IActionResult> Index()
        {
            var serveMeHRCoreContext = _context.FileDetails.Include(f => f.ServiceRequest);
            return View(await serveMeHRCoreContext.ToListAsync());
        }

        // GET: FileDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetails = await _context.FileDetails
                .Include(f => f.ServiceRequest)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fileDetails == null)
            {
                return NotFound();
            }

            return View(fileDetails);
        }

        // GET: FileDetails/Create
        public IActionResult Create()
        {
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription");
            var model = new FileDetails();
            model.Id = Guid.NewGuid();

            return View(model);
        }

        // POST: FileDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,Extension,ServiceRequestId")] FileDetails fileDetails)
        {
            if (ModelState.IsValid)
            {
                fileDetails.Id = Guid.NewGuid();
                _context.Add(fileDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", fileDetails.ServiceRequestId);
            return View(fileDetails);
        }

        // GET: FileDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetails = await _context.FileDetails.SingleOrDefaultAsync(m => m.Id == id);
            if (fileDetails == null)
            {
                return NotFound();
            }
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", fileDetails.ServiceRequestId);
            return View(fileDetails);
        }

        // POST: FileDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FileName,Extension,ServiceRequestId")] FileDetails fileDetails)
        {
            if (id != fileDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileDetailsExists(fileDetails.Id))
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
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "Id", "RequestDescription", fileDetails.ServiceRequestId);
            return View(fileDetails);
        }

        // GET: FileDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetails = await _context.FileDetails
                .Include(f => f.ServiceRequest)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fileDetails == null)
            {
                return NotFound();
            }

            return View(fileDetails);
        }

        // POST: FileDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fileDetails = await _context.FileDetails.SingleOrDefaultAsync(m => m.Id == id);
            _context.FileDetails.Remove(fileDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileDetailsExists(Guid id)
        {
            return _context.FileDetails.Any(e => e.Id == id);
        }

        public IActionResult UpLoadFiles(string returncontroller)
        {
            TempData["rc"] = returncontroller;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpLoadFiles(FileDetails fileDetails, ICollection<IFormFile> files,int id)
        {
            ////Attached File Processing========================================================
            //ViewBag.referer = Request.Headers["Referer"].ToString();
             string returncontroller = TempData["rc"] as string;
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

                        _context.FileDetails.Add(new FileDetails()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid(),
                            ServiceRequestId = id


                            //    fileDetails.Add(fileDetail);
                            //    serviceRequests.FileDetails = fileDetails;
                            //}
                        });
                        await _context.SaveChangesAsync();
                        ViewBag.Message = " Files Uploaded";
                        return RedirectToAction("Edit", returncontroller, new { id });

                        //return RedirectToAction("Edit", "ServiceRequests", new { id});
                    }

                }

                catch
                {
                    ViewBag.Message = "Upload Failed";
                }
            return View();
        }

        //    //================================================================================


        [HttpGet("download")]
        public async Task<IActionResult> Download(string fileName)
        {
            if (fileName == null)
                return Content("filename not present");
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            var path = uploads + "\\" + fileName;

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            //return File(memory, GetContentType(path), Path.GetFileName(path));
            return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }


        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}