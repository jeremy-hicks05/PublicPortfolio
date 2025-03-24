using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HdFieldsController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdFieldsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdFields()
        {
            return await _context.HdFields.ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("ForQueue/All/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdAllFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf => 
            hdf.HdQueueId == queueId &&
            hdf.HdTicketFieldName != null)
            .ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf => hdf.HdQueueId == queueId && 
            hdf.HdTicketFieldName != null && 
            !hdf.HdTicketFieldName.Contains("custom")).Skip(1).ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("Custom/ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdFieldsCustomForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf => 
            hdf.HdQueueId == queueId && 
            hdf.HdTicketFieldName != null &&
            hdf.HdTicketFieldName.Contains("custom")).ToListAsync();
        }

        [HttpGet("ReadOnly/ForQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdReadOnlyFieldsCustomForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf =>
            hdf.HdQueueId == queueId &&
            hdf.HdTicketFieldName != null &&
            hdf.Visible == "readonly" &&
            hdf.HdTicketFieldName.Contains("custom")).ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("Custom/ForQueue/{queueId}/UserCreate")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdUserCreateFieldsCustomForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf => 
            hdf.HdQueueId == queueId && hdf.HdTicketFieldName != null &&
                hdf.HdTicketFieldName.Contains("custom") &&
                //!hdf.HdTicketFieldName.Contains("owner_id") &&
                //!hdf.HdTicketFieldName.Contains("machine_id") &&
                hdf.Visible == "usercreate").ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("ForQueue/{queueId}/Required")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdRequiredFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf => 
            hdf.HdQueueId == queueId && 
            hdf.HdTicketFieldName != null && 
            !hdf.HdTicketFieldName.Contains("custom") && 
            hdf.RequiredState == "all").ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("ForQueue/{queueId}/UserCreate")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdUserCreateFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf =>
                hdf.HdQueueId == queueId && 
                hdf.HdTicketFieldName != null &&
                !hdf.HdTicketFieldName.Contains("sat_survey") &&
                !hdf.HdTicketFieldName.Contains("due_date") &&
                !hdf.HdTicketFieldName.Contains("title") &&
                !hdf.HdTicketFieldName.Contains("summary") &&
                !hdf.HdTicketFieldName.Contains("impact") &&
                !hdf.HdTicketFieldName.Contains("priority") &&
                !hdf.HdTicketFieldName.Contains("category") &&
                !hdf.HdTicketFieldName.Contains("approver") &&
                !hdf.HdTicketFieldName.Contains("custom") &&
                !hdf.HdTicketFieldName.Contains("status") &&
                !hdf.HdTicketFieldName.Contains("submitter") &&
                hdf.Visible == "usercreate").ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("Text/ForQueue/{queueId}/UserCreate")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdUserCreateTextFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.HdTicketFieldName != null &&
                hdf.HdTicketFieldName.Contains("custom") &&
                hdf.Visible == "usercreate" &&
                    _context.HdCustomFields.Where(hdcf =>
                        hdcf.HdQueueId == hdf.HdQueueId &&
                        hdcf.Type == "text").Select(hdcf => hdcf.Name).ToList()
                        .Contains(hdf.Name)).ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("Checkbox/ForQueue/{queueId}/UserCreate")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdUserCreateCheckboxFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.HdTicketFieldName != null &&
                hdf.HdTicketFieldName.Contains("custom") &&
                //hdf.Visible == "usercreate" &&
                    _context.HdCustomFields.Where(hdcf =>
                        hdcf.HdQueueId == hdf.HdQueueId &&
                        hdcf.Type == "checkbox").Select(hdcf => hdcf.Name).ToList()
                        .Contains(hdf.Name)).ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("Date/ForQueue/{queueId}/UserCreate")]
        public async Task<ActionResult<IEnumerable<HdField>>> GetHdUserCreateDateFieldsForQueue(int queueId)
        {
            return await _context.HdFields.Where(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.HdTicketFieldName != null &&
                hdf.HdTicketFieldName.Contains("custom") &&
                hdf.Visible == "usercreate" &&
                    _context.HdCustomFields.Where(hdcf =>
                        hdcf.HdQueueId == hdf.HdQueueId &&
                        hdcf.Type == "date").Select(hdcf => hdcf.Name).ToList()
                        .Contains(hdf.Name)).ToListAsync();
        }

        // GET: api/HdFields
        [HttpGet("HasDueDate/{queueId}")]
        public bool HasDueDate(int queueId)
        {
            return _context.HdFields.Where(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.Visible == "usercreate" &&
                hdf.HdTicketFieldName != null)
                .Select(hdf => 
                    hdf.HdTicketFieldName)
                .ToList()
                .Contains("due_date");
        }

        // GET: api/HdFields
        [HttpGet("DueDateLabel/{queueId}")]
        public string DueDateLabel(int queueId)
        {
            return
                _context.HdFields.FirstOrDefault(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.Visible == "usercreate" &&
                hdf.Name == "due_date") == null ? 
                "none" 
                :
                _context.HdFields.First(hdf =>
                hdf.HdQueueId == queueId &&
                hdf.Visible == "usercreate" &&
                hdf.Name == "due_date").FieldLabel;
        }

        // GET: api/HdFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdField>> GetHdField(long id)
        {
            var hdField = await _context.HdFields.FindAsync(id);

            if (hdField == null)
            {
                return NotFound();
            }

            return hdField;
        }

        // PUT: api/HdFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdField(long id, HdField hdField)
        {
            if (id != hdField.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdField).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdFieldExists(id))
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

        // POST: api/HdFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdField>> PostHdField(HdField hdField)
        {
            _context.HdFields.Add(hdField);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdField", new { id = hdField.Id }, hdField);
        }

        // DELETE: api/HdFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdField(long id)
        {
            var hdField = await _context.HdFields.FindAsync(id);
            if (hdField == null)
            {
                return NotFound();
            }

            _context.HdFields.Remove(hdField);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdFieldExists(long id)
        {
            return _context.HdFields.Any(e => e.Id == id);
        }
    }
}
