using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.Common;

namespace MySqlToCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("input export sql:");
                var sql = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(sql)) continue;
                var fileName = ExportToCsvBySql(sql);
                Console.WriteLine("export success to " + fileName);
            }
        }

        static string ExportToCsvBySql(string sql)
        {
            var constr = ConfigurationManager.AppSettings["constr"];

            using (var db = new MySqlConnection(constr))
            {
                using (var reader = db.ExecuteReader(sql))
                {
                    // use reader here
                    return ToCsv(reader);
                }
            }
        }

        public static string ToCsv(IDataReader reader)
        {
            var filePath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(string.Join(",", Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList()));
                while (reader.Read())
                {
                    var cells = Enumerable.Range(0, reader.FieldCount)
                        .Select(reader.GetValue)
                        .Select(t =>
                        {
                            if (t == null) return null;
                            return CsvWriter.CsvString(t.ToString(), ',');
                        });

                    writer.WriteLine(string.Join(",", cells));
                }
                writer.Flush();
            }
            return filePath;
        }
    }
}
