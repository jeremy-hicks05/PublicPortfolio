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
    public class HdPrioritiesController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdPrioritiesController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdPriorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdPriority>>> GetHdPriorities()
        {
            return await _context.HdPriorities.ToListAsync();
        }

        // GET: api/HdPriorities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdPriority>> GetHdPriority(long id)
        {
            var hdPriority = await _context.HdPriorities.FindAsync(id);

            if (hdPriority == null)
            {
                return NotFound();
            }

            return hdPriority;
        }

        [HttpGet("FromQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdPriority>>> GetHdPrioritiesFromQueue(int queueId)
        {
            var hdPriorities = await _context.HdPriorities
                .Where(hdc => hdc.HdQueueId == queueId).ToListAsync();

            if (hdPriorities == null)
            {
                return NotFound();
            }

            return hdPriorities;
        }

        [HttpGet("GetPriority/{queueId}/{impactId}/")]
        public long GetHdPriority(int queueId, int impactId)
        {

            HdImpact hdImpact = _context.HdImpacts.First(i => i.Id == impactId);

            if (hdImpact.Name.Contains("people cannot work"))
            {
                return _context.HdPriorities
                        .First(hdc =>
                            hdc.HdQueueId == queueId &&
                            hdc.Name.Contains("High")).Id;
            }
            else if (hdImpact.Name.Contains("person cannot work") ||
                hdImpact.Name.Contains("people inconvenienced"))
            {
                return _context.HdPriorities
                        .First(hdc =>
                            hdc.HdQueueId == queueId &&
                            hdc.Name.Contains("Medium")).Id;
            }
            else
            {
                return _context.HdPriorities
                        .First(hdc =>
                            hdc.HdQueueId == queueId &&
                            hdc.Name.Contains("Low")).Id;
            }
        }

        //[HttpGet("FromImpact/{impactId}")]
        //public async Task<ActionResult<HdPriority>> GetHdPriorityFromImpact(int impactId)
        //{
        //    var hdPriority = _context.HdPriorities.FirstOrDefault(hdp => hdp.Name == "High");

        //    if (hdPriority == null)
        //    {
        //        return NotFound();
        //    }

        //    return hdPriority;
        //}

        // PUT: api/HdPriorities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdPriority(long id, HdPriority hdPriority)
        {
            if (id != hdPriority.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdPriority).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdPriorityExists(id))
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

        // POST: api/HdPriorities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdPriority>> PostHdPriority(HdPriority hdPriority)
        {
            _context.HdPriorities.Add(hdPriority);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdPriority", new { id = hdPriority.Id }, hdPriority);
        }

        // DELETE: api/HdPriorities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdPriority(long id)
        {
            var hdPriority = await _context.HdPriorities.FindAsync(id);
            if (hdPriority == null)
            {
                return NotFound();
            }

            _context.HdPriorities.Remove(hdPriority);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdPriorityExists(long id)
        {
            return _context.HdPriorities.Any(e => e.Id == id);
        }
    }
}
