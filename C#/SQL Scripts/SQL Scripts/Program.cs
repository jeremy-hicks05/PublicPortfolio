using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SQL_Scripts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting script");

            //string connectionString = "";
            //string strFileType = "Type";
            string path = @"C:\Users\jeremy.hicks\Desktop\";
            string filename = "passenger counts 10_2023.xlsx";
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                path + filename + ";" +
                "Extended Properties=\"Excel 12.0;HDR=YES\";";


            string query = "SELECT * FROM [Sheet1$]";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();

                var rows = new List<Row>();
                while (reader.Read())
                {
                    var fieldCount = reader.FieldCount;

                    var fieldIncrementor = 1;
                    var fields = new List<string>();
                    while (fieldCount >= fieldIncrementor)
                    {
                        fields.Add(reader[fieldIncrementor - 1].ToString());
                        fieldIncrementor++;
                    }

                    rows.Add(
                        new Row()
                        {
                            Id = fields[0],
                            fiscal_yr = fields[1],
                            date = fields[2],
                            mode = fields[3],
                            dow = fields[4],
                            route = fields[5],
                            run = fields[6],
                            emp_no = fields[7],
                            in_out = fields[8],
                            time = fields[9],
                            location = fields[10],
                            transfers = fields[11],
                            passengers = fields[12]
                        }
                        );
                }

                foreach (Row row in rows)
                {
                    Console.WriteLine(row.Id);
                    // insert into SQL table

                    SqlConnection sqlConnection =
                    new SqlConnection
                        (connectionString =
                        "Server=" +
                        "FLTAS003;" +
                        "Database=" +
                        "MTA;" +
                        "Integrated Security=SSPI;");

                    sqlConnection.Open();

                    string sql = "INSERT INTO FR_ridership_reports_2023_10" +
                        "(id," +
                        "fiscal_yr," +
                        "date," +
                        "mode," +
                        "dow," +
                        "route," +
                        "run," +
                        "emp_no," +
                        "in_out," +
                        "time," +
                        "location," +
                        "transfers," +
                        "passengers) " +
                        "VALUES(" +
                        "@param1," +
                        "@param2," +
                        "@param3," +
                        "@param4," +
                        "@param5," +
                        "@param6," +
                        "@param7," +
                        "@param8," +
                        "@param9," +
                        "@param10," +
                        "@param11," +
                        "@param12," +
                        "@param13)";
                    using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                    {
                        cmd.Parameters.Add("@param1", 
                            SqlDbType.Int).Value = row.Id;
                        cmd.Parameters.Add("@param2", 
                            SqlDbType.VarChar, 10).Value = row.fiscal_yr;
                        cmd.Parameters.Add("@param3", 
                            SqlDbType.Date).Value = row.date;
                        cmd.Parameters.Add("@param4", 
                            SqlDbType.VarChar, 10).Value = row.mode;
                        cmd.Parameters.Add("@param5", 
                            SqlDbType.VarChar, 10).Value = row.dow;
                        cmd.Parameters.Add("@param6", 
                            SqlDbType.VarChar, 10).Value = row.route;
                        cmd.Parameters.Add("@param7", 
                            SqlDbType.VarChar, 10).Value = row.run;
                        cmd.Parameters.Add("@param8", 
                            SqlDbType.VarChar, 10).Value = row.emp_no;
                        cmd.Parameters.Add("@param9", 
                            SqlDbType.VarChar, 10).Value = row.in_out;
                        cmd.Parameters.Add("@param10", 
                            SqlDbType.Date, 100).Value = row.time;
                        cmd.Parameters.Add("@param11", 
                            SqlDbType.VarChar, 10).Value = row.location;
                        cmd.Parameters.Add("@param12", 
                            SqlDbType.Int, 10).Value = row.transfers;
                        cmd.Parameters.Add("@param13", 
                            SqlDbType.Int, 10).Value = row.passengers;
                        cmd.ExecuteNonQuery();

                    }
                    sqlConnection.Close();
                }

                reader.Close();
                Console.ReadLine();
            }
        }
    }

    class Row
    {
        public string Id;
        public string fiscal_yr;
        public string date;
        public string mode;
        public string dow;
        public string route;
        public string run;
        public string emp_no;
        public string in_out;
        public string time;
        public string location;
        public string transfers;
        public string passengers;
    }
}