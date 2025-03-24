using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data;
using MTAIntranetAngular.API.Data.Models;
using MTAIntranetAngular.Utility;

namespace MTAIntranetAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalStatesController : ControllerBase
    {
        private readonly MtaticketsContext _context;

        public ApprovalStatesController(MtaticketsContext context)
        {
            _context = context;
        }

        // GET: api/ApprovalStates
        [HttpGet]
        public async Task<ActionResult<ApiResult<ApprovalState>>> GetApprovalStates(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null)
        {
          if (_context.ApprovalStates == null)
          {
              return NotFound();
          }

            return await ApiResult<ApprovalState>.CreateAsync(_context.ApprovalStates
                .AsNoTracking(),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery);
        }

        // GET: api/ApprovalStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalState>> GetApprovalState(int id)
        {
          if (_context.ApprovalStates == null)
          {
              return NotFound();
          }
            var approvalState = await _context.ApprovalStates.FindAsync(id);

            if (approvalState == null)
            {
                return NotFound();
            }

            return approvalState;
        }

        // PUT: api/ApprovalStates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalState(int id, ApprovalState approvalState)
        {
            if (id != approvalState.ApprovalStateId)
            {
                return BadRequest();
            }

            _context.Entry(approvalState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalStateExists(id))
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

        // POST: api/ApprovalStates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApprovalState>> PostApprovalState(ApprovalState approvalState)
        {
          if (_context.ApprovalStates == null)
          {
              return Problem("Entity set 'MtaticketsContext.ApprovalStates'  is null.");
          }
            _context.ApprovalStates.Add(approvalState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprovalState", new { id = approvalState.ApprovalStateId }, approvalState);
        }

        // DELETE: api/ApprovalStates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalState(int id)
        {
            if (_context.ApprovalStates == null)
            {
                return NotFound();
            }
            var approvalState = await _context.ApprovalStates.FindAsync(id);
            if (approvalState == null)
            {
                return NotFound();
            }

            _context.ApprovalStates.Remove(approvalState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApprovalStateExists(int id)
        {
            return (_context.ApprovalStates?
                .Any(aState => aState.ApprovalStateId == id))
                .GetValueOrDefault();
        }

        [HttpPost]
        [Route("IsDupeApprovalState")]
        public bool IsDupeApprovalState(ApprovalState approvalState)
        {
            return _context.ApprovalStates.Any(
            aState => aState.Name == approvalState.Name
            && aState.ApprovalStateId != approvalState.ApprovalStateId
            );
        }
    }
}
