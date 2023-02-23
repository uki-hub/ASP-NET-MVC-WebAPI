using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_PSBO_DATA_MODEL.M766
{
    public class ConnectivityStatusModel
    {
        public bool StatusActive { get; set; }
        public bool StatusSendReservation { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
