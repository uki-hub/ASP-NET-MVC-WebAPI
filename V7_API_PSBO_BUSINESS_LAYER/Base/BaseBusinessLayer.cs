using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_PSBO_BUSINESS_LAYER.Base
{
    public abstract class BaseBusinessLayer
    {
        internal string DateTimeNow_yyyyMMddHHmmss { get => DateTimeTo_yyyMMddHHmmss(DateTime.Now); }
        internal string DateTimeTo_yyyMMddHHmmss(DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");

    }
}
