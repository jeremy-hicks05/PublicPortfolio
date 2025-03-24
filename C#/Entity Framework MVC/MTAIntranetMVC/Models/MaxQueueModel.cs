namespace MTAIntranet.MVC.Models
{
    using MTAIntranet.Shared;
    using System.ComponentModel.DataAnnotations;

    public class MaxQueueModel
    {
        private readonly EAMProdContext? _db;

        public int RowId { get; set; }

        public IQueryable<MaxQueue>? MaxQueueListQ { get; set; }
        public List<MaxQueue>? MaxQueueList { get; set; }

        

        public MaxQueueModel()
        {
            MaxQueueList = _db!.MaxQueues!.Where(mq => mq.row_id == 31552).ToList();
        }

        public MaxQueueModel(EAMProdContext db)
        {
            _db = db;
            //ErrorList = _db.MaxQueues;
        }
    }
}
