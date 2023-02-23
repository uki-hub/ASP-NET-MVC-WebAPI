using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V7_API_BASE.Functions;
using V7_API_BASE.Models;
using V7_API_PSBO_DATA_ACCESS.Base;

namespace V7_API_PSBO_DATA_ACCESS.M766
{
    public class ConnectivityStatusDA : BaseDataAccess
    {
        public bool UpdateConnectivity(string intAccNo, bool stsActive, bool stsSendRsv, string startTime)
        {
            return base.SQL.ExecuteQuery($@"
                                update hem261 set 
	                                 stsactv = @StsActive
	                                ,stsSendRsv = @StsSendRsv
	                                ,startTime = @StartTime
                                where
	                                intAccNo = @IntAccNo",
                                    new
                                    {
                                        IntAccNo = intAccNo,
                                        StsActive = stsActive ? "Y" : null,
                                        StsSendRsv = stsSendRsv ? "Y" : null,
                                        StartTime = startTime
                                    });
        }
    }
}
