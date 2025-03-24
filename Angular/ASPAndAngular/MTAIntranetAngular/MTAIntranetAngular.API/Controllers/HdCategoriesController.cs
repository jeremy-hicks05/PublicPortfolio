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
    public class HdCategoriesController : ControllerBase
    {
        private readonly Org1Context _context;

        public HdCategoriesController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/HdCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HdCategory>>> GetHdCategories()
        {
            return await _context.HdCategories.ToListAsync();
        }

        // GET: api/HdCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdCategory>> GetHdCategory(long id)
        {
            var hdCategory = await _context.HdCategories.FindAsync(id);

            if (hdCategory == null)
            {
                return NotFound();
            }

            return hdCategory!;
        }

        [HttpGet("{categoryId}/GetDefaultOwner")]
        public int GetDefaultOwnerOfHdCategory(long categoryId)
        {
            var hdCategoryOwner = Convert.ToInt32(_context.HdCategories.FindAsync(categoryId)
                .Result!.DefaultOwnerId);

            //if (hdCategoryOwner == null)
            //{
            //    return NotFound();
            //}

            return hdCategoryOwner!;
        }

        [HttpGet("FromQueue/{queueId}")]
        public async Task<ActionResult<IEnumerable<HdCategory>>> GetHdCategoriesFromQueue(int queueId)
        {
            var hdCategories = await _context.HdCategories
                .Where(hdc => hdc.HdQueueId == queueId &&
                    hdc.UserSettable == true)
                .ToListAsync();

            if (hdCategories == null)
            {
                return NotFound();
            }

            return hdCategories;
        }

        // PUT: api/HdCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHdCategory(long id, HdCategory hdCategory)
        {
            if (id != hdCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(hdCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdCategoryExists(id))
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

        // POST: api/HdCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HdCategory>> PostHdCategory(HdCategory hdCategory)
        {
            _context.HdCategories.Add(hdCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHdCategory", new { id = hdCategory.Id }, hdCategory);
        }

        // DELETE: api/HdCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHdCategory(long id)
        {
            var hdCategory = await _context.HdCategories.FindAsync(id);
            if (hdCategory == null)
            {
                return NotFound();
            }

            _context.HdCategories.Remove(hdCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HdCategoryExists(long id)
        {
            return _context.HdCategories.Any(e => e.Id == id);
        }
    }
}
