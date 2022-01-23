using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelektika_1.Models
{
    class DataAcess
    {

        private static readonly string connectionString = "Data Source=./Utilities/KnowlegeBase.db";
        public static List<DataSetModel> LoadDataSets()
        {
            Trace.WriteLine(Directory.GetCurrentDirectory());
            using System.Data.IDbConnection cnn = new SqliteConnection(connectionString);
            var output = cnn.Query<DataSetModel>("select * from DataSet", new DynamicParameters());
            return output.ToList();
        }

        public static List<string> LoadSports()
        {
            using System.Data.IDbConnection cnn = new SqliteConnection(connectionString);
            var output = cnn.Query<string>("select distinct sport from DataSet", new DynamicParameters());
            return output.ToList();
        }



        public static void SaveDataSet(DataSetModel dataSet)
        {
            using System.Data.IDbConnection cnn = new SqliteConnection(connectionString);
            cnn.Execute(
                "insert into DataSet (Height, Weight, Sport, Position) values (@Height, @Weight, @Sport, @Position)",
                dataSet
                );
        }

        public static void DeleteDBEntries()
        {
            using System.Data.IDbConnection cnn = new SqliteConnection(connectionString);
            cnn.Execute("delete from DataSet;");
        }
    }
}
