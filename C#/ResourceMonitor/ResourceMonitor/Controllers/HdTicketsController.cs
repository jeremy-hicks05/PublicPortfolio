using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities;
using ResourceMonitor.Data;
using ResourceMonitor.Models.KACE;
using ResourceMonitor.ModelsTest;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ResourceMonitor.Controllers
{
    [Authorize(Roles = "ITS Users,Finance Users")]
    public class HdTicketsController : Controller
    {
        private readonly Org1Context _context;

        public HdTicketsController(Org1Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> TicketPaths()
        {
            return View();
        }


        // GET: HdTickets
        public async Task<IActionResult> Index()
        {
            DateTime startDate = DateTime.Today.AddDays(-1);
            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                // look back 3 days if today is Monday to cover the weekend
                startDate = DateTime.Today.AddDays(-3);
            }

            DateTime endDate = DateTime.Today;

            List<HdTicket> tickets = _context.HdTickets.Where(
                hdt => hdt.Created.Date >= startDate.Date &&
                hdt.Created.Date <= endDate.Date).ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );
            ViewBag.DaysBack = DateTime.Today.DayOfWeek == DayOfWeek.Monday ? 3 : 1;
            return View(models);
        }

        [Route("{controller}/{action}/{numDays}")]
        public async Task<IActionResult> Index(int numDays)
        {
            DateTime startDate = DateTime.Today.AddDays(-1 * numDays);

            DateTime endDate = DateTime.Today;

            List<HdTicket> tickets = _context.HdTickets.Where(
                hdt => hdt.Created.Date >= startDate.Date &&
                hdt.Created.Date <= endDate.Date).ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );
            ViewBag.DaysBack = numDays;
            return View(models);
        }

        [Route("{controller}/{action}/{year}/{month}/{day}")]
        public async Task<IActionResult> CreatedOnDay(int year, int month, int day)
        {
            DateTime dayToView = new DateTime(year, month, day);
            //DateTime startDate = DateTime.Today.AddDays(-1);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    // look back 3 days if today is Monday to cover the weekend
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            //DateTime endDate = DateTime.Today;

            List<HdTicket> tickets = _context.HdTickets.Where(
                hdt => hdt.Created.Date == dayToView.Date).ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models);
        }

        [Route("{controller}/{action}/{year}/{month}/{day}")]
        public async Task<IActionResult> CreatedOnWeek(int year, int month, int day)
        {
            DateTime startDate = new DateTime(year, month, day);
            //DateTime startDate = DateTime.Today.AddDays(-1);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    // look back 3 days if today is Monday to cover the weekend
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            //DateTime endDate = DateTime.Today;

            var statusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed"
                        &&
                        s.Name != "Resolved"
                        //s.Name != "Waiting on Submitter Input" &&
                        //s.Name != "Waiting on Vendor Input" &&
                        //s.Name != "Future Project"
                        )
                        .Select(s => s.Id)
                        .ToList();

            List<HdTicket> tickets = _context.HdTickets.Where(
                hdt => hdt.Created.Date >= startDate.Date &&
                hdt.Created.Date < startDate.AddDays(7).Date &&
                statusList.Contains(hdt.HdStatusId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models);
        }

        [Route("{controller}/{action}/{year}/{month}")]
        public async Task<IActionResult> CreatedOnMonth(int year, int month)
        {
            DateTime startDay = new DateTime(year, month, 1);
            //DateTime startDate = DateTime.Today.AddDays(-1);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    // look back 3 days if today is Monday to cover the weekend
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            //DateTime endDate = DateTime.Today;

            var statusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed"
                        &&
                        s.Name != "Resolved"
                        //s.Name != "Waiting on Submitter Input" &&
                        //s.Name != "Waiting on Vendor Input" &&
                        //s.Name != "Future Project"
                        )
                        .Select(s => s.Id)
                        .ToList();

            List<HdTicket> tickets = _context.HdTickets.Where(
                hdt =>
                hdt.Created.Date.Year == startDay.Date.Year &&
                hdt.Created.Date.Month == startDay.Date.Month &&
                statusList.Contains(hdt.HdStatusId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models);
        }

        [Route("{controller}/{action}")]
        public async Task<IActionResult> Projects()
        {
            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name != "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            List<HdTicket> tickets = _context.HdTickets
                .Where(t =>
                _context.HdQueues.First(
                    s =>
                    s.Id == t.HdQueueId).Name.Contains("Project") &&
                    notClosedStatusList.Contains(t.HdStatusId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models
            //    .Where(hdt => 
            //hdt.HdStatus.Name.Contains("Project"))
                );
        }

        [Route("{controller}/{action}")]
        public async Task<IActionResult> StaleProjects()
        {
            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name != "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            List<HdTicket> tickets = _context.HdTickets
                .Where(t =>
                _context.HdQueues.First(
                    s =>

                    s.Id == t.HdQueueId).Name.Contains("Project")
                    && t.Created <= DateTime.Today.AddMonths(-6) &&
                    notClosedStatusList.Contains(t.HdStatusId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models
            //    .Where(hdt => 
            //hdt.HdStatus.Name.Contains("Project"))
                );
        }

        public async Task<IActionResult> Closed()
        {
            DateTime startDate = DateTime.Today.AddDays(-8);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            DateTime endDate = DateTime.Today.AddDays(-1);

            List<HdTicket> tickets = _context.HdTickets.Where(
                hdt => hdt.Created.Date >= startDate.Date &&
                hdt.Created.Date <= endDate.Date).ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models.Where(hdt => hdt.HdStatus.Name == "Closed"));
        }

        public async Task<IActionResult> NotClosed()
        {
            //DateTime startDate = DateTime.Today.AddDays(-8);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            //DateTime endDate = DateTime.Today.AddDays(-1);

            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name != "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            List<HdTicket> tickets = _context.HdTickets
                .Where(hdt => notClosedStatusList
                        .Contains(hdt.HdStatusId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            return View(models);
        }

        [Route("{controller}/{action}/{userName}")]
        public async Task<IActionResult> NotClosed(string userName)
        {
            //DateTime startDate = DateTime.Today.AddDays(-8);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            //DateTime endDate = DateTime.Today.AddDays(-1);

            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name != "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            List<HdTicket> tickets = _context.HdTickets
                .Where(hdt => notClosedStatusList
                        .Contains(hdt.HdStatusId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            ViewBag.UserName = userName;

            return View(models
                .Where(hdt => 
                        //hdt.HdStatus.Name != "Closed" && 
                        hdt.Owner != null &&
                        hdt.Owner.Id != 30 && //not unassigned
                        hdt.Owner.UserName == userName));
        }

        [Route("{controller}/{action}/{userName}")]
        public async Task<IActionResult> NotClosedNonProjects(string userName)
        {
            //DateTime startDate = DateTime.Today.AddDays(-8);
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //{
            //    startDate = DateTime.Today.AddDays(-3);
            //}

            //DateTime endDate = DateTime.Today.AddDays(-1);

            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name!= "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            var notProjectQueueList = _context.HdQueues
                .Where(hdq => hdq.Name != "Project").Select(hdq => hdq.Id);

            List<HdTicket> tickets = _context.HdTickets
                .Where(hdt => 
                        notClosedStatusList.Contains(hdt.HdStatusId) &&
                        notProjectQueueList.Contains(hdt.HdQueueId))
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            ViewBag.UserName = userName;

            var result = models
                .Where(hdt =>
                        //hdt.HdStatus.Name != "Closed" && 
                        hdt.Owner != null &&
                        hdt.Owner.Id != 30 && //not unassigned
                        hdt.Owner.UserName == userName).ToList();

            return View(result);
        }

        [Route("{controller}/{action}/{userName?}")]
        public async Task<IActionResult> Dashboard(string? userName)
        {
            //var testTicket = _context.HdTickets.First(hdt => hdt.Id == 14990);

            List<long> itsOwnersLabel = _context.UserLabelJts
                .Where(ulj =>
                    ulj.LabelId == 90).Select(ulj => ulj.UserId).ToList();

            List<User> itsOwners = _context.Users
                .Where(u =>
                itsOwnersLabel.Contains(u.Id)
                && u.UserName != "admin").ToList();

            itsOwners.Add(new Models.KACE.User()
            {
                UserName = "Unassigned"
            });

            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name != "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            var notProjectQueueList = _context.HdQueues
                .Where(hdq => hdq.Name != "Project").Select(hdq => hdq.Id);

            List<long> openTicketIDs = _context.
                HdTickets.Where(hdt =>
                    //notProjectQueueList.Contains(hdt.HdQueueId) &&
                    notClosedStatusList.Contains(hdt.HdStatusId))
                .Select(hdt => hdt.Id).ToList();

            //bool hasMyTicket = openTicketIDs.Contains(14990);

            IEnumerable<IGrouping<long?, HdTicketChange>> allComments =
                _context.HdTicketChanges
                .Where(
                    c =>
                    openTicketIDs.Contains((long)c.HdTicketId))
                .OrderByDescending(c => c.Timestamp).GroupBy(
                c => c.HdTicketId);

            IEnumerable<HdTicketChange> highestTimestampChanges = allComments
                .SelectMany(group => group)
                // Order the list by TimeStamp attribute in descending order
                .OrderByDescending(change => change.Timestamp)
                // Group by the key (long?) and take the first element from each group
                .GroupBy(change => change.HdTicketId)
                .Select(group => group.First());

            List<HdTicketChange> htcList = highestTimestampChanges
                .Where(
                    c =>
                    notClosedStatusList
                        .Contains(_context.HdTickets
                            .First(t =>
                                t.Id == c.HdTicketId).HdStatusId)
                    )
                .OrderBy(hL => hL.Timestamp)
                .ToList();

            //var hasMyTicketTest = htcList.OrderBy(cad => cad.HdTicketId).Select(htc => htc.HdTicketId).Contains(14990);

            List<CommentOwnerAndSubmitter> commentsAndOwners = new();

            htcList
                .ToList()
                .ForEach(
                htc =>
                    commentsAndOwners.Add(
                        new CommentOwnerAndSubmitter()
                        {
                            Comment = htc,
                            Owner = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .OwnerId,
                            Submitter = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .SubmitterId
                        }
                    ));

            //var myTestTicket = commentsAndOwners.First(cao => cao.Comment.HdTicketId == 14990);

            // create allTickets list here?

            var allTickets = commentsAndOwners;

            commentsAndOwners = commentsAndOwners.
                Where(cao => cao.Owner !=
                cao.Comment.UserId).ToList();

            //var hasMyTicketComment = commentsAndOwners.OrderBy(cad => cad.Comment.HdTicketId).Select(cad => cad.Comment.HdTicketId).Contains(14990);

            List<HdTicket> tickets = new();
            List<HdTicket> testAllTickets = new();

            // add stale tickets
            List<HdTicket> staleTickets = _context.HdTickets.Where(hdt =>
                (notProjectQueueList.Contains(hdt.HdQueueId) &&
                notClosedStatusList.Contains(hdt.HdStatusId) &&
                hdt.Modified <= DateTime.Today.AddDays(-14))
                ||
                (!notProjectQueueList.Contains(hdt.HdQueueId) &&
                notClosedStatusList.Contains(hdt.HdStatusId) &&
                hdt.Modified <= DateTime.Today.AddDays(-30))
                ).ToList();

            //List<HdTicket> allTickets = _context.HdTickets.Where(hdt =>
            //    (notProjectQueueList.Contains(hdt.HdQueueId) &&
            //    notClosedStatusList.Contains(hdt.HdStatusId)))
            //    .ToList();

            commentsAndOwners
                .ForEach(
                cao =>
                    tickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId)));

            allTickets
                .ForEach(
                cao =>
                    testAllTickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId)));

            tickets.RemoveAll(t => t == null);

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();
            List<HdTicketViewModel> allTicketsModels = new List<HdTicketViewModel>();
            //List<HdTicketViewModel> allNonProjectTicketsModels = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                    models.Add(new HdTicketViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Summary = t.Summary,
                        HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                        HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                        Created = t.Created,
                        Modified = t.Modified,
                        Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                        Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                        HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                        HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                        HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                        Escalated = t.Escalated,
                        DueDate = t.DueDate,
                        //LastCommenter = _context.Users.FirstOrDefault(u => u.Id ==
                        //    commentsAndOwners
                        //    .First(cao => cao.Comment.HdTicketId == t.Id).Comment.UserId)
                    })
                    );

            staleTickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Modified = t.Modified,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                    //LastCommenter = _context.Users.FirstOrDefault(u => u.Id ==
                    //        commentsAndOwners
                    //        .First(cao => cao.Comment.HdTicketId == t.Id).Comment.UserId)
                })
            );

            testAllTickets.ForEach(t =>
                allTicketsModels.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Modified = t.Modified,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                    //LastCommenter = _context.Users.FirstOrDefault(u => u.Id ==
                    //        commentsAndOwners
                    //        .First(cao => cao.Comment.HdTicketId == t.Id).Comment.UserId)
                })
            );

            models = models.DistinctBy(m => m.Id).ToList();

            ViewBag.ITSOwners = itsOwners;

            Dictionary<string, int> usersAndTicketsThatNeedAttentionCounts = new();
            Dictionary<string, int> usersAndNotClosedTicketsCounts = new();
            Dictionary<string, int> usersAndNotClosedNonProjectTicketsCounts = new();

            foreach (User u in itsOwners)
            {
                usersAndTicketsThatNeedAttentionCounts.Add(u.UserName, models.Where(m =>
                //m.Owner != null && 
                (m.Owner == null ? "Unassigned" : m.Owner.UserName) == u.UserName
                //(m.Owner.UserName ?? "Unassigned") == u.UserName
                ).Count());

                usersAndNotClosedTicketsCounts.Add(u.UserName, allTicketsModels.Where(m =>
                //m.Owner != null && 
                (m.Owner == null ? "Unassigned" : m.Owner.UserName) == u.UserName
                //(m.Owner.UserName ?? "Unassigned") == u.UserName
                ).Count());

                usersAndNotClosedNonProjectTicketsCounts.Add(u.UserName, allTicketsModels.Where(m =>
                //m.Owner != null && 
                (m.Owner == null ? "Unassigned" : m.Owner.UserName) == u.UserName &&
                m.HdQueue.Name != "Project"
                //(m.Owner.UserName ?? "Unassigned") == u.UserName
                ).Count());
            }

            ViewBag.TicketsThatNeedAttentionCounts = usersAndTicketsThatNeedAttentionCounts;
            ViewBag.NotClosedTicketsCounts = usersAndNotClosedTicketsCounts;
            ViewBag.NotClosedNonProject = usersAndNotClosedNonProjectTicketsCounts;
            ViewBag.UserName = userName;

            //var testModels = models.Where(m => m.Owner != null &&
            //m.Owner.UserName != "mgates" &&
            //m.Owner.UserName != "kgosciniak" &&
            //m.Owner.UserName != "noah.burns").ToList();

            return
                userName == null ?
                View(models
                    .OrderBy(m => m.Created))
                :
                View(models
                    .Where(m => m.Owner == null || m.Owner.UserName == userName)
                    .OrderBy(m => m.Created));
        } // end Dashboard

        [Route("{controller}/{action}/{userName?}")]
        public async Task<IActionResult> NeedsAttention(string? userName)
        {
            var testTicket = _context.HdTickets.First(hdt => hdt.Id == 14990);

            List<long> itsOwnersLabel = _context.UserLabelJts
                .Where(ulj =>
                    ulj.LabelId == 90).Select(ulj => ulj.UserId).ToList();

            List<User> itsOwners = _context.Users
                .Where(u =>
                itsOwnersLabel.Contains(u.Id)).ToList();

            itsOwners.Add(new Models.KACE.User()
            {
                UserName = "Unassigned"
            });

            var notClosedStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed" && s.Name != "Resolved")
                        .Select(s => s.Id)
                        .ToList();

            var notProjectQueueList = _context.HdQueues
                .Where(hdq => hdq.Name != "Project").Select(hdq => hdq.Id);

            List<long> openTicketIDs = _context.
                HdTickets.Where(hdt =>
                    //notProjectQueueList.Contains(hdt.HdQueueId) &&
                    notClosedStatusList.Contains(hdt.HdStatusId))
                .Select(hdt => hdt.Id).ToList();

            //bool hasMyTicket = openTicketIDs.Contains(14990);

            IEnumerable<IGrouping<long?, HdTicketChange>> allComments =
                _context.HdTicketChanges
                .Where(
                    c =>
                    openTicketIDs.Contains((long)c.HdTicketId))
                .OrderByDescending(c => c.Timestamp).GroupBy(
                c => c.HdTicketId);

            IEnumerable<HdTicketChange> highestTimestampChanges = allComments
                .SelectMany(group => group)
                // Order the list by TimeStamp attribute in descending order
                .OrderByDescending(change => change.Timestamp)
                // Group by the key (long?) and take the first element from each group
                .GroupBy(change => change.HdTicketId)
                .Select(group => group.First());



            List<HdTicketChange> htcList = highestTimestampChanges
                .Where(
                    c =>
                    notClosedStatusList
                        .Contains(_context.HdTickets
                            .First(t =>
                                t.Id == c.HdTicketId).HdStatusId)
                    )
                .OrderBy(hL => hL.Timestamp)
                .ToList();

            //var hasMyTicketTest = htcList.OrderBy(cad => cad.HdTicketId).Select(htc => htc.HdTicketId).Contains(14990);

            List<CommentOwnerAndSubmitter> commentsAndOwners = new();

            htcList
                .ToList()
                .ForEach(
                htc =>
                    commentsAndOwners.Add(
                        new CommentOwnerAndSubmitter()
                        {
                            Comment = htc,
                            Owner = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .OwnerId,
                            Submitter = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .SubmitterId
                        }
                    ));

            //var myTestTicket = commentsAndOwners.First(cao => cao.Comment.HdTicketId == 14990);

            // create allTickets list here?

            var allTickets = commentsAndOwners;

            commentsAndOwners = commentsAndOwners.
                Where(cao => cao.Owner !=
                cao.Comment.UserId).ToList();

            var hasMyTicketComment = commentsAndOwners.OrderBy(cad => cad.Comment.HdTicketId).Select(cad => cad.Comment.HdTicketId).Contains(14990);

            List<HdTicket> tickets = new();
            List<HdTicket> testAllTickets = new();

            // add stale tickets
            List<HdTicket> staleTickets = _context.HdTickets.Where(hdt =>
                (notProjectQueueList.Contains(hdt.HdQueueId) &&
                notClosedStatusList.Contains(hdt.HdStatusId) &&
                hdt.Modified <= DateTime.Today.AddDays(-14))
                ||
                (!notProjectQueueList.Contains(hdt.HdQueueId) &&
                notClosedStatusList.Contains(hdt.HdStatusId) &&
                hdt.Modified <= DateTime.Today.AddDays(-30))
                ).ToList();

            //List<HdTicket> allTickets = _context.HdTickets.Where(hdt =>
            //    (notProjectQueueList.Contains(hdt.HdQueueId) &&
            //    notClosedStatusList.Contains(hdt.HdStatusId)))
            //    .ToList();

            commentsAndOwners
                .ForEach(
                cao =>
                    tickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId)));

            allTickets
                .ForEach(
                cao =>
                    testAllTickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId)));

            tickets.RemoveAll(t => t == null);

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();
            List<HdTicketViewModel> allTicketsModels = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                    models.Add(new HdTicketViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Summary = t.Summary,
                        HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                        HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                        Created = t.Created,
                        Modified = t.Modified,
                        Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                        Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                        HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                        HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                        HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                        Escalated = t.Escalated,
                        DueDate = t.DueDate,
                        //LastCommenter = _context.Users.FirstOrDefault(u => u.Id ==
                        //    commentsAndOwners
                        //    .First(cao => cao.Comment.HdTicketId == t.Id).Comment.UserId)
                    })
                    );

            staleTickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Modified = t.Modified,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                    //LastCommenter = _context.Users.FirstOrDefault(u => u.Id ==
                    //        commentsAndOwners
                    //        .First(cao => cao.Comment.HdTicketId == t.Id).Comment.UserId)
                })
            );

            testAllTickets.ForEach(t =>
                allTicketsModels.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Modified = t.Modified,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                    //LastCommenter = _context.Users.FirstOrDefault(u => u.Id ==
                    //        commentsAndOwners
                    //        .First(cao => cao.Comment.HdTicketId == t.Id).Comment.UserId)
                })
            );

            models = models.DistinctBy(m => m.Id).ToList();

            ViewBag.ITSOwners = itsOwners;

            Dictionary<string, int> usersAndTicketsThatNeedAttentionCounts = new();
            Dictionary<string, int> usersAndNotClosedTicketsCounts = new();

            foreach (User u in itsOwners)
            {
                usersAndTicketsThatNeedAttentionCounts.Add(u.UserName, models.Where(m =>
                //m.Owner != null && 
                (m.Owner == null ? "Unassigned" : m.Owner.UserName) == u.UserName
                //(m.Owner.UserName ?? "Unassigned") == u.UserName
                ).Count());

                usersAndNotClosedTicketsCounts.Add(u.UserName, allTicketsModels.Where(m =>
                //m.Owner != null && 
                (m.Owner == null ? "Unassigned" : m.Owner.UserName) == u.UserName
                //(m.Owner.UserName ?? "Unassigned") == u.UserName
                ).Count());
            }

            ViewBag.TicketsThatNeedAttentionCounts = usersAndTicketsThatNeedAttentionCounts;
            ViewBag.NotClosedTicketsCounts = usersAndNotClosedTicketsCounts;
            ViewBag.UserName = userName;

            var testModels = models.Where(m => m.Owner != null && 
            m.Owner.UserName != "mgates" &&
            m.Owner.UserName != "kgosciniak" &&
            m.Owner.UserName != "noah.burns").ToList();

            return
                userName == null ?
                View(models
                    .OrderBy(m => m.Created))
                :
                View(models
                    .Where(m => m.Owner == null || m.Owner.UserName == userName)
                    .OrderBy(m => m.Created));
        } // end NeedsAttention

        public async Task<IActionResult> WaitingOnManagement()
        {
            List<long> itsOwnersLabel = _context.UserLabelJts
                .Where(ulj =>
                    ulj.LabelId == 90).Select(ulj => ulj.UserId).ToList();

            List<User> itsOwners = _context.Users
                .Where(u =>
                itsOwnersLabel.Contains(u.Id)).ToList();

            var waitingOnManagementStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name == "Waiting on Management Input"
                        //&&
                        //s.Name != "Waiting on Submitter Input" &&
                        //s.Name != "Waiting on Vendor Input" &&
                        //s.Name != "Future Project"
                        )
                        .Select(s => s.Id)
                        .ToList();

            List<long> openTicketIDs = _context.
                HdTickets.Where(hdt =>
                    waitingOnManagementStatusList.Contains(hdt.HdStatusId))
                .Select(hdt => hdt.Id).ToList();

            IEnumerable<IGrouping<long?, HdTicketChange>> allComments =
                _context.HdTicketChanges
                .Where(
                    c =>
                    openTicketIDs.Contains((long)c.HdTicketId))
                .OrderByDescending(c => c.Timestamp).GroupBy(
                c => c.HdTicketId);

            IEnumerable<HdTicketChange> highestTimestampChanges = allComments
                .SelectMany(group => group)
                // Order the list by TimeStamp attribute in descending order
                .OrderByDescending(change => change.Timestamp)
                // Group by the key (long?) and take the first element from each group
                .GroupBy(change => change.HdTicketId)
                .Select(group => group.First());



            List<HdTicketChange> htcList = highestTimestampChanges
                .Where(
                    c =>
                    waitingOnManagementStatusList
                        .Contains(_context.HdTickets
                            .First(t =>
                                t.Id == c.HdTicketId).HdStatusId)
                    ).ToList();

            List<CommentOwnerAndSubmitter> commentsAndOwners = new();

            htcList
                .ToList()
                .ForEach(
                htc =>
                    commentsAndOwners.Add(
                        new CommentOwnerAndSubmitter()
                        {
                            Comment = htc,
                            Owner = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .OwnerId,
                            Submitter = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .SubmitterId
                        }
                    ));

            List<HdTicket> tickets = new();

            commentsAndOwners
                .ForEach(
                cao =>
                    tickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId)));

            tickets.RemoveAll(t => t == null);

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                    models.Add(new HdTicketViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Summary = t.Summary,
                        HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                        HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                        Created = t.Created,
                        Modified = t.Modified,
                        Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                        Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                        HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                        HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                        HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                        Escalated = t.Escalated,
                        DueDate = t.DueDate,
                    })
            );

            models = models.DistinctBy(m => m.Id).ToList();

            return
                View(models
                    .OrderBy(m => m.Created));
        }

        public async Task<IActionResult> WaitingOnVendor()
        {
            List<long> itsOwnersLabel = _context.UserLabelJts
                .Where(ulj =>
                    ulj.LabelId == 90).Select(ulj => ulj.UserId).ToList();

            List<User> itsOwners = _context.Users
                .Where(u =>
                itsOwnersLabel.Contains(u.Id)).ToList();

            var waitingOnManagementStatusList = _context.HdStatuses
                        .Where(s =>
                        s.Name == "Waiting on Vendor Input"
                        //&&
                        //s.Name != "Waiting on Submitter Input" &&
                        //s.Name != "Waiting on Vendor Input" &&
                        //s.Name != "Future Project"
                        )
                        .Select(s => s.Id)
                        .ToList();

            List<long> openTicketIDs = _context.
                HdTickets.Where(hdt =>
                    waitingOnManagementStatusList.Contains(hdt.HdStatusId))
                .Select(hdt => hdt.Id).ToList();

            IEnumerable<IGrouping<long?, HdTicketChange>> allComments =
                _context.HdTicketChanges
                .Where(
                    c =>
                    openTicketIDs.Contains((long)c.HdTicketId))
                .OrderByDescending(c => c.Timestamp).GroupBy(
                c => c.HdTicketId);

            IEnumerable<HdTicketChange> highestTimestampChanges = allComments
                .SelectMany(group => group)
                // Order the list by TimeStamp attribute in descending order
                .OrderByDescending(change => change.Timestamp)
                // Group by the key (long?) and take the first element from each group
                .GroupBy(change => change.HdTicketId)
                .Select(group => group.First());



            List<HdTicketChange> htcList = highestTimestampChanges
                .Where(
                    c =>
                    waitingOnManagementStatusList
                        .Contains(_context.HdTickets
                            .First(t =>
                                t.Id == c.HdTicketId).HdStatusId)
                    ).ToList();

            List<CommentOwnerAndSubmitter> commentsAndOwners = new();

            htcList
                .ToList()
                .ForEach(
                htc =>
                    commentsAndOwners.Add(
                        new CommentOwnerAndSubmitter()
                        {
                            Comment = htc,
                            Owner = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .OwnerId,
                            Submitter = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .SubmitterId
                        }
                    ));

            List<HdTicket> tickets = new();

            commentsAndOwners
                .ForEach(
                cao =>
                    tickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId)));

            tickets.RemoveAll(t => t == null);

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                    models.Add(new HdTicketViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Summary = t.Summary,
                        HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                        HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                        Created = t.Created,
                        Modified = t.Modified,
                        Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                        Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                        HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                        HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                        HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                        Escalated = t.Escalated,
                        DueDate = t.DueDate,
                    })
            );

            models = models.DistinctBy(m => m.Id).ToList();

            return
                View(models
                    .OrderBy(m => m.Created));
        }

        public async Task<IActionResult> MyTicketsThatNeedResponse()
        {
            string username = User.Identity.Name;
            string usernameWithoutDomain = username.Split('\\')[1];

            List<IGrouping<long?, HdTicketChange>> allComments =
                _context.HdTicketChanges
                .Where(
                    c =>
                    _context.HdStatuses
                        .Where(s =>
                        s.Name != "Closed"
                        )
                        .Select(s => s.Id)
                        .ToList().Contains
                    (
                    _context.HdTickets
                    .First(t =>
                        t.Id == c.HdTicketId &&
                        (t.SubmitterId == _context.Users.First(u => u.UserName == usernameWithoutDomain).Id)
                    ).HdStatusId
                    ))
                .OrderByDescending(c => c.Timestamp).GroupBy(
                c => c.HdTicketId).ToList();

            List<HdTicketChange> highestTimestampChanges = allComments
                // Flatten the list of IGrouping<long?, HdTicketChange> into a single list of HdTicketChange
                .SelectMany(group => group)
                // Order the list by TimeStamp attribute in descending order
                .OrderByDescending(change => change.Timestamp)
                // Group by the key (long?) and take the first element from each group
                .GroupBy(change => change.HdTicketId)
                .Select(group => group.First())
                //.Take(1000)
                // Convert the result to a list
                .ToList();

            //commentsAndOwners = commentsAndOwners.
            //    Where(cao => cao.Owner !=
            //    cao.Comment.UserId).ToList();

            List<CommentOwnerAndSubmitter> commentsAndOwners = new();

            highestTimestampChanges.ForEach(
                htc =>
                    commentsAndOwners.Add(
                        new CommentOwnerAndSubmitter()
                        {
                            Comment = htc,
                            Owner = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .OwnerId,
                            Submitter = _context.HdTickets
                            .First(t =>
                                t.Id == htc.HdTicketId)
                            .SubmitterId
                        }
                    ));

            commentsAndOwners = commentsAndOwners.
                Where(cao => cao.Submitter != cao.Comment.UserId).ToList();

            List<HdTicket> tickets = new();

            commentsAndOwners
                .ForEach(
                cao =>
                    tickets.Add(_context.HdTickets
                        .FirstOrDefault(hdt =>
                        hdt.Id == cao.Comment.HdTicketId
                    //&&
                    //(hdt.HdStatusId != _context.HdStatuses
                    //    .First(s => s.Name == "Closed").Id)
                    )));

            tickets.RemoveAll(t => t == null);

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            //return View(models);
            return View(models);
        }

        public async Task<IActionResult> MyTickets()
        {
            string username = User.Identity.Name;
            string usernameWithoutDomain = username.Split('\\')[1];

            List<HdTicket> tickets = _context.HdTickets
                .Where(hdt =>
                _context.HdStatuses.Where(s => s.Name != "Closed").Select(s => s.Id).ToList().Contains(hdt.HdStatusId) &&
                (_context.Users.First(u => u.Id == hdt.OwnerId).UserName == usernameWithoutDomain ||
                _context.Users.First(u => u.Id == hdt.SubmitterId).UserName == usernameWithoutDomain)
                )
                .OrderByDescending(t => t.Created)
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            tickets.RemoveAll(m => m.OwnerId == 0);
            tickets.RemoveAll(m => m.SubmitterId == 0);

            //return View(models);
            return View(models);
        }

        [Route("{controller}/{action}")]
        public async Task<IActionResult> MyDepartmentTickets()
        {
            string username = User.Identity.Name;
            string usernameWithoutDomain = username.Split('\\')[1];

            // find userID in KACE
            long myUserId = _context.Users.First(u => u.UserName == usernameWithoutDomain).Id;
            List<long> labelIds = _context.UserLabelJts.Where(ulj => ulj.UserId == myUserId)
                .Select(u => u.LabelId).ToList();

            List<long> coworkerIds = _context.UserLabelJts
                .Where(ulj =>
                    labelIds.Contains(ulj.LabelId)).Select(ulj => ulj.UserId).ToList();

            List<User> coworkers = _context.Users
                .Where(u =>
                coworkerIds.Contains(u.Id)).ToList();

            List<HdTicket> tickets = _context.HdTickets
                .Where(hdt =>
                (coworkerIds.Contains(hdt.OwnerId) || coworkerIds.Contains(hdt.SubmitterId)) &&
                _context.HdStatuses.Where(s => s.Name != "Closed").Select(s => s.Id)
                .ToList().Contains(hdt.HdStatusId))
                .OrderByDescending(t => t.Created)
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate,
                })
                );

            tickets.RemoveAll(m => m.OwnerId == 0);
            tickets.RemoveAll(m => m.SubmitterId == 0);

            //return View(models);
            return View(models);
        }

        [Route("{controller}/{action}/{departmentId}")]
        public async Task<IActionResult> ByDepartmentId(long departmentId)
        {
            string username = User.Identity.Name;
            string usernameWithoutDomain = username.Split('\\')[1];

            List<long> coworkerIds = _context.UserLabelJts
                .Where(ulj =>
                    ulj.LabelId == departmentId).Select(ulj => ulj.UserId).ToList();

            List<User> coworkers = _context.Users
                .Where(u =>
                coworkerIds.Contains(u.Id)).ToList();

            List<HdTicket> tickets = _context.HdTickets
                .Where(hdt =>
                (coworkerIds.Contains(hdt.OwnerId) || coworkerIds.Contains(hdt.SubmitterId)) &&
                _context.HdStatuses.Where(s => s.Name != "Closed").Select(s => s.Id)
                .ToList().Contains(hdt.HdStatusId))
                .OrderByDescending(t => t.Created)
                .ToList();

            List<HdTicketViewModel> models = new List<HdTicketViewModel>();

            tickets.ForEach(t =>
                models.Add(new HdTicketViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Summary = t.Summary,
                    HdPriority = _context.HdPriorities.FirstOrDefault(p => p.Id == t.HdPriorityId),
                    HdImpact = _context.HdImpacts.FirstOrDefault(i => i.Id == t.HdImpactId),
                    Created = t.Created,
                    Owner = _context.Users.FirstOrDefault(u => u.Id == t.OwnerId),
                    Submitter = _context.Users.FirstOrDefault(u => u.Id == t.SubmitterId),
                    HdStatus = _context.HdStatuses.FirstOrDefault(hds => hds.Id == t.HdStatusId),
                    HdQueue = _context.HdQueues.FirstOrDefault(hdq => hdq.Id == t.HdQueueId),
                    HdCategory = _context.HdCategories.FirstOrDefault(hdc => hdc.Id == t.HdCategoryId),
                    Escalated = t.Escalated,
                    DueDate = t.DueDate
                })
                );

            tickets.RemoveAll(m => m.OwnerId == 0);
            tickets.RemoveAll(m => m.SubmitterId == 0);

            var departments = _context.UserLabelJts
                .Select(ulj => ulj.LabelId).ToList()
                .Distinct().ToList();

            var departmentNames = _context.Labels
                .Where(label => departments.Contains(label.Id))
                .ToList();

            ViewBag.Departments = departmentNames;

            //return View(models);
            return View(models);
        }

        // GET: HdTickets/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            // TODO: Add list of comments (Hd_Ticket_Change(s)) to ticket detail
            if (id == null)
            {
                return NotFound();
            }

            var hdTicket = await _context.HdTickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hdTicket == null)
            {
                return NotFound();
            }

            Tuple<HdTicket, List<HdTicketChange>> ticketAndComments =
                new(hdTicket,
                    _context.HdTicketChanges.Where(hdc => hdc.HdTicketId == hdTicket.Id).ToList());

            return View(ticketAndComments);
        }

        // GET: HdTickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HdTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Summary,HdPriorityId,HdImpactId,Modified,Created,OwnerId,SubmitterId,HdStatusId,HdQueueId,HdCategoryId,CcList,Escalated,CustomFieldValue0,CustomFieldValue1,CustomFieldValue2,CustomFieldValue3,CustomFieldValue4,CustomFieldValue5,CustomFieldValue6,CustomFieldValue7,CustomFieldValue8,CustomFieldValue9,CustomFieldValue10,CustomFieldValue11,CustomFieldValue12,CustomFieldValue13,CustomFieldValue14,DueDate,IsManualDueDate,SlaNotified,TimeOpened,TimeClosed,TimeStalled,MachineId,SatisfactionRating,SatisfactionComment,Resolution,AssetId,ParentId,IsParent,ApproverId,ApproveState,Approval,ApprovalNote,ServiceTicketId,HdServiceStatusId,HdUseProcessStatus,HtmlSummary,TicketTemplateId,EmailMessageId")] HdTicket hdTicket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hdTicket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hdTicket);
        }

        // GET: HdTickets/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hdTicket = await _context.HdTickets.FindAsync(id);
            if (hdTicket == null)
            {
                return NotFound();
            }
            return View(hdTicket);
        }

        // POST: HdTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Summary,HdPriorityId,HdImpactId,Modified,Created,OwnerId,SubmitterId,HdStatusId,HdQueueId,HdCategoryId,CcList,Escalated,CustomFieldValue0,CustomFieldValue1,CustomFieldValue2,CustomFieldValue3,CustomFieldValue4,CustomFieldValue5,CustomFieldValue6,CustomFieldValue7,CustomFieldValue8,CustomFieldValue9,CustomFieldValue10,CustomFieldValue11,CustomFieldValue12,CustomFieldValue13,CustomFieldValue14,DueDate,IsManualDueDate,SlaNotified,TimeOpened,TimeClosed,TimeStalled,MachineId,SatisfactionRating,SatisfactionComment,Resolution,AssetId,ParentId,IsParent,ApproverId,ApproveState,Approval,ApprovalNote,ServiceTicketId,HdServiceStatusId,HdUseProcessStatus,HtmlSummary,TicketTemplateId,EmailMessageId")] HdTicket hdTicket)
        {
            if (id != hdTicket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hdTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HdTicketExists(hdTicket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hdTicket);
        }

        // GET: HdTickets/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hdTicket = await _context.HdTickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hdTicket == null)
            {
                return NotFound();
            }

            return View(hdTicket);
        }

        [HttpGet("HdTickets/ByMonth/{monthsBack?}")]
        public IActionResult ByMonth(int? monthsBack)
        {
            int monthsBackNotNull = monthsBack ?? 6;
            int monthsBackMinusOne = monthsBackNotNull - 1;
            var firstDayOfMonth =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Dictionary<string, List<HdTicket>[]> statusesAndTickets = new();

            List<HdTicket>[] hdTickets = new List<HdTicket>[monthsBackNotNull];

            List<HdTicket>[] openHdTickets = new List<HdTicket>[monthsBackNotNull];

            List<HdTicket>[] closedHdTickets = new List<HdTicket>[monthsBackNotNull];

            List<HdStatus> openedAndNewStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Opened" ||
                    hds.Name == "New").ToList();

            List<HdStatus> closedStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Closed").ToList();

            var lastWeekTickets = _context.HdTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday && 
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= firstDayOfMonth.AddMonths(-1 * monthsBackMinusOne)
                ).ToList();

            for (int i = monthsBackMinusOne; i > -1; i--)
            {
                hdTickets[i] = lastWeekTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= firstDayOfMonth.AddMonths(i * (-1)) &&
                hdt.Created <= firstDayOfMonth.AddMonths(i * (-1) + 1)).ToList();

                openHdTickets[i] = lastWeekTickets.Where(hdt =>
                openedAndNewStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= firstDayOfMonth.AddMonths(i * (-1)) &&
                hdt.Created <= firstDayOfMonth.AddMonths(i * (-1) + 1)).ToList();

                closedHdTickets[i] = lastWeekTickets.Where(hdt =>
                closedStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= firstDayOfMonth.AddMonths(i * (-1)) &&
                hdt.Created <= firstDayOfMonth.AddMonths(i * (-1) + 1)).ToList();
            }

            statusesAndTickets.Add("All", hdTickets);
            statusesAndTickets.Add("Opened", openHdTickets);
            statusesAndTickets.Add("Closed", closedHdTickets);

            int total = 0;

            for (int i = 0; i < monthsBackNotNull; i++)
            {
                total +=
                    //statusesAndTickets["All"][i].Count() -
                    //statusesAndTickets["Closed"][i].Count();
                    statusesAndTickets["All"][i].Count();
            }

            int remainingOpen = 0;

            for (int i = 0; i < monthsBackNotNull; i++)
            {
                remainingOpen +=
                    //statusesAndTickets["All"][i].Count() -
                    //statusesAndTickets["Closed"][i].Count();
                    statusesAndTickets["Opened"][i].Count();
            }

            int closed = 0;

            for (int i = 0; i < monthsBackNotNull; i++)
            {
                if (statusesAndTickets["Closed"][i] == null)
                {

                }
                else
                {
                    closed +=
                        //statusesAndTickets["All"][i].Count() -
                        //statusesAndTickets["Closed"][i].Count();
                        statusesAndTickets["Closed"][i].Count();
                }
            }

            var avgAgeOfOpenTickets = _context.HdTickets
                .Where(hdt =>
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .Average(hdt => (DateTime.Today - hdt.Created).Days);

            var oldestOpenTicket = _context.HdTickets
                .Where(hdt =>
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .OrderBy(hdt => hdt.Created)
                .First();

            ViewBag.OldestTicketID = oldestOpenTicket!.Id;
            ViewBag.OldestTicketDate = oldestOpenTicket!.Created;
            ViewBag.AverageDaysOpened = (int)avgAgeOfOpenTickets;
            ViewBag.Total = total;
            ViewBag.RemainingOpen = remainingOpen;
            ViewBag.Closed = closed;
            ViewBag.MonthsBack = monthsBackNotNull;
            ViewBag.MonthsBackMinusOne = monthsBackMinusOne;
            ViewBag.MaxTickets = statusesAndTickets["All"].Max(l => l.Count);
            double heightRatio = 15.0 / (ViewBag.MaxTickets);
            ViewBag.HeightRatio = heightRatio;
            return View(statusesAndTickets);
        }

        [HttpGet("HdTickets/ByWeek/{weeksBack?}")]
        public IActionResult ByWeek(int? weeksBack)
        {
            int weeksBackNotNull = (weeksBack ?? 4);
            int weeksBackMinusOne = (weeksBackNotNull - 1); // change parenthesis?
            var firstDayOfMonth =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Dictionary<string, List<HdTicket>[]> statusesAndTickets = new();

            List<HdTicket>[] hdTickets = new List<HdTicket>[weeksBackNotNull];

            List<HdTicket>[] openHdTickets = new List<HdTicket>[weeksBackNotNull];

            List<HdTicket>[] closedHdTickets = new List<HdTicket>[weeksBackNotNull];

            List<HdStatus> openedAndNewStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Opened" ||
                    hds.Name == "New").ToList();

            List<HdStatus> closedStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Closed").ToList();

            var lastWeekTickets = _context.HdTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday && 
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(-7 * weeksBackMinusOne)
                ).ToList();

            for (int i = weeksBackMinusOne; i > -1; i--)
            {
                hdTickets[i] = lastWeekTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-7)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-7) + 7)).ToList();

                openHdTickets[i] = lastWeekTickets.Where(hdt =>
                openedAndNewStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-7)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-7) + 7)).ToList();

                closedHdTickets[i] = lastWeekTickets.Where(hdt =>
                closedStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-7)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-7) + 7)).ToList();
            }

            statusesAndTickets.Add("All", hdTickets);
            statusesAndTickets.Add("Opened", openHdTickets);
            statusesAndTickets.Add("Closed", closedHdTickets);

            int total = 0;

            for (int i = 0; i < weeksBackNotNull; i++)
            {
                total +=
                    //statusesAndTickets["All"][i].Count() -
                    //statusesAndTickets["Closed"][i].Count();
                    statusesAndTickets["All"][i].Count();
            }

            int remainingOpen = 0;

            for (int i = 0; i < weeksBackNotNull; i++)
            {
                if (statusesAndTickets["Opened"][i] == null)
                {

                }
                else
                {
                    remainingOpen +=
                        //statusesAndTickets["All"][i].Count() -
                        //statusesAndTickets["Closed"][i].Count();
                        statusesAndTickets["Opened"][i].Count();
                }
            }

            int closed = 0;

            for (int i = 0; i < weeksBackNotNull; i++)
            {
                if (statusesAndTickets["Closed"][i] == null)
                {

                }
                else
                {
                    closed +=
                        //statusesAndTickets["All"][i].Count() -
                        //statusesAndTickets["Closed"][i].Count();
                        statusesAndTickets["Closed"][i].Count();
                }
            }

            var avgAgeOfOpenTickets = _context.HdTickets
                .Where(hdt =>
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .Average(hdt => (DateTime.Today - hdt.Created).Days);

            var oldestOpenTicket = _context.HdTickets
                .Where(hdt =>
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .OrderBy(hdt => hdt.Created)
                .First();

            ViewBag.OldestTicketID = oldestOpenTicket!.Id;
            ViewBag.OldestTicketDate = oldestOpenTicket!.Created;
            ViewBag.AverageDaysOpened = (int)avgAgeOfOpenTickets;
            ViewBag.Total = total;
            ViewBag.RemainingOpen = remainingOpen;
            ViewBag.Closed = closed;
            ViewBag.WeeksBack = weeksBackNotNull;
            ViewBag.WeeksBackMinusOne = weeksBackMinusOne;
            ViewBag.MaxTickets = statusesAndTickets["All"].Max(l => l.Count);
            double heightRatio = 15.0 / (ViewBag.MaxTickets);
            ViewBag.HeightRatio = heightRatio;
            return View(statusesAndTickets);
        }

        [HttpGet("HdTickets/ByDay/{daysBack?}")]
        public IActionResult ByDay(int? daysBack)
        {
            int daysBackNotNull = daysBack ?? 8;
            int daysBackMinusOne = daysBackNotNull - 1;
            int highestTicketCount = 0;
            Dictionary<string, List<HdTicket>[]> statusesAndTickets = new();

            List<HdTicket>[] hdTickets = new List<HdTicket>[daysBackNotNull];

            List<HdTicket>[] openHdTickets = new List<HdTicket>[daysBackNotNull];

            List<HdTicket>[] closedHdTickets = new List<HdTicket>[daysBackNotNull];

            List<HdStatus> openedAndNewStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Opened" ||
                    hds.Name == "New").ToList();

            List<HdStatus> closedStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Closed").ToList();

            List<HdStatus> notClosedStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name != "Closed").ToList();

            var lastWeekTickets = _context.HdTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(-1 * daysBackMinusOne)
                ).ToList();

            for (int i = daysBackMinusOne; i > -1; i--)
            {
                hdTickets[i] = lastWeekTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-1)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-1) + 1)).ToList();

                openHdTickets[i] = lastWeekTickets.Where(hdt =>
                openedAndNewStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-1)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-1) + 1)).ToList();

                closedHdTickets[i] = lastWeekTickets.Where(hdt =>
                closedStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-1)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-1) + 1)).ToList();

                highestTicketCount =
                    hdTickets[i].Count > highestTicketCount ?
                    hdTickets[i].Count
                    :
                    highestTicketCount;
            }

            statusesAndTickets.Add("All", hdTickets);
            statusesAndTickets.Add("Opened", openHdTickets);
            statusesAndTickets.Add("Closed", closedHdTickets);

            int total = 0;

            for (int i = 0; i < daysBackNotNull; i++)
            {
                total +=
                    //statusesAndTickets["All"][i].Count() -
                    //statusesAndTickets["Closed"][i].Count();
                    statusesAndTickets["All"][i].Count();
            }

            int remainingOpen = 0;

            for (int i = 0; i < daysBackNotNull; i++)
            {
                remainingOpen +=
                    statusesAndTickets["Opened"][i].Count();
            }

            int closed = 0;

            for (int i = 0; i < daysBackNotNull; i++)
            {
                if (statusesAndTickets["Closed"][i] == null)
                {

                }
                else
                {
                    closed +=
                        //statusesAndTickets["All"][i].Count() -
                        //statusesAndTickets["Closed"][i].Count();
                        statusesAndTickets["Closed"][i].Count();
                }
            }

            var avgAgeOfOpenTickets = _context.HdTickets
                .Where(hdt =>
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .Average(hdt => (DateTime.Today - hdt.Created).Days);

            var oldestOpenTicket = _context.HdTickets
                .Where(hdt =>
                    !(_context.HdQueues.First(
                        s =>
                            s.Id == hdt.HdQueueId).Name.Contains("Project")) &&
                    notClosedStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .OrderBy(hdt => hdt.Created)
                .First();

            ViewBag.OldestTicketID = oldestOpenTicket!.Id;
            ViewBag.OldestTicketDate = oldestOpenTicket!.Created;
            ViewBag.AverageDaysOpened = (int)avgAgeOfOpenTickets;
            ViewBag.Total = total;
            ViewBag.Closed = closed;
            ViewBag.RemainingOpen = remainingOpen;
            ViewBag.DaysBack = daysBackNotNull;
            ViewBag.DaysBackMinusOne = daysBackMinusOne;
            ViewBag.HighestTicketCount = highestTicketCount;
            return View(statusesAndTickets);
        }

        [HttpGet("HdTickets/ByUser/{userName}/{daysBack?}")]
        public IActionResult ByUser(string userName, int? daysBack)
        {
            int daysBackNotNull = daysBack ?? 8;
            int daysBackMinusOne = daysBackNotNull - 1;
            Dictionary<string, List<HdTicket>[]> statusesAndTickets = new();

            List<HdTicket>[] hdTickets = new List<HdTicket>[daysBackNotNull];

            List<HdTicket>[] openHdTickets = new List<HdTicket>[daysBackNotNull];

            List<HdTicket>[] closedHdTickets = new List<HdTicket>[daysBackNotNull];

            List<HdStatus> openedAndNewStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Opened" ||
                    hds.Name == "New").ToList();

            List<HdStatus> closedStatuses =
                _context.HdStatuses.Where(hds =>
                    hds.Name == "Closed").ToList();

            List<long> itsOwnersLabel = _context.UserLabelJts
                .Where(ulj =>
                    ulj.LabelId == 90).Select(ulj => ulj.UserId).ToList();

            List<User> itsOwners = _context.Users
                .Where(u =>
                itsOwnersLabel.Contains(u.Id)).ToList();

            ViewBag.ITSOwners = itsOwners;

            User user = _context.Users.First(u => u.UserName == userName);

            var lastWeekTickets = _context.HdTickets.Where(hdt =>
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(-1 * daysBackMinusOne)
                && hdt.OwnerId == user.Id
                ).ToList();

            for (int i = daysBackMinusOne; i > -1; i--)
            {
                hdTickets[i] = lastWeekTickets.Where(hdt =>
                hdt.OwnerId == user.Id &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-1)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-1) + 1)).ToList();

                openHdTickets[i] = lastWeekTickets.Where(hdt =>
                hdt.OwnerId == user.Id &&
                openedAndNewStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-1)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-1) + 1)).ToList();

                closedHdTickets[i] = lastWeekTickets.Where(hdt =>
                hdt.OwnerId == user.Id &&
                closedStatuses
                    .Select(hds => hds.Id)
                    .Contains(hdt.HdStatusId) &&
                //hdt.Created.DayOfWeek != DayOfWeek.Sunday &&
                //hdt.Created.DayOfWeek != DayOfWeek.Saturday &&
                hdt.Created >= DateTime.Today.AddDays(i * (-1)) &&
                hdt.Created <= DateTime.Today.AddDays(i * (-1) + 1)).ToList();
            }

            statusesAndTickets.Add("All" + userName, hdTickets);
            statusesAndTickets.Add("Opened" + userName, openHdTickets);
            statusesAndTickets.Add("Closed" + userName, closedHdTickets);

            int total = 0;

            for (int i = 0; i < daysBackNotNull; i++)
            {
                total +=
                    //statusesAndTickets["All"][i].Count() -
                    //statusesAndTickets["Closed"][i].Count();
                    statusesAndTickets["All" + userName][i].Count();
            }

            int remainingOpen = 0;

            for (int i = 0; i < daysBackNotNull; i++)
            {
                remainingOpen +=
                    statusesAndTickets["Opened" + userName][i].Count();
            }

            int closed = 0;

            for (int i = 0; i < daysBackNotNull; i++)
            {
                if (statusesAndTickets["Closed" + userName][i] == null)
                {

                }
                else
                {
                    closed +=
                        //statusesAndTickets["All"][i].Count() -
                        //statusesAndTickets["Closed"][i].Count();
                        statusesAndTickets["Closed" + userName][i].Count();
                }
            }

            var openTickets = _context.HdTickets
                .Where(hdt =>
                    hdt.OwnerId == user.Id &&
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList();

            var avgAgeOfOpenTickets = openTickets.Count == 0 ? 0 :
                openTickets.Average(hdt => (DateTime.Today - hdt.Created).Days);

            var oldestOpenTicket =
                openTickets.Count == 0 ? null :
                _context.HdTickets
                .Where(hdt =>
                    hdt.OwnerId == user.Id &&
                    openedAndNewStatuses.Select(s => s.Id)
                    .ToList()
                    .Contains(
                        hdt.HdStatusId))
                .ToList()
                .OrderBy(hdt => hdt.Created)
                .First();

            ViewBag.OldestTicketID = oldestOpenTicket == null ? 0 : oldestOpenTicket.Id;
            ViewBag.OldestTicketDate = oldestOpenTicket == null ? DateTime.Now : oldestOpenTicket.Created;
            ViewBag.AverageDaysOpened = (int)avgAgeOfOpenTickets;
            ViewBag.Total = total;
            ViewBag.Closed = closed;
            ViewBag.RemainingOpen = remainingOpen;
            ViewBag.DaysBack = daysBackNotNull;
            ViewBag.DaysBackMinusOne = daysBackMinusOne;
            ViewBag.UserName = userName;
            return View(statusesAndTickets);
        }

        // POST: HdTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hdTicket = await _context.HdTickets.FindAsync(id);
            if (hdTicket != null)
            {
                _context.HdTickets.Remove(hdTicket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HdTicketExists(long id)
        {
            return _context.HdTickets.Any(e => e.Id == id);
        }
    }
}
