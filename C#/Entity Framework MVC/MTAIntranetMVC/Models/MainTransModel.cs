namespace MTAIntranet.MVC.Models
{
    using MTAIntranet.Shared;

    public class MainTransModel
    {
        private readonly FuelmasterContext? _db;
        public IQueryable<MainTrans>? MainTransQ { get; set; }
        public List<MainTrans>? MainTransList { get; set; }
        public int Odometer { get; set; }
        public string? VehicleID { get; set; }

        public MainTransModel()
        {
        }

        public MainTransModel(FuelmasterContext db)
        {
            _db = db;
            MainTransQ = _db.MainTrans;
        }

        public MainTransModel(FuelmasterContext db, string id)
        {
            _db = db;

            MainTransList = _db.MainTrans?
                .Where(trans =>
                trans.VEHICLEID != null &&
                trans.VEHICLEID.Contains(id))
                .OrderBy(trans => trans.TRANTIME).Reverse().ToList();

            MainTransList?.ForEach(
                mt =>
                {
                    if (mt.USERID == "         ")
                    {
                        mt.User =
                        new User()
                        {
                            FNAME = "UNIT",
                            LNAME = "AIMS",
                            USERID = "00009999"
                        };
                    }
                    else if (mt.USERID != "         ")
                    {
                        mt.User = _db.User?
                            .Where(u => u.USERID == mt.USERID)
                            .First();
                    }
                    else
                    {
                        mt.User =
                        new User()
                        {
                            FNAME = "User",
                            LNAME = "Missing",
                            USERID = "00000000"
                        };
                    }
                });
        }

        public MainTransModel(FuelmasterContext db, string vehicleid, int odometer)
        {
            _db = db;

            MainTransList = _db.MainTrans?
                .Where(trans =>
                trans.VEHICLEID != null &&
                trans.VEHICLEID.Contains(vehicleid)
                    && trans.ODOMETER == odometer)
                .OrderBy(trans => trans.TRANTIME).Reverse().ToList();

            MainTransList?.ForEach(
                mt =>
                {
                    if (mt.USERID == "         ")
                    {
                        mt.User =
                        new User()
                        {
                            FNAME = "UNIT",
                            LNAME = "AIMS",
                            USERID = "00009999"
                        };
                    }
                    else if (mt.USERID != "         ")
                    {
                        mt.User = _db.User?
                            .Where(u => u.USERID == mt.USERID)
                            .First();
                    }
                    else
                    {
                        mt.User =
                        new User()
                        {
                            FNAME = "User",
                            LNAME = "Missing",
                            USERID = "00000000"
                        };
                    }
                });
        }
    }
}
