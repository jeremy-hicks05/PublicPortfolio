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
    public class WebsitesController : Controller
    {
        private readonly MtaresourceMonitoringContext _context;

        public WebsitesController(MtaresourceMonitoringContext context)
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

            var resource = _context.Websites.FirstOrDefault(w => w.Id == id); // Adjust for each type
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

            var resource = _context.Websites.FirstOrDefault(w => w.Id == id); // Change for each type
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found." });
            }

            var recipientsList = resource.Recipients.Split(',').Select(r => r.Trim()).ToList();

            if (recipientsList.Contains(email))
            {
                // Unsubscribe
                EmailConfiguration.SendWebsiteUnsubscribeSuccess(resource, email);
                recipientsList.Remove(email);
                resource.Recipients = string.Join(",", recipientsList);
                _context.SaveChanges();
                return Json(new { success = true, message = "Unsubscribed successfully." });
            }
            else
            {
                // Subscribe
                EmailConfiguration.SendWebsiteSubscriptionSuccess(resource, email);
                resource.Recipients += (string.IsNullOrWhiteSpace(resource.Recipients) ? "" : ",") + email;
                _context.SaveChanges();
                return Json(new { success = true, message = "Subscribed successfully." });
            }
        }

        // GET: Websites
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Index()
        {
              return _context.Websites != null ? 
                          View(await _context.Websites.ToListAsync()) :
                          Problem("Entity set 'MtaresourceMonitoringContext.Websites'  is null.");
        }

        // GET: Websites/Details/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Websites == null)
            {
                return NotFound();
            }

            var website = await _context.Websites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }


        // GET: Websites/Create
        [Authorize(Roles = "ITS Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Websites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerName,WebsiteName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Website website)
        {
            if (ModelState.IsValid)
            {
                _context.Add(website);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(website);
        }

        // GET: Websites/Edit/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Websites == null)
            {
                return NotFound();
            }

            var website = await _context.Websites.FindAsync(id);
            if (website == null)
            {
                return NotFound();
            }
            return View(website);
        }

        // POST: Websites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerName,WebsiteName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Website website)
        {
            if (id != website.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(website);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebsiteExists(website.Id))
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
            return View(website);
        }

        // GET: Websites/Delete/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Websites == null)
            {
                return NotFound();
            }

            var website = await _context.Websites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }

        // POST: Websites/Delete/5
        [Authorize(Roles = "ITS Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Websites == null)
            {
                return Problem("Entity set 'MtaresourceMonitoringContext.Websites'  is null.");
            }
            var website = await _context.Websites.FindAsync(id);
            if (website != null)
            {
                _context.Websites.Remove(website);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteExists(int id)
        {
          return (_context.Websites?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
