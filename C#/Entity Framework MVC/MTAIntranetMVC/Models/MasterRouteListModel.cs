namespace MTAIntranet.MVC.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using MTAIntranet.MVC.Utility;
    using MTAIntranet.Shared;
    using System.ComponentModel.Design.Serialization;
    using System.Reflection.Metadata;

    public class MasterRouteListModel
    {
        public List<MasterRoute>? MasterRoutes;
        public MasterRoute? MasterRoute { get; set; }

        private readonly MTAIntranetContext? db;
        public int? Year;

        public MasterRouteListModel(MTAIntranetContext injectedContex, MrSignature signature, int? year)
        {
            List<MasterRoute> masterRoutes = new();

            db = injectedContex;
            Year = year;
            if (db.MasterRoutes != null)
            {
                MasterRoutes = db.MasterRoutes.ToList();
            }

            if (db.MasterRoutes != null)
            {
                for (DateTime dateIterator = (Year == null ? new DateTime(DateTime.Now.Year, 1, 1) : new DateTime(Year.Value, 1, 1));
                dateIterator.Year == Year;
                dateIterator = dateIterator.AddDays(1))
                {
                    foreach (MasterRoute masterRoute in db.MasterRoutes.ToList().Where(mr => mr.GetSignature() == signature.GetValue()))
                    {
                        if (Utility.MatchDoW(dateIterator, masterRoute.dow!))
                        {
                            if (masterRoute != null && masterRoute.mode != null && masterRoute.route != null &&
                                masterRoute.run != null && masterRoute.suffix != null && masterRoute.route_name != null &&
                                masterRoute.pull_out_time != null && masterRoute.pull_in_time != null && db.Pulloffs != null)
                            {
                                MasterRouteModel mr = new MasterRouteModel(masterRoute, dateIterator);
                                mr.Matches = db.Pulloffs.ToList()
                                    .Where(p => p.PulloffTime!.Value.Year == Year &&
                                            p.GetUniqueSignature() ==
                                        (masterRoute.route_name +
                                            masterRoute.mode +
                                            masterRoute.route +
                                            masterRoute.run +
                                            masterRoute.suffix +
                                            dateIterator.Month +
                                            dateIterator.Day).Replace(" ", ""))
                                    .ToList();
                                if (mr.pull_out_time!.Value.Year == Year &&
                                    signature.GetValue() == "" && mr.Matches!.Count > 0)
                                {
                                    MasterRoute mrToAdd = new()
                                    {
                                        route_name = mr.route_name,
                                        mode = mr.mode,
                                        route = mr.route_name,
                                        run = mr.route_name,
                                        suffix = mr.route_name,
                                        pull_out_time = mr.pull_out_time,
                                        pull_in_time = mr.pull_in_time,
                                        beg_dh_miles = mr.beg_dh_miles,
                                        end_dh_miles = mr.end_dh_miles
                                    };
                                    masterRoutes.Add(mrToAdd);
                                }
                                else if (mr.pull_out_time!.Value.Year == Year &&
                                    mr.GetSignature() == signature.GetValue()
                                    && mr.Matches!.Count > 0)
                                {
                                    MasterRoute mrToAdd = new()
                                    {
                                        route_name = mr.route_name,
                                        mode = mr.mode,
                                        route = mr.route_name,
                                        run = mr.route_name,
                                        suffix = mr.route_name,
                                        pull_out_time = mr.pull_out_time,
                                        pull_in_time = mr.pull_in_time,
                                        beg_dh_miles = mr.beg_dh_miles,
                                        end_dh_miles = mr.end_dh_miles
                                    };
                                    masterRoutes.Add(mrToAdd);
                                }
                            }
                        }
                    }
                }
                MasterRoutes = masterRoutes;
            }
        }

        public MasterRouteListModel(MTAIntranetContext injectedContex, MrSignature signature, MrUniqueSignature uniqueSignature, int? year)
        {
            List<MasterRoute> masterRoutes = new();

            db = injectedContex;
            Year = year;
            if (db.MasterRoutes != null)
            {
                MasterRoutes = db.MasterRoutes.ToList();
            }

            if (db.MasterRoutes != null)
            {
                for (DateTime dateIterator = (Year == null ? new DateTime(DateTime.Now.Year, 1, 1) : new DateTime(Year.Value, 1, 1));
                        dateIterator.Year == Year;
                        dateIterator = dateIterator.AddDays(1))
                {
                    foreach (MasterRoute masterRoute in db.MasterRoutes.ToList().Where(mr => mr.GetSignature() == signature.GetValue()))
                    {
                        if (Utility.MatchDoW(dateIterator, masterRoute.dow!))
                        {
                            if (masterRoute != null && masterRoute.mode != null && masterRoute.route != null &&
                                masterRoute.run != null && masterRoute.suffix != null && masterRoute.route_name != null &&
                                masterRoute.pull_out_time != null && masterRoute.pull_in_time != null && db.Pulloffs != null)
                            {
                                MasterRouteModel mr = new MasterRouteModel(masterRoute, dateIterator);
                                mr.Matches = db.Pulloffs.ToList()
                                    .Where(p => p.PulloffTime!.Value.Year == Year &&
                                            p.GetUniqueSignature() ==
                                        (masterRoute.route_name +
                                            masterRoute.mode +
                                            masterRoute.route +
                                            masterRoute.run +
                                            masterRoute.suffix +
                                            dateIterator.Month +
                                            dateIterator.Day).Replace(" ", ""))
                                    .ToList();
                                if (mr.pull_out_time!.Value.Year == Year &&
                                    mr.GetSignature() == signature.GetValue())
                                {
                                    MasterRoute mrToAdd = new()
                                    {
                                        route_name = mr.route_name,
                                        mode = mr.mode,
                                        route = mr.route_name,
                                        run = mr.route_name,
                                        suffix = mr.route_name,
                                        pull_out_time = mr.pull_out_time,
                                        pull_in_time = mr.pull_in_time,
                                        beg_dh_miles = mr.beg_dh_miles,
                                        end_dh_miles = mr.end_dh_miles
                                    };
                                    masterRoutes.Add(mrToAdd);
                                }
                            }
                        }
                    }
                }
                MasterRoutes = masterRoutes.ToList().Where(mr => mr.GetUniqueSignature() == uniqueSignature.GetValue()).ToList();
            }
        }

        public MasterRouteListModel(MTAIntranetContext injectedContex, int? year)
        {
            db = injectedContex;
            Year = year;
            if (db.MasterRoutes != null)
            {
                MasterRoutes = db.MasterRoutes.ToList();
            }

            if (db.MasterRoutes != null)
            {
                foreach (MasterRoute masterRoute in db.MasterRoutes.ToList())
                {
                    if (db.Pulloffs != null)
                    {
                        masterRoute.Matches = db.Pulloffs.ToList()
                        .Where(p => p.PulloffTime!.Value.Year == Year &&
                        p.GetSignature() == masterRoute.GetSignature())
                        .ToList();
                    }
                }
            }
        }


    }

    public interface ISignature
    {
        public string GetValue();
    }

    public class MrSignature : ISignature
    {
        private readonly string _value = "";

        public MrSignature(string value)
        {
            _value = value;
        }

        public string GetValue()
        {
            return _value;
        }
    }

    public class MrUniqueSignature : ISignature
    {
        private readonly string _value = "";

        public MrUniqueSignature(string value)
        {
            _value = value;
        }

        public string GetValue()
        {
            return _value;
        }
    }
}