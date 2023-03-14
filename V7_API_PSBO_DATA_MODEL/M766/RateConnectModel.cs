using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_PSBO_DATA_MODEL.M766
{
    public class RateConnectModel
    {
        public string RateTierNo { get; set; }
        //public string IntAccNo { get; set; }
        public bool StsNoAvailUpdate { get; set; }
        public bool StsNoRateUpdate { get; set; }
        public bool? StsNoUpdate { get; set; }
    }
}
