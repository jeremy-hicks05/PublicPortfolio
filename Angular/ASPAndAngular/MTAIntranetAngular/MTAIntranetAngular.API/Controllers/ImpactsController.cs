using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpactsController : ControllerBase
    {
        private readonly MtaticketsContext _context;

        public ImpactsController(MtaticketsContext context)
        {
            _context = context;
        }

        // GET: api/Impacts
        [HttpGet]
        public async Task<ActionResult<ApiResult<Impact>>> GetImpacts(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null)
        {
            if (_context.Impacts == null)
            {
                return NotFound();
            }
            return await ApiResult<Impact>.CreateAsync(_context.Impacts
                .AsNoTracking(),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery);
        }

        // GET: api/Impacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Impact>> GetImpact(int id)
        {
            if (_context.Impacts == null)
            {
                return NotFound();
            }
            var impact = await _context.Impacts.FindAsync(id);

            if (impact == null)
            {
                return NotFound();
            }

            return impact;
        }

        // PUT: api/Impacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImpact(int id, Impact impact)
        {
            if (id != impact.ImpactId)
            {
                return BadRequest();
            }

            _context.Entry(impact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImpactExists(id))
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

        // POST: api/Impacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Impact>> PostImpact(Impact impact)
        {
            if (_context.Impacts == null)
            {
                return Problem("Entity set 'MtaticketsContext.Impacts'  is null.");
            }
            _context.Impacts.Add(impact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImpact", new { id = impact.ImpactId }, impact);
        }

        // DELETE: api/Impacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImpact(int id)
        {
            if (_context.Impacts == null)
            {
                return NotFound();
            }
            var impact = await _context.Impacts.FindAsync(id);
            if (impact == null)
            {
                return NotFound();
            }

            _context.Impacts.Remove(impact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImpactExists(int id)
        {
            return (_context.Impacts?.Any(e => e.ImpactId == id)).GetValueOrDefault();
        }

        [HttpPost]
        [Route("IsDupeImpact")]
        public bool IsDupeImpact(Impact impact)
        {
            return _context.Impacts.Any(
            i => i.Description == impact.Description
            && i.ImpactId != impact.ImpactId
            );
        }
    }
}
