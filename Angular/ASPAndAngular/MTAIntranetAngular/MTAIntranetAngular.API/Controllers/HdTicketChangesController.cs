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
    public class HdTicketChangesController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdTicketChangesController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdTicketChanges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdTicketChange>>> GetHdTicketChanges()
        {
            return await _context.HdTicketChanges.ToListAsync();
        }

        // GET: api/HdTicketChanges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdTicketChange>> GetHdTicketChange(long id)
        {
            // TODO: Return list of commends for ticket ID
            var hdTicketChange = await _context.HdTicketChanges.FindAsync(id);

            if (hdTicketChange == null)
            {
                return NotFound();
            }

            return hdTicketChange;
        }

        // GET: api/HdTicketChanges/ForTicket/5
        [HttpGet("ForTicket/{id}")]
        public List<HdTicketChangeDTO> GetHdTicketChangesForTicket(long id)
        {
            // TODO: Return list of commends for ticket ID
            //var hdTicketChanges = _context.HdTicketChanges.Where(hdtc => hdtc.Comment != null && hdtc.Comment != "" && hdtc.HdTicketId == id).ToList();
            var hdTicketChanges = _context.HdTicketChanges.Where(hdtc => hdtc.HdTicketId == id).Skip(1).ToList();

            List<HdTicketChangeDTO> result = new();

            hdTicketChanges.ForEach(
                hdtc => result.Add(new HdTicketChangeDTO()
                {
                    Id = hdtc.Id,
                    HdTicketId = hdtc.HdTicketId,
                    Timestamp = hdtc.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                    UserId = hdtc.UserId,
                    UserName = _context.Users.FirstOrDefault(u => u.Id == hdtc.UserId) == null ? "KACE" :
                        _context.Users.First(u => u.Id == hdtc.UserId).UserName,
                    OwnersOnly = hdtc.OwnersOnly,
                    Comment = hdtc.Comment,
                    Description = hdtc.Description
                })
                );

            return result;
        }

        // PUT: api/HdTicketChanges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdTicketChange(long id, HdTicketChange hdTicketChange)
        {
            if (id != hdTicketChange.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdTicketChange).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketChangeExists(id))
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

        // POST: api/HdTicketChanges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdTicketChange>> PostHdTicketChange(HdTicketChange hdTicketChange)
        {
            _context.HdTicketChanges.Add(hdTicketChange);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdTicketChange", new { id = hdTicketChange.Id }, hdTicketChange);
        }

        // DELETE: api/HdTicketChanges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdTicketChange(long id)
        {
            var hdTicketChange = await _context.HdTicketChanges.FindAsync(id);
            if (hdTicketChange == null)
            {
                return NotFound();
            }

            _context.HdTicketChanges.Remove(hdTicketChange);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdTicketChangeExists(long id)
        {
            return _context.HdTicketChanges.Any(e => e.Id == id);
        }
    }
}
