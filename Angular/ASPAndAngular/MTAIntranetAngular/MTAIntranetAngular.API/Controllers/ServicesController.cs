using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.ServiceProcess;
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
    public class ServicesController : ControllerBase
    {
        private readonly MtaresourceMonitoringContext _context;

        public ServicesController(MtaresourceMonitoringContext context)
        {
            _context = context;
        }

        [Route("Monitor")]
        public async Task<ActionResult<IEnumerable<Service>>> Monitor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (Service s in _context.Services)
                {
                    if (s.ServiceName != null && s.ServerName != null)
                    {
                        ServiceController sc = new ServiceController(s.ServiceName, s.ServerName);

                        switch (sc.Status)
                        {
                            case ServiceControllerStatus.Running:
                                s.PreviousState = s.CurrentState;
                                s.CurrentState = "Healthy";
                                s.LastCheck = DateTime.Now;
                                s.LastHealthy = DateTime.Now;
                                //_context.SaveChanges();
                                break;
                            case ServiceControllerStatus.Stopped:
                                if ((DateTime.Now - s.LastHealthy).Ticks / 360_000_000 >= s.AcceptableDowntime)
                                {
                                    s.PreviousState = s.CurrentState;
                                    s.CurrentState = "Unhealthy";
                                }
                                s.LastCheck = DateTime.Now;
                                //_context.SaveChanges();
                                break;
                            //sc.Start();
                            case ServiceControllerStatus.Paused:
                                if ((DateTime.Now - s.LastHealthy).Ticks / 360_000_000 >= s.AcceptableDowntime)
                                {
                                    s.PreviousState = s.CurrentState;
                                    s.CurrentState = "Unhealthy";
                                }
                                s.LastCheck = DateTime.Now;
                                //_context.SaveChanges();
                                break;
                            case ServiceControllerStatus.StopPending:
                                if ((DateTime.Now - s.LastHealthy).Ticks / 360_000_000 >= s.AcceptableDowntime)
                                {
                                    s.PreviousState = s.CurrentState;
                                    s.CurrentState = "Unhealthy";
                                }
                                s.LastCheck = DateTime.Now;
                                //_context.SaveChanges();
                                break;
                            case ServiceControllerStatus.StartPending:
                                if ((DateTime.Now - s.LastHealthy).Ticks / 360_000_000 >= s.AcceptableDowntime)
                                {
                                    s.PreviousState = s.CurrentState;
                                    s.CurrentState = "Unhealthy";
                                }
                                s.LastCheck = DateTime.Now;
                                //_context.SaveChanges();
                                break;
                            default:
                                if (s.PreviousState == "Unknown" || 
                                    ((DateTime.Now - s.LastHealthy)).Ticks / 360_000_000 >= s.AcceptableDowntime)
                                {
                                    s.PreviousState = s.CurrentState;
                                    s.CurrentState = "Unhealthy";
                                }
                                s.LastCheck = DateTime.Now;
                                //_context.SaveChanges();
                                break;
                        }

                        if (s.PreviousState == "Unknown" &&
                            s.CurrentState == "Unhealthy")
                        {
                            // failed initial connection
                            EmailConfiguration.SendServiceFailure(s);
                            s.LastEmailSent = DateTime.Now;
                            //_context.SaveChanges();
                        }
                        else if (s.PreviousState == "Unknown" &&
                            s.CurrentState == "Healthy")
                        {
                            // successful initial connection
                            EmailConfiguration.SendServiceInitSuccess(s);
                            s.LastEmailSent = DateTime.Now;
                            s.LastHealthy = DateTime.Now;
                            //_context.SaveChanges();
                        }
                        else if (s.PreviousState == "Healthy" &&
                            s.CurrentState == "Unhealthy")
                        {
                            // service failure
                            EmailConfiguration.SendServiceFailure(s);
                            s.LastEmailSent = DateTime.Now;
                            //_context.SaveChanges();
                        }
                        else if (s.PreviousState == "Unhealthy" &&
                            s.CurrentState == "Healthy")
                        {
                            // send successful restoration message
                            EmailConfiguration.SendServiceRestored(s);
                            s.LastEmailSent = DateTime.Now;
                            s.LastHealthy = DateTime.Now;
                            //_context.SaveChanges();
                        }
                        else if (s.EmailFrequency != 0 &&
                            s.PreviousState == "Unhealthy" &&
                            s.CurrentState == "Unhealthy" &&
                            (s.LastEmailSent.AddMinutes(Convert.ToDouble(s.EmailFrequency))
                                <= DateTime.Now))
                        {
                            // process failure reminder
                            EmailConfiguration.SendServiceFailure(s);
                            s.LastEmailSent = DateTime.Now;
                            //_context.SaveChanges();
                        }
                    }
                }
                _context.SaveChanges();
            }
            return await _context.Services.ToListAsync();
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            _context.Services.Add(service);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServiceExists(service.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetService", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
