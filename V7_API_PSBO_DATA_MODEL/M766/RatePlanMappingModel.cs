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
        public string RatePlanName { get; set; }
        public bool isConnected { get; set; }
        public bool isNoUpdateRate { get; set; }
        public bool isNoUpdateAvailability { get; set; }
        public bool isNoUpdateRestriction { get; set; }
    }
}
