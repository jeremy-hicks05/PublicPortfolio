using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HdCustomFieldsController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdCustomFieldsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdCustomFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdCustomField>>> GetHdCustomFields()
        {
            return await _context.HdCustomFields.ToListAsync();
        }

        // GET: api/HdCustomFields
        [HttpGet("ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdCustomField>>> GetHdCustomFieldsForQueue(int queueId)
        {
            return await _context.HdCustomFields.Where(hdcf => 
            hdcf.HdQueueId == queueId).ToListAsync();
        }

        // GET: api/HdCustomFields
        [HttpGet("Text/ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdCustomField>>> GetHdCustomTextFieldsForQueue(int queueId)
        {
            return await _context.HdCustomFields.Where(hdcf => 
            hdcf.HdQueueId == queueId &&
            hdcf.Type == "text"
            ).ToListAsync();
        }

        // GET: api/HdCustomFields
        [HttpGet("ReadOnly/ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdCustomField>>> GetHdCustomReadOnlyFieldsForQueue(int queueId)
        {
            return await _context.HdCustomFields.Where(hdcf =>
            hdcf.HdQueueId == queueId &&
            _context.HdFields.Where(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.Visible == "readonly")
                .Select(hdf => hdf.Name)
                .ToList()
                .Contains(hdcf.Name))
            .ToListAsync();
        }

        // GET: api/HdCustomFields
        [HttpGet("Date/ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdCustomField>>> GetHdCustomDateFieldsForQueue(int queueId)
        {
            return await _context.HdCustomFields.Where(hdcf => 
            hdcf.HdQueueId == queueId &&
            hdcf.Type == "date"
            ).ToListAsync();
        }

        // GET: api/HdCustomFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdCustomField>> GetHdCustomField(int id)
        {
            var hdCustomField = await _context.HdCustomFields.FindAsync(id);

            if (hdCustomField == null)
            {
                return NotFound();
            }

            return hdCustomField;
        }

        // PUT: api/HdCustomFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdCustomField(int id, HdCustomField hdCustomField)
        {
            if (id != hdCustomField.HdQueueId)
            {
                return BadRequest();
            }

            _context.Entry(hdCustomField).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdCustomFieldExists(id))
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

        // POST: api/HdCustomFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdCustomField>> PostHdCustomField(HdCustomField hdCustomField)
        {
            _context.HdCustomFields.Add(hdCustomField);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HdCustomFieldExists(hdCustomField.HdQueueId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHdCustomField", new { id = hdCustomField.HdQueueId }, hdCustomField);
        }

        // DELETE: api/HdCustomFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdCustomField(int id)
        {
            var hdCustomField = await _context.HdCustomFields.FindAsync(id);
            if (hdCustomField == null)
            {
                return NotFound();
            }

            _context.HdCustomFields.Remove(hdCustomField);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdCustomFieldExists(int id)
        {
            return _context.HdCustomFields.Any(e => e.HdQueueId == id);
        }
    }
}
