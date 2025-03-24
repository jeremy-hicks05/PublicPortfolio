namespace MTAIntranetMVC.Models
{
    using MTAIntranet.MVC.Models;
    using MTAIntranet.Shared;

    public class AddPulloffToMasterRouteModel
    {
        MTAIntranetContext _db;
        public PulloffModel Pulloff { get; set; } = new();
        public MasterRoute MasterRoute { get; set; } = new();
        public int Year { get; set; }

        public AddPulloffToMasterRouteModel(MTAIntranetContext db,
            MrSignature signature,
            int year)
        {
            _db = db;
            Year = year;
            if (_db.MasterRoutes is not null)
            {
                //MasterRoute tempMasterRoute = _db.MasterRoutes!.ToList()
                //.Where(mr =>
                //        mr.GetSignature() == signature.GetValue())
                //.ToList().First();

                MasterRoute = _db.MasterRoutes!.ToList()
                .Where(mr =>
                        mr.GetSignature() == signature.GetValue())
                .ToList().First();

                Pulloff.Mode = MasterRoute.mode;
                Pulloff.DoW = MasterRoute.dow;
                Pulloff.Route_Name = MasterRoute.route_name;
                Pulloff.Suffix = MasterRoute.suffix;
                Pulloff.Route = MasterRoute.route;
                Pulloff.Run = MasterRoute.run;
                Pulloff.Year = year;

                Pulloff.PulloffTime = null;
                Pulloff.PulloffReturn = null;

                MasterRoute.Matches = db.Pulloffs!.ToList()
                    .Where(pulloff => pulloff.PulloffTime is not null &&
                    pulloff.PulloffTime.Value.Year == Year &&
                    pulloff.GetSignature() == MasterRoute.GetSignature()).ToList();
            }
        }

        public AddPulloffToMasterRouteModel(MTAIntranetContext db,
            PulloffModel pulloff,
            int year)
        {
            _db = db;
            Year = year;
            Pulloff = pulloff;
            if (_db.MasterRoutes is not null)
            {
                MasterRoute = _db.MasterRoutes!.ToList()
                .Where(mr =>
                        mr.GetSignature() == pulloff.GetSignature())
                .ToList().First();

                //Pulloff.Mode = MasterRoute.mode;
                //Pulloff.DoW = MasterRoute.dow;
                //Pulloff.Route_Name = MasterRoute.route_name;
                //Pulloff.Suffix = MasterRoute.suffix;
                //Pulloff.Route = MasterRoute.route;
                //Pulloff.Run = MasterRoute.run;
                //Pulloff.Year = year;

                //Pulloff.PulloffTime = null;
                //Pulloff.PulloffReturn = null;

                MasterRoute.Matches = db.Pulloffs!.ToList()
                    .Where(pulloff => pulloff.PulloffTime is not null &&
                    pulloff.PulloffTime.Value.Year == Year &&
                    pulloff.GetSignature() == MasterRoute.GetSignature()).ToList();
            }
        }
    }
}