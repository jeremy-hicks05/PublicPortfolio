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
    public class HdStatusController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdStatusController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdStatus>>> GetHdStatuses()
        {
            return await _context.HdStatuses.ToListAsync();
        }

        [HttpGet("FromQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdStatus>>> GetHdStatusesFromQueue(int queueId)
        {
            var hdStatuses = await _context.HdStatuses.Where(hdc => hdc.HdQueueId == queueId).ToListAsync();

            if (hdStatuses == null)
            {
                return NotFound();
            }

            return hdStatuses;
        }

        [HttpGet("DefaultStatusFromQueue/{queueId}")]
        public long GetDefaultStatusFromQueue(int queueId)
        {

            return _context.HdQueues.First(hdq => hdq.Id == queueId).DefaultStatusId ?? 0;

            //long hdStatuses = _context.HdStatuses
            //    .First(hds => 
            //    hds.HdQueueId == queueId).Id;

            //return hdStatuses;
        }

        [HttpGet("OpenStatusFromQueue/{queueId}")]
        public long GetOpenStatusFromQueue(int queueId)
        {
            long hdStatuses = _context.HdStatuses.First(hds => hds.HdQueueId == queueId && hds.Name == "Opened").Id;

            return hdStatuses;
        }

        [HttpGet("NewStatusFromQueue/{queueId}")]
        public long GetNewStatusFromQueue(int queueId)
        {
            long hdStatuses = _context.HdStatuses.First(hds => hds.HdQueueId == queueId && hds.Name == "New").Id;

            return hdStatuses;
        }

        // GET: api/HdStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdStatus>> GetHdStatus(long id)
        {
            var hdStatus = await _context.HdStatuses.FindAsync(id);

            if (hdStatus == null)
            {
                return NotFound();
            }

            return hdStatus;
        }

        // PUT: api/HdStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdStatus(long id, HdStatus hdStatus)
        {
            if (id != hdStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdStatusExists(id))
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

        // POST: api/HdStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdStatus>> PostHdStatus(HdStatus hdStatus)
        {
            _context.HdStatuses.Add(hdStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdStatus", new { id = hdStatus.Id }, hdStatus);
        }

        // DELETE: api/HdStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdStatus(long id)
        {
            var hdStatus = await _context.HdStatuses.FindAsync(id);
            if (hdStatus == null)
            {
                return NotFound();
            }

            _context.HdStatuses.Remove(hdStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdStatusExists(long id)
        {
            return _context.HdStatuses.Any(e => e.Id == id);
        }
    }
}
