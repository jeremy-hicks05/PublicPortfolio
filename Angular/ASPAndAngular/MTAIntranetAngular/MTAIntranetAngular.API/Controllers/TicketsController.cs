using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;
using MTAIntranetAngular.Utility;

namespace MTAIntranetAngular.API.Controllers
{
    [Authorize(Roles = AccessRoles.ITS + "," +
        AccessRoles.HR)]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly MtaticketsContext _context;

        public TicketsController(MtaticketsContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<ApiResult<Ticket>>> GetTickets(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            return await ApiResult<Ticket>.CreateAsync(
                _context.Tickets
                .AsNoTracking()
                .Include(t => t.Category)
                .Include(t => t.SubType)
                .Include(t => t.SubType!.Category)
                .Include(t => t.ApprovalState)
                .Include(t => t.Impact),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .AsNoTracking()
                .Include(t => t.Category)
                .Include(t => t.SubType)
                .Include(t => t.SubType!.Category)
                .Include(t => t.ApprovalState)
                .Include(t => t.Impact)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            //ticket.DateLastUpdated = DateTime.Now;
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                EmailConfiguration.SendTicketInfoTo(ticket);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // /api/tickets/approve/{id}
        [HttpPut("Approve/{id}")]
        public async Task<ActionResult> Approve(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            ticket.DateLastUpdated = DateTime.Now;
            ticket.ApprovalStateId = _context.ApprovalStates.First(a => a.Name == "Approved").ApprovalStateId;
            ticket.ApprovalState = _context.ApprovalStates.First(a => a.Name == "Approved");
            ticket.ApprovedBy = User.Identity?.Name;
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                EmailConfiguration.SendEmailToKACE(ticket);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // /api/tickets/approve/{id}
        [HttpPut("Reject/{id}")]
        public async Task<ActionResult> Reject(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            ticket.DateLastUpdated = DateTime.Now;
            ticket.ApprovalStateId =
                _context.ApprovalStates
                .First(a => a.Name == "Rejected").ApprovalStateId;
            ticket.ApprovalState = _context.ApprovalStates
                .First(a => a.Name == "Rejected");
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                EmailConfiguration.SendRejectionNotice(ticket);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'MtaticketsContext.Tickets'  is null.");
            }
            ticket.EnteredByUser = User.Identity!.Name!;
            ticket.Impact = _context.Impacts.Find(ticket.ImpactId)!;
            ticket.Category = _context.Categories.Find(ticket.CategoryId)!;
            ticket.ApprovalState = _context.ApprovalStates.Find(ticket.ApprovalStateId)!;
            ticket.SubType = _context.TicketSubTypes.Find(ticket.SubTypeId)!;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            EmailConfiguration.SendTicketInfoTo(ticket);
            if (ticket.ApprovalState?.Name == "Needs Approval")
            {
                EmailConfiguration.SendApprovalRequestToManager(ticket);
            }
            else
            {
                EmailConfiguration.SendEmailToKACE(ticket);
            }
            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }



        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.TicketId == id)).GetValueOrDefault();
        }

        // -- EMAIL -- //
        #region EmailReminders
        [HttpGet]
        [AllowAnonymous]
        [Route("SendEmailReminders")]
        public void SendEmailReminders()
        {
            foreach (Ticket ticket in
                _context.Tickets
                .Include(t => t.SubType)
                .Include(t => t.Impact)
                .Include(t => t.ApprovalState)
                .Include(t => t.Category)
                .Where(t => t.ApprovalState!.Name == "Needs Approval"))
            {
                EmailConfiguration.SendApprovalRequestToManager(ticket);
            }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("SendTicketInfo/{id}")]
        //public void SendTicketInfo(int id)
        //{
        //    Ticket ticket = _context.Tickets
        //        .Include(t => t.SubType)
        //        .Include(t => t.Impact)
        //        .Include(t => t.ApprovalState)
        //        .Include(t => t.Category)
        //        .First(t => t.TicketId == id);
        //    EmailConfiguration.SendTicketInfoTo(ticket);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("SendTicketToKACE/{id}")]
        //public void SendTicketToKACE(int id)
        //{
        //    Ticket ticket = _context.Tickets
        //        .Include(t => t.SubType)
        //        .Include(t => t.Impact)
        //        .Include(t => t.ApprovalState)
        //        .Include(t => t.Category)
        //        .First(t => t.TicketId == id);
        //    EmailConfiguration.SendEmailToKACE(ticket);
        //}
        #endregion
        // -- END EMAIL -- //
    }
}
