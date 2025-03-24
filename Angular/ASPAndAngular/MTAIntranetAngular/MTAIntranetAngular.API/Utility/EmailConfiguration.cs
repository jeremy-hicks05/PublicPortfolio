using System.Net.Mail;
using System.Net;
using System.Text;
using MTAIntranetAngular.API.Data.Models;
using static HotChocolate.ErrorCodes;
using System.Diagnostics;

namespace MTAIntranetAngular.Utility
{
    public static class EmailConfiguration
    {
        public static string? From { get; set; }
        public static string? SmtpServer { get; set; }
        public static int Port { get; set; }
        public static string? UserName { get; set; }
        public static string? Password { get; set; }

        private static IConfiguration? _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void SendApprovalRequestToManager(Ticket ticket)
        {
            string recipients = "";
            recipients += "email..."; // REMOVE IF NOT TESTING
            //recipients += GetManagerEmailFromDisplayName(ticket.EnteredByUser[10..]);

            List<string> recipientsArray = recipients.Split(',').ToList();
            recipientsArray = recipientsArray.Distinct().ToList();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            for (int i = 0; i < recipientsArray.Count; i++)
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.office365.com";
                MyMail.Port = 587;
                MyMail.EnableSsl = true;
                MyMsg.Priority = MailPriority.Normal;
                MyMsg.To.Add(new MailAddress(recipientsArray[i].Trim('\'')));
                MyMsg.Subject = "Ticket Approval Request: " +
                    (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + " " +
                    (ticket.Category != null ? ticket.Category.Name : "Null Category") +
                    " ticket from Intranet: " +
                    ticket.DateEntered.ToString("MM/dd/yyyy hh:mm tt");
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;
                MyMsg.From = SetMailAddress();
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = ticket.Summary + "<br />" +
                    //"https://mtadev.mta-flint.net/Tickets/" + ticket.TicketId + "<br />" +
                    @"https://mtadev.mta-flint.net/mtaIntranet#/ticket/" + ticket.TicketId + "<br />" +
                    "Impact: " +
                    (ticket.Impact != null ? ticket.Impact.Description : "Null Impact") + "<br />" +
                    "Approval State: " +
                    (ticket.ApprovalState != null ? ticket.ApprovalState.Name : "Null Approval State") + "<br />" +
                    "Entered by User: " + ticket.EnteredByUser + "<br />" +
                    "Ticket Type: " +
                    (ticket.Category != null ? ticket.Category.Name : "Null Category") + "<br />" +
                    "Ticket SubType: " +
                    (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + "<br />";

                //MyMsg.Body += "<a href=https://mtadev.mta-flint.net/Tickets/ApproveTicket/" + ticket.TicketId + ">Approve / Reject Ticket</a>";
                MyMsg.Body += @"<a href=https://mtadev.mta-flint.net/mtaIntranet#/ticket/" + ticket.TicketId + ">View Ticket</a>";

                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = SetCredentials();
                MyMail.Credentials = MyCredentials;
                MyMail.Send(MyMsg);
            }
        }

        public static void SendTicketInfoTo(Ticket ticket)
        {
            string recipients = "";
            recipients += "email..."; // REMOVE IF NOT TESTING
            //recipients += ticket.SubType.Cclist;

            if (ticket.ImpactId == 2 || ticket.ImpactId == 3)
            {
                //recipients += "," + GetManagerEmailFromDisplayName(ticket.EnteredByUser[10..]);
            }

            List<string> recipientsArray = recipients.Split(',').ToList();
            recipientsArray = recipientsArray.Distinct().ToList();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            for (int i = 0; i < recipientsArray.Count; i++)
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.office365.com";
                MyMail.Port = 587;
                MyMail.EnableSsl = true;
                MyMsg.Priority = MailPriority.Normal;
                MyMsg.To.Add(new MailAddress(recipientsArray[i].Trim('\'')));
                MyMsg.Subject = "Notice: " +
                    (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + " " +
                    (ticket.Category != null ? ticket.Category.Name : "Null Category") +
                    " ticket from Intranet: " +
                    ticket.DateEntered.ToString("MM/dd/yyyy hh:mm tt");
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;
                MyMsg.From = SetMailAddress();
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = ticket.Summary + "<br />" +
                    //"https://mtadev.mta-flint.net/Tickets/" + ticket.TicketId + "<br />" +
                    @"https://mtadev.mta-flint.net/mtaIntranet#/ticket/" + ticket.TicketId + "<br />" +
                    "Impact: " +
                    (ticket.Impact != null ? ticket.Impact.Description : "Null Impact") + "<br />" +
                    "Approval State: " +
                    (ticket.ApprovalState != null ? ticket.ApprovalState.Name : "Null ApprovalState") + "<br />" +
                    "Entered by User: " + ticket.EnteredByUser + "<br />" +
                    "Ticket Type: " +
                    (ticket.Category != null ? ticket.Category.Name : "Null Category") + "<br />" +
                    "Ticket SubType: " +
                    (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + "<br />";
                MyMsg.Body += "New Manager needs: <br />";
                MyMsg.Body += FillInManagerRequirements(ticket.SubType!.Name) + "<br />";

                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = SetCredentials();
                MyMail.Credentials = MyCredentials;
                MyMail.Send(MyMsg);
            }
        }
        //private static string GetManagerEmailFromDisplayName(string userName)
        //{
        //    string managerEmail = "";

        //    string domainName = "MTA-FLINT.NET";

        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //    {
        //        using (var de = new DirectoryEntry("LDAP://" + domainName))
        //        {
        //            using (var adSearch = new DirectorySearcher(de))
        //            {
        //                // Get user from active directory.
        //                adSearch.Filter = "(sAMAccountName=" + userName.Trim().ToLower(CultureInfo.CurrentCulture) + ")";
        //                adSearch.PropertiesToLoad.Add("manager");
        //                var adSearchResult = adSearch.FindOne();
        //                if (adSearchResult == null)
        //                {
        //                    return userName + " not found - found ";
        //                }
        //                else
        //                {
        //                    managerEmail = GetUsersManagersEmail(adSearchResult.Properties["manager"][0]!.ToString()!.Split(',')[0].Remove(0, 3)); ;
        //                }
        //            }
        //        }
        //    }
        //    return managerEmail;
        //}

        //public static string GetUsersManagersEmail(string userDisplayName)
        //{
        //    string response = "";
        //    string domainName = "MTA-FLINT.NET";

        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //    {
        //        using (var de = new DirectoryEntry("LDAP://" + domainName))
        //        {
        //            using (var adSearch = new DirectorySearcher(de))
        //            {
        //                // Get user from active directory.
        //                adSearch.Filter = "(displayName=" + userDisplayName.Trim().ToLower(CultureInfo.CurrentCulture) + ")";
        //                var adSearchResult = adSearch.FindOne();
        //                if (adSearchResult == null)
        //                {
        //                    return userDisplayName + " not found - found ";
        //                }
        //                else
        //                {
        //                    var entry = adSearchResult.GetDirectoryEntry();

        //                    response += entry.Properties["mail"][0];
        //                }
        //            }
        //        }
        //    }
        //    return response;
        //}

        public static void SendEmailToKACE(Ticket ticket)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.Subject =
                (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + " " +
                (ticket.Category != null ? ticket.Category.Name : "Null Category") + " ticket from Intranet: " +
                ticket.DateEntered.ToString("MM/dd/yyyy hh:mm tt");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body =
                "@category=" +
                (ticket.Category != null ? ticket.Category.Name : "Null Category") + " <br />" +
                "@cc_list=" +
                (ticket.SubType != null ? ticket.SubType.Cclist : "Null CCList") + " <br />" +
                "@impact=" +
                (ticket.Impact != null ? ticket.Impact.Description : "Null Impact") + " <br />" +
                "@submitter=" + ((ticket.ApprovedBy == "NA" || ticket.ApprovedBy == null) ? ticket.EnteredByUser[10..] : ticket.ApprovedBy[10..]) + " <br />";
            if (ticket.Category != null && ticket.Category.Name == "New Manager"
                && ticket.SubType != null)
            {
                MyMsg.Body += "New Manager needs: <br />";
                MyMsg.Body += FillInManagerRequirements(ticket.SubType.Name) + "<br />";
            }
            MyMsg.Body += ticket.Summary.Replace("_", "<br />") + " <br />";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static string FillInManagerRequirements(string department)
        {
            string commonRequirements = "<li>AD Login </li>" +
                        "<li>PC </li>" +
                        "<li>Phone Extension </li> " +
                        "<li>Kronos Login </li>" +
                        "<li>Email Address </li>" +
                        "<li>Distribution Lists </li>";

            switch (department)
            {
                case "Your Ride":
                    return "<ul>" + commonRequirements + "</ul>";
                case "Fixed Route":
                    return "<ul>" + commonRequirements +
                        "<li>EAM Login </li>" +
                        "</ul>";
                case "RTW":
                    return "<ul>" + commonRequirements +
                        "</ul>";
                case "Maintenance":
                    return "<ul>" + commonRequirements +
                        "<li>EAM Login </li>" +
                        "</ul>";
                case "Finance":
                    return "<ul>" + commonRequirements +
                        "<li>Sage</li>" +
                        "</ul>";
                case "Purchsing":
                    return "<ul>" + commonRequirements +
                        "</ul>";
                case "Customer Service":
                    return "<ul>" + commonRequirements +
                        "</ul>";
                case "IT":
                    return "<ul>" + commonRequirements +
                        "</ul>";
                case "Planning":
                    return "<ul>" + commonRequirements +
                        "<li>EAM Login </li>" +
                        "</ul>";
                default:
                    return "<ul>" + commonRequirements + "</ul>";
            }
        }

        public static void SendRejectionNotice(Ticket ticket)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add(ticket.SubType?.Cclist ?? "email...");  // TESTING
            MyMsg.Subject = "Ticket " +
                (ticket.ApprovalState != null ? ticket.ApprovalState.Name : "Null ApprovalState") + ": " +
                (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + " " +
                (ticket.Category != null ? ticket.Category.Name : "Null Category") +
                " ticket from Intranet: " +
                ticket.DateEntered.ToString("MM/dd/yyyy hh:mm tt");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body =
                //"https://mtadev.mta-flint.net/Tickets/" + ticket.TicketId + "<br />" +
                @"https://mtadev.mta-flint.net/mtaIntranet#/ticket/" + ticket.TicketId + "<br />" +
                "Category: " + (ticket.Category != null ? ticket.Category.Name : "Null Category") + " <br />" +
                "Impact: " + (ticket.Impact != null ? ticket.Impact.Description : "Null Impact") + " <br />" +
                "SubType: " + (ticket.SubType != null ? ticket.SubType.Name : "Null Subtype") + " <br />" +
                "Description: " + (ticket.SubType != null ? ticket.SubType.Description : "Null Subtype") + " <br />" +
                ((ticket.ApprovalState != null ? ticket.ApprovalState.Name : "Null ApprovalState") == "Approved" ?
                "Approved "
                : "Reason for rejection: ") + ticket.ReasonForRejection;
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        // servers, services, processes, website, and sqltable health checks
        public static void SendServerInitSuccess(API.Data.Models.Server server)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(server.Recipients ?? "email...");
            MyMsg.Subject = "Server " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " connected and monitoring...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Connection to " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " successful.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServerFailure(API.Data.Models.Server server)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(server.Recipients ?? "email...");
            MyMsg.Subject = "Server " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " is not responding";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please resolve the problem with " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName;
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServerRestored(API.Data.Models.Server server)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(server.Recipients ?? "email...");
            MyMsg.Subject = "Server " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " has been restored.";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please confirm that " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " has been restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServiceInitSuccess(Service service)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(service.Recipients ?? "email...");
            ////https://mtadev.mta-flint.net:40443/Websites
            MyMsg.Subject = "Service " +
                (service.ServiceName ?? "NoServiceName") +
            " on server " +
                (service.ServerName ?? "NoServerName") + " FriendlyName: " + service.FriendlyName +
                " connected and monitoring...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Sevice " +
                (service.ServiceName ?? "NoServiceName") + " on " +
                (service.ServerName ?? "NoServerName") +
                " connection successful.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }
        public static void SendServiceFailure(Service service)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(service.Recipients ?? "email...");
            //https://mtadev.mta-flint.net:40443/Websites
            MyMsg.Subject = "Service " +
                (service.ServiceName ?? "NoServiceName") +
                " on server " +
                (service.ServerName ?? "NoServerName") + " FriendlyName: " + service.FriendlyName +
                " is not responding.";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please restore sevice " +
                (service.ServerName ?? "NoServerName") + " on " +
                (service.ServerName ?? "NoServerName") + " FriendlyName: " + service.FriendlyName;
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServiceRestored(Service service)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(service.Recipients ?? "email...");
            //https://mtadev.mta-flint.net:40443/Websites
            MyMsg.Subject = "Service " +
                (service.ServiceName ?? "NoServiceName") + " on server " +
                (service.ServerName ?? "NoServerName") + " FriendlyName: " + service.FriendlyName +
                " has been restored.";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please confirm sevice " + " FriendlyName: " + service.FriendlyName +
                (service.ServiceName ?? "NoServiceName") + " is running on " +
                (service.ServerName ?? "NoServerName");
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendProcessInitSuccess(API.Data.Models.Process process)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(process.Recipients ?? "email...");
            MyMsg.Subject = "Process " +
                (process.ProcessName ?? "NoProcessName") + " FriendlyName: " + process.FriendlyName +
                " connected and monitoring on server " +
                (process.ServerName ?? "NoServerName") + "...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Process " +
                (process.ProcessName ?? "NoProcessName") +
                " on server " +
                (process.ServerName ?? "NoServerName") + " FriendlyName: " + process.FriendlyName +
                " connection successful.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendProcessFailure(API.Data.Models.Process process)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(process.Recipients ?? "email...");
            MyMsg.Subject = "Process " +
                (process.ProcessName ?? "NoProcessName") + " FriendlyName: " + process.FriendlyName +
                " is not running on server " +
                (process.ServerName ?? "NoServerName");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please start process " +
                (process.ProcessName ?? "NoProcessName") +
                " on server " +
                (process.ServerName ?? "NoServerName") + " FriendlyName: " + process.FriendlyName;
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendProcessRestored(API.Data.Models.Process process)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(process.Recipients ?? "email...");
            MyMsg.Subject = "Process " +
                (process.ProcessName ?? "NoProcessName") + " FriendlyName: " + process.FriendlyName +
                " has been restored on server " +
                (process.ServerName ?? "NoServerName");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please confirm process " +
                (process.ProcessName ?? "NoProcessName") + " FriendlyName: " + process.FriendlyName +
                " is running on server " +
                (process.ServerName ?? "NoServerName");
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendWebsiteInitSuccess(Website website)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(website.Recipients ?? "email...");
            MyMsg.Subject = "Website " +
                (website.WebsiteName ?? "NoWebsiteName") +
                " connected and monitoring on " +
                (website.ServerName ?? "NoServerName") + "...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Website " +
                (website.WebsiteName ?? "NoWebsiteName") +
            " on server " +
                (website.ServerName ?? "NoServerName") + " FriendlyName: " + website.FriendlyName +
                " connection successful.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendWebsiteFailure(Website website)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(website.Recipients ?? "email...");
            MyMsg.Subject = "Website " +
                (website.WebsiteName ?? "NoWebsiteName") + " FriendlyName: " + website.FriendlyName +
                " is not responding on server " +
                (website.ServerName ?? "NoServerName");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please restore website " +
                (website.WebsiteName ?? "NoWebsiteName") + " FriendlyName: " + website.FriendlyName +
                " on server " +
                (website.ServerName ?? "NoServerName");
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendWebsiteRestored(Website website)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(website.Recipients ?? "email...");
            MyMsg.Subject = "Website " +
                (website.WebsiteName ?? "NoWebsiteName") + " FriendlyName: " + website.FriendlyName +
                " has been restored on server " +
                (website.ServerName ?? "NoServerName");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please confirm website " +
                (website.WebsiteName ?? "NoWebsiteName") + " FriendlyName: " + website.FriendlyName +
                " has been restored on server " +
                (website.ServerName ?? "NoServerName");
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendSQLTableInitSuccess(Sqltable sqltable)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(sqltable.Recipients ?? "email...");
            MyMsg.Subject = "SQL Table " +
                (sqltable.TableName ?? "NoSQLTableName") + " FriendlyName: " + sqltable.FriendlyName +
                " connected and monitoring on " +
                (sqltable.ServerName ?? "NoServerName") + "...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "SQL Table " +
                (sqltable.TableName ?? "NoSQLTableName") +
                " on server " +
                (sqltable.ServerName ?? "NoServerName") +
                " connection successful.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendSQLTableFailure(Sqltable sqltable)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(sqltable.Recipients ?? "email...");
            MyMsg.Subject = "SQL Table " +
                (sqltable.TableName ?? "NoSQLTableName") + " FriendlyName: " + sqltable.FriendlyName +
                " is not responding on server " +
                (sqltable.ServerName ?? "NoServerName");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please restore sqltable " +
                (sqltable.TableName ?? "NoSQLTableName") + " FriendlyName: " + sqltable.FriendlyName +
                " on server " +
                (sqltable.ServerName ?? "NoServerName");
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendSQLTableRestored(Sqltable sqltable)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            //MyMsg.To.Add("email...");
            MyMsg.To.Add(sqltable.Recipients ?? "email...");
            MyMsg.Subject = "Website " +
                (sqltable.TableName ?? "NoSQLTableName") + " FriendlyName: " + sqltable.FriendlyName +
                " has been restored on server " +
                (sqltable.ServerName ?? "NoServerName");
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "Please confirm sqltable " +
                (sqltable.TableName ?? "NoSQLTableName") + " FriendlyName: " + sqltable.FriendlyName +
                " has been restored on server " +
                (sqltable.ServerName ?? "NoServerName");
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        private static NetworkCredential SetCredentials()
        {
            return new NetworkCredential("intranet@mtaflint.org", "$mtainet23!");
            //return new NetworkCredential(_configuration?["EmailUN"], _configuration?["EmailPW"]);
        }

        private static MailAddress SetMailAddress()
        {
            return new MailAddress("intranet@mtaflint.org", "intranet@mtaflint.org");
        }
    }
}
