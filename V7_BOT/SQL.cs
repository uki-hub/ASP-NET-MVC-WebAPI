using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_BOT
{
    public class SQL : IDisposable
    {
        SqlConnection _conn { get; set; }

        public SQL(string connectionString = "")
        {
            string cs = connectionString;

            if (String.IsNullOrEmpty(cs))
                cs = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            _conn = new SqlConnection(cs);
            _conn.Open();

        }

        public DataSet Query(string query, bool executeOnly = false)
        {
            var result = new DataSet();

            try
            {
                using (var da = new SqlDataAdapter())
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = _conn;
                    cmd.CommandText = query;
                    if (executeOnly)
                    {
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        da.SelectCommand = cmd;
                        da.Fill(result);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }


            return result;
        }

        public void Dispose()
        {
            _conn.Close();
        }
    }
}
