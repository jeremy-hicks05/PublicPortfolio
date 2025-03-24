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
    public class UserLabelJtsController : ControllerBase
    {
        private readonly Org1Context _context;

        public UserLabelJtsController(Org1Context context)
        {
            _context = context;
        }

        // GET: api/UserLabelJts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLabelJt>>> GetUserLabelJts()
        {
            return await _context.UserLabelJts.ToListAsync();
        }

        [HttpGet("FromUser/{userId}")]
        public async Task<ActionResult<IEnumerable<long>>> GetUserLabelJtsFromUserId(int userId)
        {
            return await _context.UserLabelJts
                .Where(ujt => ujt.UserId == userId)
                .Select(ujt => ujt.LabelId)
                .ToListAsync();
        }

        // GET: api/UserLabelJts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLabelJt>> GetUserLabelJt(long id)
        {
            var userLabelJt = await _context.UserLabelJts.FindAsync(id);

            if (userLabelJt == null)
            {
                return NotFound();
            }

            return userLabelJt;
        }

        // PUT: api/UserLabelJts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLabelJt(long id, UserLabelJt userLabelJt)
        {
            if (id != userLabelJt.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userLabelJt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLabelJtExists(id))
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

        // POST: api/UserLabelJts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserLabelJt>> PostUserLabelJt(UserLabelJt userLabelJt)
        {
            _context.UserLabelJts.Add(userLabelJt);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserLabelJtExists(userLabelJt.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserLabelJt", new { id = userLabelJt.UserId }, userLabelJt);
        }

        // DELETE: api/UserLabelJts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLabelJt(long id)
        {
            var userLabelJt = await _context.UserLabelJts.FindAsync(id);
            if (userLabelJt == null)
            {
                return NotFound();
            }

            _context.UserLabelJts.Remove(userLabelJt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserLabelJtExists(long id)
        {
            return _context.UserLabelJts.Any(e => e.UserId == id);
        }
    }
}
