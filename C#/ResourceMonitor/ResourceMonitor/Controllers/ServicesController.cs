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
    public class ServicesController : Controller
    {
        private readonly MtaresourceMonitoringContext _context;

        public ServicesController(MtaresourceMonitoringContext context)
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

            var resource = _context.Services.FirstOrDefault(w => w.Id == id); // Adjust for each type
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

            var resource = _context.Services.FirstOrDefault(w => w.Id == id); // Change for each type
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found." });
            }

            var recipientsList = resource.Recipients.Split(',').Select(r => r.Trim()).ToList();

            if (recipientsList.Contains(email))
            {
                // Unsubscribe
                EmailConfiguration.SendServiceUnsubscribeSuccess(resource, email);
                recipientsList.Remove(email);
                resource.Recipients = string.Join(",", recipientsList);
                _context.SaveChanges();
                return Json(new { success = true, message = "Unsubscribed successfully." });
            }
            else
            {
                // Subscribe
                EmailConfiguration.SendServiceSubscriptionSuccess(resource, email);
                resource.Recipients += (string.IsNullOrWhiteSpace(resource.Recipients) ? "" : ",") + email;
                _context.SaveChanges();
                return Json(new { success = true, message = "Subscribed successfully." });
            }
        }

        // GET: Services
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Index()
        {
              return _context.Services != null ? 
                          View(await _context.Services.ToListAsync()) :
                          Problem("Entity set 'MtaresourceMonitoringContext.Services'  is null.");
        }

        // GET: Services/Details/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        [Authorize(Roles = "ITS Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerName,ServiceName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Edit/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerName,ServiceName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            return View(service);
        }

        // GET: Services/Delete/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [Authorize(Roles = "ITS Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'MtaresourceMonitoringContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
