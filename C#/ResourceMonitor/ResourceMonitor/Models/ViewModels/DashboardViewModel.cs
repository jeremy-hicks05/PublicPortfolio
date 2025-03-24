namespace ResourceMonitor.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<Server> Servers { get; set; }
        public List<Service> Services { get; set; }
        public List<Process> Processes { get; set; }
        public List<Website> Websites { get; set; }
        public List<Sqltable> SqlTables { get; set; }
    }

    //public class ServerStatus { public string Name { get; set; } public string Status { get; set; } }
    //public class ServiceStatus { public string Name { get; set; } public string Status { get; set; } }
    //public class ProcessStatus { public string Name { get; set; } public string Status { get; set; } }
    //public class WebsiteStatus { public string Url { get; set; } public string Status { get; set; } }
    //public class SqlTableStatus { public string Server { get; set; } public string TableName { get; set; } public string Status { get; set; } }

}
