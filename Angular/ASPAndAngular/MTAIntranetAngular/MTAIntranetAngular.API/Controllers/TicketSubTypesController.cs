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
    public class TicketSubTypesController : ControllerBase
    {
        private readonly MtaticketsContext _context;

        public TicketSubTypesController(MtaticketsContext context)
        {
            _context = context;
        }

        // GET: api/TicketSubTypes
        [HttpGet]
        public async Task<ActionResult<ApiResult<TicketSubType>>> GetTicketSubTypes(
            int pageIndex = 0,
            int pageSize = 50,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null
            )
        {
            if (_context.TicketSubTypes == null)
            {
                return NotFound();
            }
            return await ApiResult<TicketSubType>.CreateAsync(
                _context.TicketSubTypes
                .AsNoTracking()
                .Include(t => t.Category),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery);
        }

        // GET: api/TicketSubTypes/category
        [HttpGet("filter/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TicketSubType>>> GetTicketSubTypes(int categoryId)
        {
            if (_context.TicketSubTypes == null)
            {
                return NotFound();
            }
            return await _context.TicketSubTypes
                .Where(sub => sub.Category!.CategoryId == categoryId)
                //.AsNoTracking()
                .ToListAsync();
        }

        //GET: api/TicketSubTypes/FilterByName/{Name
        [HttpGet]
        [Route("FilterByName/{name}")]
        public List<TicketSubType> FilterByName(string name)
        {
            if (_context.TicketSubTypes == null)
            {
                //return NotFound();
            }
            var ticketSubTypes = _context.TicketSubTypes!
                .Include(t => t.Category)
                //.AsNoTracking()
                .Where(st => st.Category!.Name == name).ToList();

            //if (ticketSubType == null)
            //{
            //return NotFound();
            //}

            return ticketSubTypes;
        }

        //GET: api/TicketSubTypes/FilterByName/{Name
        [HttpGet]
        [Route("FilterById/{id}")]
        public List<TicketSubType> FilterById(int id)
        {
            if (_context.TicketSubTypes == null)
            {
                //return NotFound();
            }
            var ticketSubTypes = _context.TicketSubTypes!
                .Include(t => t.Category)
                //.AsNoTracking()
                .Where(st => st.Category!.CategoryId == id).ToList();

            //if (ticketSubType == null)
            //{
            //return NotFound();
            //}

            return ticketSubTypes;
        }

        // GET: api/TicketSubTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketSubType>> GetTicketSubType(int id)
        {
            if (_context.TicketSubTypes == null)
            {
                return NotFound();
            }
            var ticketSubType = await _context.TicketSubTypes
                .Include(t => t.Category)
                //.AsNoTracking()
                .FirstOrDefaultAsync(s => s.TicketSubTypeId == id);

            if (ticketSubType == null)
            {
                return NotFound();
            }

            return ticketSubType;
        }



        // PUT: api/TicketSubTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketSubType(int id, TicketSubType ticketSubType)
        {
            if (id != ticketSubType.TicketSubTypeId)
            {
                return BadRequest();
            }

            _context.Entry(ticketSubType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketSubTypeExists(id))
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

        // POST: api/TicketSubTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketSubType>> PostTicketSubType(TicketSubType ticketSubType)
        {
            if (_context.TicketSubTypes == null)
            {
                return Problem("Entity set 'MtaticketsContext.TicketSubTypes'  is null.");
            }
            _context.TicketSubTypes.Add(ticketSubType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketSubType", new { id = ticketSubType.TicketSubTypeId }, ticketSubType);
        }

        // DELETE: api/TicketSubTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketSubType(int id)
        {
            if (_context.TicketSubTypes == null)
            {
                return NotFound();
            }
            var ticketSubType = await _context.TicketSubTypes.FindAsync(id);
            if (ticketSubType == null)
            {
                return NotFound();
            }

            _context.TicketSubTypes.Remove(ticketSubType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketSubTypeExists(int id)
        {
            return (_context.TicketSubTypes?
                .Any(e => e.TicketSubTypeId == id))
                .GetValueOrDefault();
        }

        [HttpPost]
        [Route("IsDupeSubType")]
        public bool IsDupeSubType(TicketSubType subType)
        {
            return _context.TicketSubTypes.Any(
            ts => ts.Name == subType.Name
            //&& ts.Category == subType.Category
            && ts.CategoryId == subType.CategoryId
            //&& ts.Description == subType.Description
            && ts.TicketSubTypeId != subType.TicketSubTypeId
            );
        }
    }
}
