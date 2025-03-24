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
    public class HdQueueSubmitterLabelJtsController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdQueueSubmitterLabelJtsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdQueueSubmitterLabelJts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdQueueSubmitterLabelJt>>> GetHdQueueSubmitterLabelJts()
        {
            return await _context.HdQueueSubmitterLabelJts.ToListAsync();
        }

        // GET: api/HdQueueSubmitterLabelJts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdQueueSubmitterLabelJt>> GetHdQueueSubmitterLabelJt(long id)
        {
            var hdQueueSubmitterLabelJt = await _context.HdQueueSubmitterLabelJts.FindAsync(id);

            if (hdQueueSubmitterLabelJt == null)
            {
                return NotFound();
            }

            return hdQueueSubmitterLabelJt;
        }

        // PUT: api/HdQueueSubmitterLabelJts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdQueueSubmitterLabelJt(long id, HdQueueSubmitterLabelJt hdQueueSubmitterLabelJt)
        {
            if (id != hdQueueSubmitterLabelJt.HdQueueId)
            {
                return BadRequest();
            }

            _context.Entry(hdQueueSubmitterLabelJt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdQueueSubmitterLabelJtExists(id))
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

        // POST: api/HdQueueSubmitterLabelJts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdQueueSubmitterLabelJt>> PostHdQueueSubmitterLabelJt(HdQueueSubmitterLabelJt hdQueueSubmitterLabelJt)
        {
            _context.HdQueueSubmitterLabelJts.Add(hdQueueSubmitterLabelJt);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HdQueueSubmitterLabelJtExists(hdQueueSubmitterLabelJt.HdQueueId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHdQueueSubmitterLabelJt", new { id = hdQueueSubmitterLabelJt.HdQueueId }, hdQueueSubmitterLabelJt);
        }

        // DELETE: api/HdQueueSubmitterLabelJts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdQueueSubmitterLabelJt(long id)
        {
            var hdQueueSubmitterLabelJt = await _context.HdQueueSubmitterLabelJts.FindAsync(id);
            if (hdQueueSubmitterLabelJt == null)
            {
                return NotFound();
            }

            _context.HdQueueSubmitterLabelJts.Remove(hdQueueSubmitterLabelJt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdQueueSubmitterLabelJtExists(long id)
        {
            return _context.HdQueueSubmitterLabelJts.Any(e => e.HdQueueId == id);
        }
    }
}
