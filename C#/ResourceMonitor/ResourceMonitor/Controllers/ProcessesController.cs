using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.Utility;
using ResourceMonitor.Data;
using ResourceMonitor.Models;

namespace ResourceMonitor.Controllers
{
    public class ProcessesController : Controller
    {
        private readonly MtaresourceMonitoringContext _context;

        public ProcessesController(MtaresourceMonitoringContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Domain Users")]
        [HttpGet]
        public IActionResult CheckSubscription(int id)
        {
            var username = (User.Identity.Name ?? "").ToLower();

            // Remove "mta-flint\" from the beginning if it exists
            if (username.StartsWith("mta-flint\\"))
            {
                username = username.Substring(10);
            }

            string email = username + "@mtaflint.org";

            var resource = _context.Processes.FirstOrDefault(w => w.Id == id); // Adjust for each type
            if (resource == null)
            {
                return Json(new { success = false, subscribed = false });
            }

            var recipientsList = resource.Recipients.Split(',').Select(r => r.Trim()).ToList();

            return Json(new { success = true, subscribed = recipientsList.Contains(email) });
        }

        [Authorize(Roles = "Domain Users")]
        [HttpPost]
        public IActionResult Subscribe(int id)
        {
            var username = (User.Identity.Name ?? "").ToLower();

            if (username.StartsWith("mta-flint\\"))
            {
                username = username.Substring(10);
            }

            string email = username + "@mtaflint.org";

            var resource = _context.Processes.FirstOrDefault(w => w.Id == id); // Change for each type
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found." });
            }

            var recipientsList = resource.Recipients.Split(',').Select(r => r.Trim()).ToList();

            if (recipientsList.Contains(email))
            {
                // Unsubscribe
                // send email to confirm
                EmailConfiguration.SendProcessUnsubscribeSuccess(resource, email);

                recipientsList.Remove(email);
                resource.Recipients = string.Join(",", recipientsList);
                _context.SaveChanges();
                return Json(new { success = true, message = "Unsubscribed successfully." });
            }
            else
            {
                // Subscribe
                // send email to confirm

                EmailConfiguration.SendProcessSubscriptionSuccess(resource, email);

                resource.Recipients += (string.IsNullOrWhiteSpace(resource.Recipients) ? "" : ",") + email;
                _context.SaveChanges();
                return Json(new { success = true, message = "Subscribed successfully." });
            }
        }


        // GET: Processes
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Index()
        {
              return _context.Processes != null ? 
                          View(await _context.Processes.ToListAsync()) :
                          Problem("Entity set 'MtaresourceMonitoringContext.Processes'  is null.");
        }

        // GET: Processes/Details/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Processes == null)
            {
                return NotFound();
            }

            var process = await _context.Processes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (process == null)
            {
                return NotFound();
            }

            return View(process);
        }

        // GET: Processes/Create
        [Authorize(Roles = "ITS Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Processes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerName,ProcessName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Process process)
        {
            if (ModelState.IsValid)
            {
                _context.Add(process);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(process);
        }

        // GET: Processes/Edit/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Processes == null)
            {
                return NotFound();
            }

            var process = await _context.Processes.FindAsync(id);
            if (process == null)
            {
                return NotFound();
            }
            return View(process);
        }

        // POST: Processes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerName,ProcessName,FriendlyName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Process process)
        {
            if (id != process.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(process);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessExists(process.Id))
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
            return View(process);
        }

        // GET: Processes/Delete/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Processes == null)
            {
                return NotFound();
            }

            var process = await _context.Processes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (process == null)
            {
                return NotFound();
            }

            return View(process);
        }

        // POST: Processes/Delete/5
        [Authorize(Roles = "ITS Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Processes == null)
            {
                return Problem("Entity set 'MtaresourceMonitoringContext.Processes'  is null.");
            }
            var process = await _context.Processes.FindAsync(id);
            if (process != null)
            {
                _context.Processes.Remove(process);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessExists(int id)
        {
          return (_context.Processes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
