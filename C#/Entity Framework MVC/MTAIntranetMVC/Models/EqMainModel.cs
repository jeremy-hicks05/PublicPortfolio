namespace MTAIntranet.MVC.Models
{
    using MTAIntranet.Shared;

    public class EqMainModel
    {
        private readonly EAMProdContext? _db;
        public IQueryable<EqMain>? VehQ { get; set; }
        public List<EqMain>? VehList { get; set; }

        public string?[]? LocationList { get; set; }

        public EqMainModel()
        {
        }

        public EqMainModel(EAMProdContext db)
        {
            _db = db;
            if (_db.EqMains is not null)
            {
                VehList = _db.EqMains
                    .Where
                    (
                        veh => 
                            veh.SlaStatus == "IN SERVICE" &&
                            veh.LocStoredLoc != "DORT" &&
                            veh.LocStoredLoc != "DORT-IT"
                    )
                    .OrderBy(veh => veh.EqEquipNo)
                    .ToList();
                LocationList = _db.EqMains.Select(veh => veh.LocStoredLoc).Distinct().ToArray();
            } 
        }

        public EqMainModel(EAMProdContext db, string? servicecenter)
        {
            _db = db;
            if (_db.EqMains is not null)
            {
                VehList = _db.EqMains
                    .Where
                    (
                        veh => 
                            veh.LocStoredLoc == servicecenter &&
                            veh.SlaStatus == "IN SERVICE" &&
                            veh.LocStoredLoc != "DORT" &&
                            veh.LocStoredLoc != "DORT-IT"
                    )
                    .OrderBy(veh => veh.EqEquipNo)
                    .ToList();
                LocationList = _db.EqMains.Select(veh => veh.LocStoredLoc).Distinct().ToArray();
            }
        }
    }
}
