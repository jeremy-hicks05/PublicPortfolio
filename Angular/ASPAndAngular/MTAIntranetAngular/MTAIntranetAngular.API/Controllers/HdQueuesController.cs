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
    public class HdQueuesController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdQueuesController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdQueues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdQueue>>> GetHdQueues()
        {
            return await _context.HdQueues.ToListAsync();
        }

        // GET: api/HdQueues
        [HttpGet("WhereUserIsSubmitter/{userId}")]
        public async Task<ActionResult<IEnumerable<HdQueue>>> GetHdQueuesSubmitterJT(int userId)
        {

             var userLabels = _context.UserLabelJts
            .Where(ujt => ujt.UserId == userId)
            .Select(ujt => ujt.LabelId)
            .ToList();

            var submitterForQueues = _context.HdQueueSubmitterLabelJts
                .Where(qa => userLabels.Contains(qa.LabelId))
                .Select(qa => qa.HdQueueId)
                .ToList();

            return await _context.HdQueues
                .Where(q => submitterForQueues.Contains(q.Id) ||
                    q.AllowAllUsers == 1)
                .ToListAsync();
        }

        [HttpGet("WhereUserIsApprover/{userId}")]
        public async Task<ActionResult<IEnumerable<HdQueue>>> GetHdQueuesApproverJT(int userId)
        {

            var userLabels = _context.UserLabelJts
           .Where(ujt => ujt.UserId == userId)
           .Select(ujt => ujt.LabelId)
           .ToList();

            var approverForQueues = _context.HdQueueApproverLabelJts
                .Where(qa => userLabels.Contains(qa.LabelId))
                .Select(qa => qa.HdQueueId)
                .ToList();

            return await _context.HdQueues
                .Where(q => approverForQueues.Contains(q.Id) ||
                    q.AllowAllUsers == 1)
                .ToListAsync();
        }

        // GET: api/HdQueues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdQueue>> GetHdQueue(long id)
        {
            var hdQueue = await _context.HdQueues.FindAsync(id);

            if (hdQueue == null)
            {
                return NotFound();
            }

            return hdQueue!;
        }

        // PUT: api/HdQueues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdQueue(long id, HdQueue hdQueue)
        {
            if (id != hdQueue.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdQueue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdQueueExists(id))
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

        // POST: api/HdQueues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdQueue>> PostHdQueue(HdQueue hdQueue)
        {
            _context.HdQueues.Add(hdQueue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdQueue", new { id = hdQueue.Id }, hdQueue);
        }

        // DELETE: api/HdQueues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdQueue(long id)
        {
            var hdQueue = await _context.HdQueues.FindAsync(id);
            if (hdQueue == null)
            {
                return NotFound();
            }

            _context.HdQueues.Remove(hdQueue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdQueueExists(long id)
        {
            return _context.HdQueues.Any(e => e.Id == id);
        }
    }
}
