using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_PSBO_DATA_MODEL.M766
{
    public class RatePlanMappingModel
    {
        public string RateTierNo { get; set; }
        public string RateTierCode { get; set; }
        public string RatePlanName { get; set; }
        public bool StsConnected { get; set; }
        public bool StsNoUpdateRate { get; set; }
        public bool StsNoUpdateAvailability { get; set; }
        public bool StsNoUpdate { get; set; }
    }
}
