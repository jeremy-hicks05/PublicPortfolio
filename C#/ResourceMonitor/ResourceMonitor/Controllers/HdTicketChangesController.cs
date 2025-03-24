using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResourceMonitor.Data;
using ResourceMonitor.Models.KACE;

namespace ResourceMonitor.Controllers
{
    public class HdTicketChangesController : Controller
    {
        private readonly Org1Context _context;

        public HdTicketChangesController(Org1Context context)
        {
            _context = context;
        }

        // GET: HdTicketChanges
        public async Task<IActionResult> Index()
        {
            return View(await _context.HdTicketChanges.ToListAsync());
        }

        // GET: HdTicketChanges/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hdTicketChange = await _context.HdTicketChanges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hdTicketChange == null)
            {
                return NotFound();
            }

            return View(hdTicketChange);
        }

        // GET: HdTicketChanges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HdTicketChanges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HdTicketId,Timestamp,UserId,Comment,CommentLoc,Description,OwnersOnlyDescription,LocalizedDescription,LocalizedOwnersOnlyDescription,Mailed,MailedTimestamp,MailerSession,NotifyUsers,ViaEmail,OwnersOnly,ResolutionChanged,SystemComment,TicketDataChange,ViaScheduledProcess,ViaImport,ViaBulkUpdate")] HdTicketChange hdTicketChange)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hdTicketChange);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hdTicketChange);
        }

        // GET: HdTicketChanges/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hdTicketChange = await _context.HdTicketChanges.FindAsync(id);
            if (hdTicketChange == null)
            {
                return NotFound();
            }
            return View(hdTicketChange);
        }

        // POST: HdTicketChanges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,HdTicketId,Timestamp,UserId,Comment,CommentLoc,Description,OwnersOnlyDescription,LocalizedDescription,LocalizedOwnersOnlyDescription,Mailed,MailedTimestamp,MailerSession,NotifyUsers,ViaEmail,OwnersOnly,ResolutionChanged,SystemComment,TicketDataChange,ViaScheduledProcess,ViaImport,ViaBulkUpdate")] HdTicketChange hdTicketChange)
        {
            if (id != hdTicketChange.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hdTicketChange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HdTicketChangeExists(hdTicketChange.Id))
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
            return View(hdTicketChange);
        }

        // GET: HdTicketChanges/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hdTicketChange = await _context.HdTicketChanges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hdTicketChange == null)
            {
                return NotFound();
            }

            return View(hdTicketChange);
        }

        // POST: HdTicketChanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hdTicketChange = await _context.HdTicketChanges.FindAsync(id);
            if (hdTicketChange != null)
            {
                _context.HdTicketChanges.Remove(hdTicketChange);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HdTicketChangeExists(long id)
        {
            return _context.HdTicketChanges.Any(e => e.Id == id);
        }
    }
}
