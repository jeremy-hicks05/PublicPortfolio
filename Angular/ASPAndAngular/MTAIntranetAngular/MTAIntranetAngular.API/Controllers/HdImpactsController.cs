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
    public class HdImpactsController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdImpactsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdImpacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdImpact>>> GetHdImpacts()
        {
            return await _context.HdImpacts.ToListAsync();
        }

        [HttpGet("FromQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdImpact>>> GetHdImpactsFromQueue(int queueId)
        {
            var hdImpacts = await _context.HdImpacts.Where(hdc => hdc.HdQueueId == queueId).ToListAsync();

            if (hdImpacts == null)
            {
                return NotFound();
            }

            return hdImpacts;
        }

        // GET: api/HdImpacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdImpact>> GetHdImpact(long id)
        {
            var hdImpact = await _context.HdImpacts.FindAsync(id);

            if (hdImpact == null)
            {
                return NotFound();
            }

            return hdImpact;
        }

        // PUT: api/HdImpacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdImpact(long id, HdImpact hdImpact)
        {
            if (id != hdImpact.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdImpact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdImpactExists(id))
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

        // POST: api/HdImpacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdImpact>> PostHdImpact(HdImpact hdImpact)
        {
            _context.HdImpacts.Add(hdImpact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdImpact", new { id = hdImpact.Id }, hdImpact);
        }

        // DELETE: api/HdImpacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdImpact(long id)
        {
            var hdImpact = await _context.HdImpacts.FindAsync(id);
            if (hdImpact == null)
            {
                return NotFound();
            }

            _context.HdImpacts.Remove(hdImpact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdImpactExists(long id)
        {
            return _context.HdImpacts.Any(e => e.Id == id);
        }
    }
}
