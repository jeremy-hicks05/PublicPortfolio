using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data;
using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HdTicketsController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdTicketsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdTicket>>> GetHdTickets()
        {
            return await _context.HdTickets.ToListAsync();
        }

        // GET: api/HdTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdTicket>> GetHdTicket(long id)
        {
            var hdTicket = await _context.HdTickets.FindAsync(id);

            if (hdTicket == null)
            {
                return NotFound();
            }

            return hdTicket;
        }

        // PUT: api/HdTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdTicket(long id, HdTicket hdTicket)
        {
            if (id != hdTicket.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdTicket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketExists(id))
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

        // POST: api/HdTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdTicket>> PostHdTicket(HdTicket hdTicket)
        {
            _context.HdTickets.Add(hdTicket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdTicket", new { id = hdTicket.Id }, hdTicket);
        }

        // DELETE: api/HdTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdTicket(long id)
        {
            var hdTicket = await _context.HdTickets.FindAsync(id);
            if (hdTicket == null)
            {
                return NotFound();
            }

            _context.HdTickets.Remove(hdTicket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdTicketExists(long id)
        {
            return _context.HdTickets.Any(e => e.Id == id);
        }
    }
}
