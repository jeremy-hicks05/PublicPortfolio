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
    public class HdQueueApproverLabelJtsController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdQueueApproverLabelJtsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdQueueApproverLabelJts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdQueueApproverLabelJt>>> GetHdQueueApproverLabelJts()
        {
            return await _context.HdQueueApproverLabelJts.ToListAsync();
        }

        // GET: api/HdQueueApproverLabelJts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdQueueApproverLabelJt>> GetHdQueueApproverLabelJt(long id)
        {
            var hdQueueApproverLabelJt = await _context.HdQueueApproverLabelJts.FindAsync(id);

            if (hdQueueApproverLabelJt == null)
            {
                return NotFound();
            }

            return hdQueueApproverLabelJt;
        }

        // PUT: api/HdQueueApproverLabelJts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdQueueApproverLabelJt(long id, HdQueueApproverLabelJt hdQueueApproverLabelJt)
        {
            if (id != hdQueueApproverLabelJt.HdQueueId)
            {
                return BadRequest();
            }

            _context.Entry(hdQueueApproverLabelJt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdQueueApproverLabelJtExists(id))
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

        // POST: api/HdQueueApproverLabelJts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdQueueApproverLabelJt>> PostHdQueueApproverLabelJt(HdQueueApproverLabelJt hdQueueApproverLabelJt)
        {
            _context.HdQueueApproverLabelJts.Add(hdQueueApproverLabelJt);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HdQueueApproverLabelJtExists(hdQueueApproverLabelJt.HdQueueId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHdQueueApproverLabelJt", new { id = hdQueueApproverLabelJt.HdQueueId }, hdQueueApproverLabelJt);
        }

        // DELETE: api/HdQueueApproverLabelJts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdQueueApproverLabelJt(long id)
        {
            var hdQueueApproverLabelJt = await _context.HdQueueApproverLabelJts.FindAsync(id);
            if (hdQueueApproverLabelJt == null)
            {
                return NotFound();
            }

            _context.HdQueueApproverLabelJts.Remove(hdQueueApproverLabelJt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdQueueApproverLabelJtExists(long id)
        {
            return _context.HdQueueApproverLabelJts.Any(e => e.HdQueueId == id);
        }
    }
}
