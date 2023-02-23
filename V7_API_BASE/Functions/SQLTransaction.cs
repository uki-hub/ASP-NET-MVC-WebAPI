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
    public class SQLTransaction : BaseSQL, IDisposable
    {
        SqlConnection _conn { get; set; }
        SqlTransaction _trans { get; set; }

        public SQLTransaction(string connectionString = "")
        {
            string cs = connectionString;

            if (String.IsNullOrEmpty(cs))
                cs = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            _conn = new SqlConnection(cs);
            _conn.Open();

            _trans = _conn.BeginTransaction();
        }

        public void Commit() => _trans.Commit();

        public void Rollback() => _trans.Rollback();

        public override List<T> GetQuery<T>(string q, object param = null)
        {
            List<T> result = new List<T>();

            try
            {
                result = _conn.Query<T>(q, param, _trans).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }


            return result;
        }

        public override T GetQuerySingleRow<T>(string q, object param = null)
        {
            T result;

            try
            {
                result = _conn.QuerySingle<T>(q, param, _trans);
            }
            catch (Exception e)
            {
                throw e;
            }


            return result;
        }

        public override T GetQuerySingleData<T>(string q, object param = null)
        {
            T result;

            try
            {
                result = _conn.ExecuteScalar<T>(q, param, _trans);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public override List<T> GetStoredProcedure<T>(string sp, object param = null)
        {
            List<T> result = new List<T>();

            try
            {
                result = _conn.Query<T>(sp, param, _trans, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public override bool ExecuteQuery(string q, object param = null)
        {
            try
            {
                _conn.Execute(q, param, _trans);
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public override bool ExecuteStoredProcedure(string sp, object param = null)
        {
            try
            {
                _conn.Execute(sp, param, _trans, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public void Dispose()
        {
            try
            {
                _trans.Dispose();
                _conn.Close();
                _conn.Dispose();
            }
            catch (Exception _) { }
        }
    }
}
