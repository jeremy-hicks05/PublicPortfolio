namespace MTAIntranetMVC.Models
{
    using MTAIntranet.MVC.Utility;
    using MTAIntranet.Shared;
    public class PulloffsByMonthModel
    {
        private readonly MTAIntranetContext? _db;
        public IList<Pulloff>? Pulloffs;
        public IQueryable<Pulloff>? PulloffsQ;
        public IList<MasterRoute>? MasterRoutes;
        public IQueryable<MasterRoute>? MasterRoutesQ;
        public string? Filter;
        public int Year;

        public PulloffsByMonthModel(MTAIntranetContext db)
        {
            _db = db;
            PulloffsQ = _db.Pulloffs!
                .Where(p => p.PulloffTime!.Value.Year == DateTime.Now.Year);
        }

        public PulloffsByMonthModel(MTAIntranetContext db, int year, int? id)
        {
            _db = db;
            Year = year;
            PulloffsQ = db.Pulloffs!
                .Where(p => p.PulloffTime!.Value.Year == year);
            Filter = Utility.MonthConverstion(id);
        }

        public PulloffsByMonthModel(IList<MasterRoute>? masterRoutes)
        {
            MasterRoutes = masterRoutes;
        }
    }
}