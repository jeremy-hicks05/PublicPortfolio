using Microsoft.AspNetCore.Mvc;
using ResourceMonitor.Models.ViewModels;
using ResourceMonitor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ResourceMonitor.Controllers
{
    [Authorize(Roles = "Domain Users")]
    [Route("Resources/{controller}")]
    public class DashboardController : Controller
    {
        private readonly MtaresourceMonitoringContext _context;

        public DashboardController(MtaresourceMonitoringContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                Servers = await _context.Servers.ToListAsync(),
                Services = await _context.Services.ToListAsync(),
                Processes = await _context.Processes.ToListAsync(),
                Websites = await _context.Websites.ToListAsync(),
                SqlTables = await _context.Sqltables.ToListAsync(),
            };
            return View(model);
        }
    }

}
