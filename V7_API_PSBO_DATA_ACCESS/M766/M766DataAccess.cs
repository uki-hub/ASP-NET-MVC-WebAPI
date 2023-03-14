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
    public class M766DataAccess : BaseDataAccess
    {
        public RoomTypeMappingDA RoomTypeMappingDA;
        public RatePlanMappingDA RatePlanMappingDA;
        public ConnectivityStatusDA ConnectivityStatusDA;

        public M766DataAccess()
        {
            RoomTypeMappingDA = new RoomTypeMappingDA();
            RatePlanMappingDA = new RatePlanMappingDA();
            ConnectivityStatusDA = new ConnectivityStatusDA();
        }

        public HEM261 GetHem261(string hotelCode)
        {
            return base.SQL.GetQuerySingleRow<HEM261>($@"
                                select
                                    TOP 1
                                    hotelcd, 
                                    intAccNo, 
                                    accID, 
                                    intPartner, 
                                    datacenter,
                                    stsactv,
	                                stsSendRsv,
	                                startTime
                                    from hem261
                                    where
                                            hotelCd = @HotelCode
                                        and intPartner in ('LGConnect', 'HG', 'SiteConnect')
                                        and coalesce(stsDelete, '') = ''
                                    order by intAccNo desc",
                                    new
                                    {
                                        HotelCode = hotelCode
                                    });
        }
    }
}
