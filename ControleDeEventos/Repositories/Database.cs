using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeEventos
{
    class Database
    {
        private static string _local = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Database.db");
        private static SQLiteConnection _connection;

        private static SQLiteConnection ConnectionDatabase() {
            _connection = new SQLiteConnection($"Data Source={_local}");
            _connection.Open();
            return _connection;
        }

        public DataTable GetQuery(string command)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                using (var cmd = ConnectionDatabase().CreateCommand())
                {
                    cmd.CommandText = command;
                    da = new SQLiteDataAdapter(cmd.CommandText, ConnectionDatabase());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public int SetQuery(string command)
        {

            try
            {
                var cmd = ConnectionDatabase().CreateCommand();
                cmd.CommandText = command;
                int id = cmd.ExecuteNonQuery();
                ConnectionDatabase().Close();
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
