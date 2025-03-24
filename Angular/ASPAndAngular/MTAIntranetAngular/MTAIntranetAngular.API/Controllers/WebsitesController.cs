using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data;
using MTAIntranetAngular.API.Data.Models;
using MTAIntranetAngular.Utility;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Security.Policy;
using static HotChocolate.ErrorCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace MTAIntranetAngular.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class WebsitesController : ControllerBase
    {
        private readonly MtaresourceMonitoringContext _context;
        private static IConfiguration? _configuration;

        public WebsitesController(MtaresourceMonitoringContext context)
        {
            _context = context;
        }

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Monitor")]
        public async Task<ActionResult<IEnumerable<Website>>> Monitor()
        {


            // Retrieve App Secrets
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<WebsitesController>()
                .Build();

            foreach (Website w in _context.Websites)
            {
                Uri serverUri = new Uri(w.ServerName);
                //Uri serverUri = new Uri("http://" + w.ServerName);

                // needs UriKind arg, or UriFormatException is thrown
                Uri relativeUri = new Uri(w.WebsiteName ?? "null", UriKind.Relative);

                // Define the URI of your website
                //string websiteUri = "https://mtadev/mtaContacts";

                // Define credentials if needed
                //string? username = config.GetValue<string>("mtadevUN");
                //string? password = config.GetValue<string>("mtadevPW");
                //string? username = _configuration?.GetSection("PingUser").GetValue<string>("un");
                //string? password = _configuration?.GetSection("PingUser").GetValue<string>("pw");

                // Create a HttpClientHandler to handle credentials
               // HttpClientHandler handler = new HttpClientHandler();

                //if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                //{
                //    handler.Credentials = new NetworkCredential(username, password);
                //    handler.PreAuthenticate = true;
                //}

                // Uri(Uri, Uri) is the preferred constructor in this case
                Uri fullUri = new Uri(serverUri, relativeUri);
                try
                {
                    using var ping = new HttpClient();
                    var reply = await ping.GetAsync(fullUri);
                    switch (reply.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            var msg =
                                $"PING/ICMP to {w.WebsiteName} succees.";
                            w.PreviousState = w.CurrentState;
                            w.CurrentState = "Healthy";
                            w.LastHealthy = DateTime.Now;
                            w.LastCheck = DateTime.Now;
                            //_context.SaveChanges();
                            break;
                        case HttpStatusCode.Unauthorized:
                            var msgUnAuth =
                                $"PING/ICMP to {w.WebsiteName} unauthorized.";
                            w.PreviousState = w.CurrentState;
                            w.CurrentState = "Healthy";
                            w.LastHealthy = DateTime.Now;
                            w.LastCheck = DateTime.Now;
                            break;
                        default:
                            if ((DateTime.Now - w.LastHealthy).Ticks / 360_000_000 >= w.AcceptableDowntime)
                            {
                                var err =
                                    $"PING/ICMP to {w.WebsiteName} failed";
                                w.PreviousState = w.CurrentState;
                                w.CurrentState = "Unhealthy";
                            }
                            w.LastCheck = DateTime.Now;
                            //_context.SaveChanges();
                            break;
                    }
                }
                catch (Exception e)
                {
                    if (w.PreviousState == "Unknown" || 
                        (DateTime.Now - w.LastHealthy).Ticks / 360_000_000 >= w.AcceptableDowntime)
                    {
                        var err =
                        $"PING/ICMP to {w.WebsiteName} failed {e.Message}";
                        w.PreviousState = w.CurrentState;
                        w.CurrentState = "Unhealthy";
                    }
                    w.LastCheck = DateTime.Now;
                    //_context.SaveChanges();
                }
                if (w.PreviousState == "Unknown" &&
                    w.CurrentState == "Unhealthy")
                {
                    // failed to initially connect
                    EmailConfiguration.SendWebsiteFailure(w);
                    w.LastEmailSent = DateTime.Now;
                    //_context.SaveChanges();
                }
                else if (w.PreviousState == "Unknown" &&
                    w.CurrentState == "Healthy")
                {
                    // successful initial connection
                    EmailConfiguration.SendWebsiteInitSuccess(w);
                    w.LastEmailSent = DateTime.Now;
                    w.LastHealthy = DateTime.Now;
                    //_context.SaveChanges();
                }
                else if (w.PreviousState == "Healthy" &&
                    w.CurrentState == "Unhealthy")
                {
                    // website failure
                    EmailConfiguration.SendWebsiteFailure(w);
                    w.LastEmailSent = DateTime.Now;
                    //_context.SaveChanges();
                }
                else if (w.PreviousState == "Unhealthy" &&
                    w.CurrentState == "Healthy")
                {
                    // send successful restoration message
                    EmailConfiguration.SendWebsiteRestored(w);
                    w.LastEmailSent = DateTime.Now;
                    w.LastHealthy = DateTime.Now;
                    //_context.SaveChanges();
                }
                else if (w.EmailFrequency != 0 &&
                    w.PreviousState == "Unhealthy" &&
                    w.CurrentState == "Unhealthy" &&
                    (w.LastEmailSent.AddMinutes(Convert.ToDouble(w.EmailFrequency))
                        <= DateTime.Now))
                {
                    // process failure reminder
                    EmailConfiguration.SendWebsiteFailure(w);
                    w.LastEmailSent = DateTime.Now;
                    //_context.SaveChanges();
                }
            }
            _context.SaveChanges();
            return await _context.Websites.ToListAsync();
        }

        // GET: api/Websites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Website>>> GetWebsites()
        {
            return await _context.Websites.ToListAsync();
        }

        // GET: api/Websites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Website>> GetWebsite(int id)
        {
            var website = await _context.Websites.FindAsync(id);

            if (website == null)
            {
                return NotFound();
            }

            return website;
        }

        // PUT: api/Websites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWebsite(int id, Website website)
        {
            if (id != website.Id)
            {
                return BadRequest();
            }

            _context.Entry(website).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WebsiteExists(id))
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

        // POST: api/Websites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Website>> PostWebsite(Website website)
        {
            _context.Websites.Add(website);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WebsiteExists(website.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWebsite", new { id = website.Id }, website);
        }

        // DELETE: api/Websites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWebsite(int id)
        {
            var website = await _context.Websites.FindAsync(id);
            if (website == null)
            {
                return NotFound();
            }

            _context.Websites.Remove(website);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WebsiteExists(int id)
        {
            return _context.Websites.Any(e => e.Id == id);
        }
    }
}
