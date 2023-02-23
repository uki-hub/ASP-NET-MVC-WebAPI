using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_BASE.Base
{
    public abstract class BaseSQL
    {
        public abstract List<T> GetQuery<T>(string q, object param = null);
        public abstract T GetQuerySingleRow<T>(string q, object param = null);
        public abstract T GetQuerySingleData<T>(string q, object param = null);
        public abstract List<T> GetStoredProcedure<T>(string sp, object param = null);
        public abstract bool ExecuteQuery(string q, object param = null);
        public abstract bool ExecuteStoredProcedure(string sp, object param = null);
    }
}
