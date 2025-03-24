namespace MTAIntranet.MVC.Utility
{
    public static class Utility
    {
        public static string MonthConverstion(int? month)
        {
            //int intMonth = int.TryParse(month);
            //string convertedMonth = "";
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sept";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    return "All of " + DateTime.Now.Year;
            }
        }

        public static bool MatchDoW(DateTime dt, string dow)
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

        public static string DoWToName(string dow)
        {
            switch (dow)
            {
                case "M":
                    return "Monday";
                case "T":
                    return "Tuesday";
                case "W":
                    return "Wednesday";
                case "H":
                    return "Thursday";
                case "F":
                    return "Friday";
                case "S":
                    return "Saturday";
                case "Y":
                    return "Sunday";
                case "D":
                    return "Weekday";
            }
            return "Error: Day of Week unrecognized";
        }
    }
}
