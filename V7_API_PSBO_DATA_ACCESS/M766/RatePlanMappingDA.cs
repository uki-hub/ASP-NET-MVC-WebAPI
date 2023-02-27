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
    public class RatePlanMappingDA : BaseDataAccess
    {
        public List<HER131> GetHotelRates(string hotelCode)
        {
            return base.SQL.GetQuery<HER131>($@"
                                    select
	                                    *
	                                    from HEReserv..HER131
	                                    where
			                                    hotelcd = @HotelCode
		                                    and (coalesce(stsactv, '') = '' or stsactv = 'A')
                                        order by tiercode ",
                                    new
                                    {
                                        HotelCode = hotelCode
                                    });
        }

        public List<HEM262> GetConnectedHotelRates(string intAccNo)
        {
            return base.SQL.GetQuery<HEM262>($@"
                               select
	                                *
	                                from hem262
	                                where
			                                intAccNo = @IntAccNo",
                                    new
                                    {
                                        IntAccNo = intAccNo
                                    });
        }

        public List<HEM750> GetRateTierStatus(string intAccNo, string tierNo)
        {
            return base.SQL.GetQuery<HEM750>($@"
                               select
                                    *
	                                    from HEM750
	                                    where
			                                    intaccno = @IntAccNo
		                                    and	tierno = @TierNo",
                                    new
                                    {
                                        IntAccNo = intAccNo,
                                        TierNo = tierNo
                                    });
        }

        public List<HEM051> GetHem051(string hotelCode, string intAccNo)
        {
            return base.SQL.GetQuery<HEM051>(@"
                                select
	                                *
	                                from hem051
	                                left join HEM264 on hem264.roomTypeCd = hem051.roomtypecd
	                                where
			                                hem051.hotelcd = @HotelCode
		                                and coalesce(stsdelete, '') = ''
		                                and hem264.intAccNo = @IntAccNo
                                ",
                                new {
                                    HotelCode = hotelCode,
                                    IntAccNo = intAccNo 
                                });
        }

        public bool InsertHem262(string tierNo, string intAccNo, string updater, string dateTimeNow)
        {
            return base.SQL.ExecuteQuery(@"
                        insert into hem262
                        (
                        tierNo
                        ,ratePlanCd
                        ,intAccNo
                        ,stsFitGit
                        ,updater
                        ,lastupdate
                        )
                        values
                        (
                        @RateTierNo,
                        @RateTierNo,
                        @IntAccNo,
                        'F',
                        @Updater,
                        @DateTimeNow
                        )",
                        new
                        {
                            RateTierNo = tierNo,
                            IntAccNo = intAccNo,
                            Updater = updater,
                            DateTimeNow = dateTimeNow
                        });
        }

        public bool InsertHem750(string intAccNo, string rateTierNo, string stsNoAvailUpdate, string stsNoRateUpdate, string stsNoUpdate, List<string> listRoomTypeCode, string updater, string dateTimeNow)
        {
            listRoomTypeCode.ForEach(r => base.SQL.ExecuteQuery(@"
                                    insert into hem750
	                                (intaccno
	                                ,roomtypecd
	                                ,tierno
                                    ,stsnoavailupdate
                                    ,stsnorateupdate
                                    ,stsnoupdate
	                                ,updater
	                                ,lastupdate
	                                )
	                                values
	                                (@IntAccNo
	                                ,@RoomTypeCode
	                                ,@TierNo
                                    ,@StsNoAvailUpdate
                                    ,@StsNoRateUpdate
                                    ,@StsNoUpdate
	                                ,@Updater
	                                ,@LastUpdate
	                                )",
                                    new
                                    {
                                        IntAccNo = intAccNo,
                                        RoomTypeCode = r,
                                        TierNo = rateTierNo,
                                        StsNoAvailUpdate= stsNoAvailUpdate,
                                        StsNoRateUpdate= stsNoRateUpdate,
                                        StsNoUpdate= stsNoUpdate,
                                        Updater = updater,
                                        LastUpdate = dateTimeNow
                                    }));

            return true;
        }

        public bool UpdateHem750(string rateTierNo, string intAccNo, string stsNoAvailUpdate, string stsNoRateUpdate, string stsNoUpdate, string updater, string dateTimeNow)
        {
            return base.SQL.ExecuteQuery(@"
                        update hem750 set
	                        stsnoavailupdate = @StsNoAvailUpdate
	                        ,stsnorateupdate = @StsNoRateUpdate
	                        ,stsnoupdate = @StsNoUpdate
	                        ,updater = @Updater
	                        ,lastupdate = @LastUpdate
                        where
		                        tierno = @RateTierNo
	                        and intaccno = @IntAccNo
                        ", 
                        new {
                            IntAccNo = intAccNo,
                            RateTierNo = rateTierNo,
                            StsNoAvailUpdate = stsNoAvailUpdate,
                            StsNoRateUpdate = stsNoRateUpdate,
                            StsNoUpdate = stsNoUpdate,
                            Updater = updater,
                            LastUpdate = dateTimeNow
                        });
        }

        public bool DeleteHem750(string intAccNo, string rateTierNo)
        {
            return base.SQL.ExecuteQuery(@"
                        delete from hem750
	                        where
			                    intaccno = @IntAccNo
		                    and tierno = @RateTierNo
                        ",
                       new
                       {
                           IntAccNo = intAccNo,
                           RateTierNo = rateTierNo
                       });
        }

        public bool DeleteHem262(string intAccNo, string rateTierNo)
        {
            return base.SQL.ExecuteQuery(@"
                        delete from hem262
	                        where
			                    intaccno = @IntAccNo
		                    and tierno = @RateTierNo
                        ",
                      new
                      {
                          IntAccNo = intAccNo,
                          RateTierNo = rateTierNo
                      });
        }
    }
}
