using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MTAIntranetAngular.API.Data;
using MTAIntranetAngular.API.Data.Models;
using MTAIntranetAngular.Utility;

namespace MTAIntranetAngular.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        private readonly MtaresourceMonitoringContext _context;

        public ProcessesController(MtaresourceMonitoringContext context)
        {
            _context = context;
        }

        [Route("Monitor")]
        public async Task<ActionResult<IEnumerable<Process>>> Monitor()
        {
            foreach (Process p in _context.Processes)
            {
                try
                {
                    System.Diagnostics.Process[] ipByName = System.Diagnostics.Process.GetProcessesByName(p.ProcessName, p.ServerName);
                    switch (ipByName.Length > 0)
                    {
                        case true:
                            //var msg =
                            //    "Success";
                            p.PreviousState = p.CurrentState;
                            p.CurrentState = "Healthy";
                            p.LastCheck = DateTime.Now;
                            p.LastHealthy = DateTime.Now;
                            //_context.SaveChanges();
                            break;
                        default:
                            if (((DateTime.Now - p.LastHealthy).Ticks / 360_000_000) >= p.AcceptableDowntime)
                            {
                                var err =
                                    $"Process {p.ProcessName} not running on {p.ServerName}";
                                p.PreviousState = p.CurrentState;
                                p.CurrentState = "Unhealthy";

                                //_context.SaveChanges();
                            }
                            p.LastCheck = DateTime.Now;
                            break;
                    }
                }
                catch(Exception e)
                {
                    if (p.PreviousState == "Unknown" ||
                        (DateTime.Now - p.LastHealthy).Ticks / 360_000_000 >= p.AcceptableDowntime)
                    {
                        var err = e.Message + " " + 
                        $"Process {p.ProcessName} not running on {p.ServerName}";
                        p.PreviousState = p.CurrentState;
                        p.CurrentState = "Unhealthy";
                    }
                    p.LastCheck = DateTime.Now;
                    //_context.SaveChanges();
                }
                // init
                if (p.PreviousState == "Unknown" &&
                    p.CurrentState == "Unhealthy")
                {
                    // failed initial connection
                    EmailConfiguration.SendProcessFailure(p);
                    p.LastEmailSent = DateTime.Now;
                    p.LastHealthy = DateTime.MinValue;
                    //_context.SaveChanges();
                }
                else if (p.PreviousState == "Unknown" &&
                    p.CurrentState == "Healthy")
                {
                    // successful initial connection
                    EmailConfiguration.SendProcessInitSuccess(p);
                    p.LastEmailSent = DateTime.Now;
                    p.LastHealthy = DateTime.Now;
                    //_context.SaveChanges();
                }
                // end init
                // healthy then unhealthy - add acceptable downtime
                else if (
                    p.PreviousState == "Healthy" &&
                    p.CurrentState == "Unhealthy")
                {
                    // process failure
                    EmailConfiguration.SendProcessFailure(p);
                    p.LastEmailSent = DateTime.Now;
                    //_context.SaveChanges();
                }
                else if (p.PreviousState == "Unhealthy" &&
                    p.CurrentState == "Healthy")
                {
                    // successful restoration
                    EmailConfiguration.SendProcessRestored(p);
                    p.LastEmailSent = DateTime.Now;
                    p.LastHealthy = DateTime.Now;
                    //_context.SaveChanges();
                }
                else if (p.EmailFrequency != 0 &&
                    p.PreviousState == "Unhealthy" &&
                    p.CurrentState == "Unhealthy" &&
                    (p.LastEmailSent
                        .AddMinutes(Convert.ToDouble(p.EmailFrequency))
                        <= DateTime.Now))
                {
                    // process failure reminder
                    EmailConfiguration.SendProcessFailure(p);
                    p.LastEmailSent = DateTime.Now;
                    //_context.SaveChanges();
                }
            }
            _context.SaveChanges();
            return await _context.Processes.ToListAsync();
        }

        // GET: api/Processes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Process>>> GetProcesses()
        {
            return await _context.Processes.ToListAsync();
        }

        // GET: api/Processes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Process>> GetProcess(int id)
        {
            var process = await _context.Processes.FindAsync(id);

            if (process == null)
            {
                return NotFound();
            }

            return process;
        }

        // PUT: api/Processes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcess(int id, Process process)
        {
            if (id != process.Id)
            {
                return BadRequest();
            }

            _context.Entry(process).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessExists(id))
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

        // POST: api/Processes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Process>> PostProcess(Process process)
        {
            _context.Processes.Add(process);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProcessExists(process.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProcess", new { id = process.Id }, process);
        }

        // DELETE: api/Processes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcess(int id)
        {
            var process = await _context.Processes.FindAsync(id);
            if (process == null)
            {
                return NotFound();
            }

            _context.Processes.Remove(process);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcessExists(int id)
        {
            return _context.Processes.Any(e => e.Id == id);
        }
    }
}
