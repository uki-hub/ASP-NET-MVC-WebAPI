using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V7_API_BASE.Base;
using V7_API_BASE.Functions;

namespace V7_API_PSBO_DATA_ACCESS.Base
{
    public abstract class BaseDataAccess
    {
        private SQLTransaction _transaction { get; set; }

        public BaseSQL SQL
        {
            get
            {
                if (_transaction == null)
                {
                    return new SQL();
                }
                else
                {
                    return _transaction;
                }
            }
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new Exception($"Transaction already created in this DataAccess({this.GetType().Name}). End it first");

            _transaction = new SQLTransaction();
        }

        public void EndTransaction()
        {
            _transaction.Dispose();
            _transaction = null;
        }

        public void Commit() => _transaction.Commit();

        public void Rollback() => _transaction.Rollback();
    }
}
