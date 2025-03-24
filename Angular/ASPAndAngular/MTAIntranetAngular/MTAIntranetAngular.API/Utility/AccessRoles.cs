namespace MTAIntranetAngular.Utility
{
    public static class AccessRoles
    {
        // Ensure WebAdmin is a part of every role for full access
        public const string Everyone = "Domain Users";
        public const string Administration = "Administration Users";
        public const string HR = "HR Users";
        public const string YRFRManagers = "YRFRManagers";
        public const string EAM = "EAM Users,ITS Staff";
        public const string WebAdmin = "WebAdmin";
        public const string ITS = "ITS Staff";
        public const string YourRide = "YR Users,ITS Staff";
        public const string RidesToWellness = "RTW Users,ITS Staff";
        public const string Planning = "Planning Users";
        public const string Maintenance = "Maintenance Users,ITS Staff";
        public const string Purchasing = "Purchasing Users,ITS Staff";
        public const string MasterRouteAdmin = "MasterRouteAdmin,ITS Staff";
        public const string PulloffsAdmin = "PulloffsAdmin,ITS Staff";
    }
}