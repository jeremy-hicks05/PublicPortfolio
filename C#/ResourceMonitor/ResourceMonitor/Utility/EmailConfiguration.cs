using System.Net.Mail;
using System.Net;
using System.Text;
//using static HotChocolate.ErrorCodes;
using System.Diagnostics;
using ResourceMonitor.Models;
using ResourceMonitor.Models.KACE;

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

        // servers, services, processes, website, and sqltable health checks
        public static void SendServerSubscriptionSuccess(Server server, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " subscription successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should now receive emails when " + 
                (server.ServerName ?? "noServerName") + " FriendlyName: " + 
                server.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServerUnsubscribeSuccess(Server server, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (server.ServerName ?? "noServerName") + " FriendlyName: " + server.FriendlyName + " unsubscribe successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should no longer receive emails when " + 
                (server.ServerName ?? "noServerName") + " FriendlyName: " + 
                server.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServiceSubscriptionSuccess(Service service, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(service.Recipients ?? "email...");
            MyMsg.Subject = "Server " + (service.ServerName ?? "noServerName") + 
                " FriendlyName: " + service.FriendlyName + 
                " subscription successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should now receive emails when " + 
                (service.ServerName ?? "noServerName") + " FriendlyName: " + 
                service.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendServiceUnsubscribeSuccess(Service service, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (service.ServerName ?? "noServerName") + 
                " FriendlyName: " + service.FriendlyName + " unsubscribe successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should no longer receive emails when " + (service.ServerName ?? "noServerName") + " FriendlyName: " + service.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendProcessSubscriptionSuccess(ResourceMonitor.Models.Process process, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (process.ServerName ?? "noServerName") + 
                " FriendlyName: " + process.FriendlyName + " subscription successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should now receive emails when " + 
                (process.ServerName ?? "noServerName") + " FriendlyName: " + 
                process.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendProcessUnsubscribeSuccess(ResourceMonitor.Models.Process process, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (process.ServerName ?? "noServerName") + 
                " FriendlyName: " + process.FriendlyName + " unsubscribe successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should no longer receive emails when " + 
                (process.ServerName ?? "noServerName") + " FriendlyName: " + 
                process.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendWebsiteSubscriptionSuccess(Website website, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (website.ServerName ?? "noServerName") 
                + " FriendlyName: " + website.FriendlyName + 
                " subscription successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should now receive emails when " + 
                (website.ServerName ?? "noServerName") + " FriendlyName: " +
                website.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendWebsiteUnsubscribeSuccess(Website website, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (website.ServerName ?? "noServerName") +
                " FriendlyName: " + website.FriendlyName + 
                " unsubscribe successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should no longer receive emails when " + 
                (website.ServerName ?? "noServerName") + " FriendlyName: " +
                website.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendSQLTableSubscriptionSuccess(Sqltable sqltable, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (sqltable.ServerName ?? "noServerName")
                + " FriendlyName: " + sqltable.FriendlyName + 
                " subscription successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should now receive emails when " + 
                (sqltable.ServerName ?? "noServerName") + " FriendlyName: " + 
                sqltable.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        public static void SendSQLTableUnsubscribeSuccess(Sqltable sqltable, string username)
        {
            SmtpClient MyMail = new SmtpClient();
            MailMessage MyMsg = new MailMessage();
            MyMail.Host = "smtp.office365.com";
            MyMail.Port = 587;
            MyMail.EnableSsl = true;
            MyMsg.Priority = MailPriority.Normal;
            MyMsg.To.Add("email...");
            MyMsg.To.Add(username);
            MyMsg.Subject = "Server " + (sqltable.ServerName ?? "noServerName") +
                " FriendlyName: " + sqltable.FriendlyName + 
                " unsubscribe successful...";
            MyMsg.SubjectEncoding = Encoding.UTF8;
            MyMsg.IsBodyHtml = true;
            MyMsg.From = SetMailAddress();
            MyMsg.BodyEncoding = Encoding.UTF8;
            MyMsg.Body = "You should no longer receive emails when " + 
                (sqltable.ServerName ?? "noServerName") + " FriendlyName: " +
                sqltable.FriendlyName + " becomes unavailable, or is restored.";
            MyMail.UseDefaultCredentials = false;
            NetworkCredential MyCredentials = SetCredentials();
            MyMail.Credentials = MyCredentials;
            MyMail.Send(MyMsg);
        }

        private static NetworkCredential SetCredentials()
        {
            return new NetworkCredential("email...", "pw...");
            //return new NetworkCredential(_configuration?["EmailUN"], _configuration?["EmailPW"]);
        }

        private static MailAddress SetMailAddress()
        {
            return new MailAddress("email...", "email...");
        }
    }
}
