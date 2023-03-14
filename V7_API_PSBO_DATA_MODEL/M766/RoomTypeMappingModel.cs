using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_PSBO_DATA_MODEL.M766
{
    public class RoomTypeMappingModel
    {
        public string RoomTypeCode { get; set; }
        public string RoomTypeName { get; set; }
        public bool IsConnected { get; set; }
        public int? MaxAdult { get; set; }
        public int? MaxXtraBed { get; set; }
    }
}
