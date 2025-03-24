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
    public class ServersController : Controller
    {
        private readonly MtaresourceMonitoringContext _context;

        public ServersController(MtaresourceMonitoringContext context)
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

            var resource = _context.Servers.FirstOrDefault(w => w.Id == id); // Adjust for each type
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

            var resource = _context.Servers.FirstOrDefault(w => w.Id == id); // Change for each type
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found." });
            }

            var recipientsList = resource.Recipients.Split(',').Select(r => r.Trim()).ToList();

            if (recipientsList.Contains(email))
            {
                // Unsubscribe
                EmailConfiguration.SendServerUnsubscribeSuccess(resource, email);
                recipientsList.Remove(email);
                resource.Recipients = string.Join(",", recipientsList);
                _context.SaveChanges();
                return Json(new { success = true, message = "Unsubscribed successfully." });
            }
            else
            {
                // Subscribe
                EmailConfiguration.SendServerSubscriptionSuccess(resource, email);
                resource.Recipients += (string.IsNullOrWhiteSpace(resource.Recipients) ? "" : ",") + email;
                _context.SaveChanges();
                return Json(new { success = true, message = "Subscribed successfully." });
            }
        }


        // GET: Servers
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Index()
        {
              return _context.Servers != null ? 
                          View(await _context.Servers.ToListAsync()) :
                          Problem("Entity set 'MtaresourceMonitoringContext.Servers'  is null.");
        }

        // GET: Servers/Details/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // GET: Servers/Create
        [Authorize(Roles = "ITS Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Server server)
        {
            if (ModelState.IsValid)
            {
                _context.Add(server);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(server);
        }

        // GET: Servers/Edit/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Server server)
        {
            if (id != server.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(server);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.Id))
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
            return View(server);
        }

        // GET: Servers/Delete/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // POST: Servers/Delete/5
        [Authorize(Roles = "ITS Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Servers == null)
            {
                return Problem("Entity set 'MtaresourceMonitoringContext.Servers'  is null.");
            }
            var server = await _context.Servers.FindAsync(id);
            if (server != null)
            {
                _context.Servers.Remove(server);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerExists(int id)
        {
          return (_context.Servers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
