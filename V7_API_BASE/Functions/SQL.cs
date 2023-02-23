using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V7_API_BASE.Base;

namespace V7_API_BASE.Functions
{
    public class SQL : BaseSQL, IDisposable
    {

        SqlConnection _conn { get; set; }

        public SQL(string connectionString = "")
        {            
            string cs = connectionString;

            if (String.IsNullOrEmpty(cs))
                cs = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            _conn = new SqlConnection(cs);
            //_conn.Open();            

        }

        public override List<T> GetQuery<T>(string q, object param = null)
        {
            List<T> result = new List<T>();

            try
            {
                _conn.Open();

                using (IDbConnection dbConn = _conn)
                {
                    result = _conn.Query<T>(q, param).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

            return result;
        }

        public override T GetQuerySingleRow<T>(string q, object param = null)
        {
            T result;

            try
            {
                _conn.Open();

                using (IDbConnection dbConn = _conn)
                {
                    result = _conn.QuerySingle<T>(q, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

            return result;
        }

        public override T GetQuerySingleData<T>(string q, object param = null)
        {
            T result;

            try
            {
                _conn.Open();

                using (IDbConnection dbConn = _conn)
                {
                    result = _conn.ExecuteScalar<T>(q, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

            return result;
        }

        public override List<T> GetStoredProcedure<T>(string sp, object param = null)
        {
            List<T> result = new List<T>();

            try
            {
                _conn.Open();

                using (IDbConnection dbConn = _conn)
                {
                    result = _conn.Query<T>(sp, param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

            return result;

            
        }

        public override bool ExecuteQuery(string q, object param = null)
        {
            try
            {
                _conn.Open();

                using (IDbConnection dbConn = _conn)
                {
                    _conn.Execute(q, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

            return true;
        }

        public override bool ExecuteStoredProcedure(string sp, object param = null)
        {           
            try
            {
                _conn.Open();

                using (IDbConnection dbConn = _conn)
                {
                    dbConn.Execute(sp, param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

            return true;
        }

        public void Dispose()
        {
            try
            {
                _conn.Close();
                _conn.Dispose();
            }
            catch (Exception _)
            {

                
            }            
        }
    }
}
