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
    public class SqltablesController : Controller
    {
        private readonly MtaresourceMonitoringContext _context;

        public SqltablesController(MtaresourceMonitoringContext context)
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

            var resource = _context.Sqltables.FirstOrDefault(w => w.Id == id); // Adjust for each type
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

            var resource = _context.Sqltables.FirstOrDefault(w => w.Id == id); // Change for each type
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found." });
            }

            var recipientsList = resource.Recipients.Split(',').Select(r => r.Trim()).ToList();

            if (recipientsList.Contains(email))
            {
                // Unsubscribe
                EmailConfiguration.SendSQLTableUnsubscribeSuccess(resource, email);
                recipientsList.Remove(email);
                resource.Recipients = string.Join(",", recipientsList);
                _context.SaveChanges();
                return Json(new { success = true, message = "Unsubscribed successfully." });
            }
            else
            {
                // Subscribe
                EmailConfiguration.SendSQLTableSubscriptionSuccess(resource, email);
                resource.Recipients += (string.IsNullOrWhiteSpace(resource.Recipients) ? "" : ",") + email;
                _context.SaveChanges();
                return Json(new { success = true, message = "Subscribed successfully." });
            }
        }


        // GET: Sqltables
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sqltables.ToListAsync());
        }

        // GET: Sqltables/Details/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sqltable = await _context.Sqltables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sqltable == null)
            {
                return NotFound();
            }

            return View(sqltable);
        }

        // GET: Sqltables/Create
        [Authorize(Roles = "ITS Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sqltables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerName,DatabaseName,TableName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Sqltable sqltable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sqltable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sqltable);
        }

        // GET: Sqltables/Edit/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sqltable = await _context.Sqltables.FindAsync(id);
            if (sqltable == null)
            {
                return NotFound();
            }
            return View(sqltable);
        }

        // POST: Sqltables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ITS Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerName,DatabaseName,TableName,FriendlyName,Recipients,PreviousState,CurrentState,LastCheck,LastEmailSent,LastHealthy,EmailFrequency,AcceptableDowntime")] Sqltable sqltable)
        {
            if (id != sqltable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sqltable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SqltableExists(sqltable.Id))
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
            return View(sqltable);
        }

        // GET: Sqltables/Delete/5
        [Authorize(Roles = "ITS Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sqltable = await _context.Sqltables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sqltable == null)
            {
                return NotFound();
            }

            return View(sqltable);
        }

        // POST: Sqltables/Delete/5
        [Authorize(Roles = "ITS Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sqltable = await _context.Sqltables.FindAsync(id);
            if (sqltable != null)
            {
                _context.Sqltables.Remove(sqltable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SqltableExists(int id)
        {
            return _context.Sqltables.Any(e => e.Id == id);
        }
    }
}
