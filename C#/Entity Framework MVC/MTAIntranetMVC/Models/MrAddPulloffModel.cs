using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTAIntranet.MVC.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MTAIntranet.Shared;
    public class MrAddPulloffModel
    {
        public MTAIntranetContext db;
        public string? SelectedRouteName;
        public IEnumerable<MasterRoute>? MasterRoutes;
        public MasterRoute? MasterRoute;
        public IEnumerable<Pulloff>? Pulloffs;

        public MrAddPulloffModel(MTAIntranetContext db)
        {
            this.db = db;
        }

        public MrAddPulloffModel(MTAIntranetContext injectedContex, string signature)
        {
            List<MasterRoute> masterRoutes = new();

            db = injectedContex;
            if (db.MasterRoutes != null)
            {
                MasterRoutes = db.MasterRoutes.ToList();
            }

            if (db.MasterRoutes != null)
            {
                for (DateTime dateIerator = new DateTime(DateTime.Now.Year, 1, 1);
                dateIerator.Year == DateTime.Now.Year;
                dateIerator = dateIerator.AddDays(1))
                {
                    foreach (MasterRoute masterRoute in db.MasterRoutes.ToList().Where(mr => mr.GetSignature() == signature))
                    {
                        if (MatchDoW(dateIerator, masterRoute.dow!))
                        {

                            if (masterRoute != null && masterRoute.mode != null && masterRoute.route != null &&
                                masterRoute.run != null && masterRoute.suffix != null && masterRoute.route_name != null &&
                                masterRoute.pull_out_time != null && masterRoute.pull_in_time != null && db.Pulloffs != null)
                            {
                                MasterRoute mr = new MasterRoute(masterRoute, dateIerator);
                                mr.Matches = db.Pulloffs.ToList()
                                    .Where(p => (p.GetUniqueSignature() ==
                                        (masterRoute.route_name +
                                            masterRoute.mode +
                                            masterRoute.route +
                                            masterRoute.run +
                                            masterRoute.suffix +
                                            dateIerator.Month +
                                            dateIerator.Day).Replace(" ", "")))
                                    .ToList();
                                if ((signature == "" ))
                                    //&& mr.Matches!.Count > 0))
                                {
                                    masterRoutes.Add(mr);
                                }
                                else if ((mr.GetSignature() == signature))
                                    //&& mr.Matches!.Count > 0))
                                {
                                    masterRoutes.Add(mr);
                                }
                            }
                        }
                    }
                }
                MasterRoutes = masterRoutes.Distinct();
                MasterRoute = masterRoutes.First();
            }
        }

        [Key]
        public int? PulloffID { get; set; }

        [Column(TypeName = "varchar (50")]
        [StringLength(50)]
        public string? Route_Name { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Mode { get; set; }

        [Column(TypeName = "varchar (2)")]
        [StringLength(2)]
        public string? DoW { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Route { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? PulloffTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? PulloffReturn { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Run { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Suffix { get; set; }

        public void AddPulloff(Pulloff pulloff)
        {
            if (MatchDoW(pulloff.PulloffTime!.Value, pulloff.DoW!))
            {
                db.Add(pulloff);
                db.SaveChanges();
            }
        }

        private static bool MatchDoW(DateTime dt, string dow)
        {
            if (((dow == "M" && dt.DayOfWeek == DayOfWeek.Monday) ||
                (dow == "T" && dt.DayOfWeek == DayOfWeek.Tuesday) ||
                (dow == "W" && dt.DayOfWeek == DayOfWeek.Wednesday) ||
                (dow == "H" && dt.DayOfWeek == DayOfWeek.Thursday) ||
                (dow == "F" && dt.DayOfWeek == DayOfWeek.Friday) ||
                (dow == "S" && dt.DayOfWeek == DayOfWeek.Saturday) ||
                (dow == "Y" && dt.DayOfWeek == DayOfWeek.Sunday))
                ||
                (dow == "D" &&
                    ((dt.DayOfWeek == DayOfWeek.Monday) ||
                    (dt.DayOfWeek == DayOfWeek.Tuesday) ||
                    (dt.DayOfWeek == DayOfWeek.Wednesday) ||
                    (dt.DayOfWeek == DayOfWeek.Thursday) ||
                    (dt.DayOfWeek == DayOfWeek.Friday))))
            {
                return true;
            }
            return false;
        }
    }
}
