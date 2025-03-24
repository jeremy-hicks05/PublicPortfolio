namespace MTAIntranetMVC.Models
{
    using MTAIntranet.Shared;

    public class DelMasterRouteModel
    {

        public MTAIntranetContext _db;
        public MasterRoute? masterRoute;

        public DelMasterRouteModel(MTAIntranetContext db, int id)
        {
            _db = db;
            masterRoute = db.MasterRoutes!
                .ToList()
                .Where(mr => mr.PK_Route_ID == id).FirstOrDefault();
        }
    }
}