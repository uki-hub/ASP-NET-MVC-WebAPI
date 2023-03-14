using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V7_API_BASE.Base;
using V7_API_BASE.Models;
using V7_API_PSBO_DATA_ACCESS.Base;

namespace V7_API_PSBO_DATA_ACCESS.M766
{
    public class RoomTypeMappingDA : BaseDataAccess
    {
        public List<HEM051> GetHotelRooms(string hotelCode)
        {
            return base.SQL.GetQuery<HEM051>($@"
                                select
	                                *
	                                from hem051
	                                where
			                                hem051.hotelcd = @HotelCode
		                                and coalesce(stsdelete, '') = ''
                                    order by dispordnum",
                                    new
                                    {
                                        HotelCode = hotelCode
                                    });
        }

        public List<HEM264> GetConnectedHotelRooms(string hotelCode, string intAccNo)
        {
            return base.SQL.GetQuery<HEM264>($@"
                               select
	                                hem264.*
	                                from hem051
	                                left join HEM264 on hem264.roomTypeCd = hem051.roomtypecd
	                                where
			                                hem051.hotelcd = @HotelCode
		                                and coalesce(stsdelete, '') = ''
		                                and hem264.intAccNo = @IntAccNo",
                                    new
                                    {
                                        HotelCode = hotelCode,
                                        IntAccNo = intAccNo
                                    });
        }

        public List<HEM262> GetHem262(string intAccNo)
        {
            return base.SQL.GetQuery<HEM262>(@"
                               select
	                                *
	                                from hem262
	                                join HEReserv..her131 on her131.tierno = hem262.tierNo
	                                where
			                                intAccNo = @IntAccNo
		                                and (coalesce(her131.stsactv, '') = '' or her131.stsactv = 'A')",
                                    new
                                    {
                                        IntAccNo = intAccNo
                                    });
        }

        public bool InsertHem264(string intAccNo, string roomTypeCode, int? maxAdult, int? maxXtraBed, string updater, string dateTimeNow)
        {
            return base.SQL.ExecuteQuery(@"
                                insert into hem264 
	                            (intAccNo
	                            ,roomTypeCd
	                            ,prRoomType
                                ,maxAdult
                                ,maxXtraBed
	                            ,updater
	                            ,lastupdate
	                            )
	                            values
	                            (@IntAccNo
	                            ,@RoomTypeCode
	                            ,@RoomTypeCode
	                            ,@MaxAdult
	                            ,@MaxXtraBed
	                            ,@Updater
	                            ,@DateTimeNow
	                            )",
                                    new
                                    {
                                        IntAccNo = intAccNo,
                                        RoomTypeCode = roomTypeCode,
                                        MaxAdult = maxAdult,
                                        MaxXtraBed = maxXtraBed,
                                        Updater = updater,
                                        DateTimeNow = dateTimeNow,
                                    });
        }

        public bool InsertHem750(string intAccNo, string roomTypeCode, List<string> listTierNo, string updater, string dateTimeNow)
        {
            listTierNo.ForEach(t => base.SQL.ExecuteQuery(@"
                                    insert into hem750
	                                (intaccno
	                                ,roomtypecd
	                                ,tierno
	                                ,updater
	                                ,lastupdate
	                                )
	                                values
	                                (@IntAccNo
	                                ,@RoomTypeCode
	                                ,@TierNo
	                                ,@Updater
	                                ,@LastUpdate
	                                )",
                                    new
                                    {
                                        IntAccNo = intAccNo,
                                        RoomTypeCode = roomTypeCode,
                                        TierNo = t,
                                        Updater = updater,
                                        LastUpdate = dateTimeNow
                                    }));

            return true;
        }

        public bool UpdateHem264(string intAccNo, string roomTypeCode, int? maxAdult, int? maxXtraBed, string updater, string dateTimeNow)
        {
            return base.SQL.ExecuteQuery(@"
                update hem264 set
		            maxAdult = @MaxAdult
		            ,maxXtraBed = @MaxXtraBed
		            ,updater = @Updater
		            ,lastupdate = @DateTimeNow
	            where
			            intAccNo = @IntAccNo
		            and roomTypeCd = @RoomTypeCode
                ",
                new
                {
                    IntAccNo = intAccNo,
                    RoomTypeCode = roomTypeCode,
                    MaxAdult = maxAdult,
                    MaxXtraBed = maxXtraBed,
                    Updater = updater,
                    DateTimeNow = dateTimeNow,
                });
        }

        public bool DeleteHem750(string intAccNo, string roomTypeCode)
        {
            return base.SQL.ExecuteQuery(@"
                        delete from hem750
	                        where
			                    intaccno = @IntAccNo
		                    and roomtypecd = @RoomTypeCode
                        ",
                       new
                       {
                           IntAccNo = intAccNo,
                           RoomTypeCode = roomTypeCode
                       });
        }

        public bool DeleteHem264(string intAccNo, string roomTypeCode)
        {
            return base.SQL.ExecuteQuery(@"
                        delete from hem264
			                where
				                intaccno = @IntAccNo
			                and roomtypecd = @RoomTypeCode
                        ",
                       new
                       {
                           IntAccNo = intAccNo,
                           RoomTypeCode = roomTypeCode
                       });
        }
    }
}
