using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V7_API_PSBO_BUSINESS_LAYER.Base;
using V7_API_PSBO_DATA_ACCESS.M766;
using V7_API_PSBO_DATA_MODEL.Auth;
using V7_API_PSBO_DATA_MODEL.M766;
using V7_API_PSBO_DATA_MODEL.Pelican;

namespace V7_API_PSBO_BUSINESS_LAYER.M766
{
    public class M766BusinessLayer : BaseBusinessLayer
    {
        private UserModel _userModel { get; set; }

        private M766DataAccess _da { get; set; }

        public M766BusinessLayer(UserModel userModel)
        {
            _da = new M766DataAccess();

            _userModel = userModel;

            if (String.IsNullOrEmpty(_userModel.IntAccNo))
            {
                var hem261 = _da.GetHem261(_userModel.HotelCode);
                _userModel.IntAccNo = hem261.intAccNo;
            }
        }

        #region Menu 1
        public PelicanResponseModel<List<RoomTypeMappingModel>> GetRoomTypeMapping()
        {
            var result = new List<RoomTypeMappingModel>();

            var hotelRooms = _da.RoomTypeMappingDA.GetHotelRooms(_userModel.HotelCode);
            var connectedRooms = _da.RoomTypeMappingDA.GetConnectedHotelRooms(_userModel.HotelCode, _userModel.IntAccNo);

            hotelRooms.ForEach(r =>
            {
                var connectedRoom = connectedRooms.FirstOrDefault(_r => _r.roomTypeCd == r.roomtypecd);

                var newItem = new RoomTypeMappingModel
                {
                    RoomTypeCode = r.roomtypecd,
                    RoomTypeName = r.roomtypenm,
                    IsConnected = connectedRoom != null,
                };

                if (connectedRoom != null)
                {
                    if (connectedRoom.maxAdult.HasValue)
                        newItem.MaxAdult = Convert.ToInt32(connectedRoom.maxAdult.Value);

                    if (connectedRoom.maxXtraBed.HasValue)
                        newItem.MaxXtraBed = Convert.ToInt32(connectedRoom.maxXtraBed.Value);
                }

                result.Add(newItem);
            });

            return new PelicanResponseModel<List<RoomTypeMappingModel>>
            {
                Data = result,
                Success = true
            };
        }

        public PelicanResponseModel<object> ConnectRoom(RoomConnectModel roomConnectModel)
        {
            var da = _da.RoomTypeMappingDA;

            try
            {
                da.BeginTransaction();

                da.InsertHem264(
                    intAccNo: _userModel.IntAccNo,
                    roomTypeCode: roomConnectModel.RoomTypeCode,
                    maxAdult: roomConnectModel.MaxAdult,
                    maxXtraBed: roomConnectModel.MaxXtraBed,
                    updater: _userModel.UserCode,
                    dateTimeNow: DateTimeNow_yyyyMMddHHmmss
                    );

                var listTierNo = da.GetHem262(_userModel.IntAccNo).Select(h => h.tierNo).ToList();

                da.DeleteHem750(
                    intAccNo: _userModel.IntAccNo,
                    roomTypeCode: roomConnectModel.RoomTypeCode
                    );

                da.InsertHem750(
                    intAccNo: _userModel.IntAccNo,
                    roomTypeCode: roomConnectModel.RoomTypeCode,
                    listTierNo: listTierNo,
                    updater: _userModel.UserCode,
                    dateTimeNow: DateTimeNow_yyyyMMddHHmmss
                    );

                da.Commit();
            }
            catch (Exception e)
            {
                da.Rollback();

                throw e;
            }
            finally
            {
                da.EndTransaction();
            }

            return new PelicanResponseModel<object>
            {
                Success = true
            };
        }

        public PelicanResponseModel<object> UpdateConnectedRoom(RoomConnectModel roomConnectModel)
        {
            var da = _da.RoomTypeMappingDA;

            try
            {
                da.BeginTransaction();

                da.UpdateHem264(
                    intAccNo: _userModel.IntAccNo,
                    roomTypeCode: roomConnectModel.RoomTypeCode,
                    maxAdult: roomConnectModel.MaxAdult,
                    maxXtraBed: roomConnectModel.MaxXtraBed,
                    updater: _userModel.UserCode,
                    dateTimeNow: DateTimeNow_yyyyMMddHHmmss
                    );

                da.Commit();
            }
            catch (Exception e)
            {
                da.Rollback();

                throw e;
            }
            finally
            {
                da.EndTransaction();
            }

            return new PelicanResponseModel<object>
            {
                Success = true
            };
        }

        public PelicanResponseModel<object> DisconnetRoom(RoomConnectModel roomConnectModel)
        {
            var da = _da.RoomTypeMappingDA;

            try
            {
                da.BeginTransaction();

                da.DeleteHem264(
                    intAccNo: _userModel.IntAccNo,
                    roomTypeCode: roomConnectModel.RoomTypeCode
                    );

                da.DeleteHem750(
                    intAccNo: _userModel.IntAccNo,
                    roomTypeCode: roomConnectModel.RoomTypeCode
                    );

                da.Commit();
            }
            catch (Exception e)
            {
                da.Rollback();

                throw e;
            }
            finally
            {
                da.EndTransaction();
            }

            return new PelicanResponseModel<object>
            {
                Success = true
            };
        }
        #endregion

        #region Menu 2
        public PelicanResponseModel<List<RatePlanMappingModel>> GetRatePlanMapping()
        {
            var result = new List<RatePlanMappingModel>();

            var hotelRates = _da.RatePlanMappingDA.GetHotelRates(_userModel.HotelCode);
            var connectedRates = _da.RatePlanMappingDA.GetConnectedHotelRates(_userModel.IntAccNo);

            hotelRates.ForEach(r =>
            {
                var rateTierStatus = _da.RatePlanMappingDA.GetRateTierStatus(_userModel.IntAccNo, r.tierno);
                var isNoUpdateAvailability = rateTierStatus.FirstOrDefault(s => s.stsnoavailupdate == "Y") != null;
                var isNoUpdateRate = rateTierStatus.FirstOrDefault(s => s.stsnorateupdate == "Y") != null;
                var isNoUpdateRestriction = rateTierStatus.FirstOrDefault(s => s.stsnoupdate == "Y") != null;

                result.Add(new RatePlanMappingModel
                {
                    RateTierNo = r.tierno,
                    RateTierCode = r.tiercode,
                    RatePlanName = r.tiername,
                    StsConnected = connectedRates.FirstOrDefault(_r => _r.tierNo == r.tierno) != null,
                    StsNoUpdateAvailability = isNoUpdateAvailability,
                    StsNoUpdateRate = isNoUpdateRate,
                    StsNoUpdate = isNoUpdateRestriction

                });
            });

            //var orderedResult = result.OrderBy(r => r.RateTierCode).ToList();

            return new PelicanResponseModel<List<RatePlanMappingModel>>
            {
                Data = result,
                Success = true
            };
        }

        public PelicanResponseModel<object> ConnectRate(RateConnectModel rateConnectModel)
        {
            var da = _da.RatePlanMappingDA;

            try
            {
                da.BeginTransaction();

                da.InsertHem262(
                    intAccNo: _userModel.IntAccNo,
                    tierNo: rateConnectModel.RateTierNo,
                    updater: _userModel.UserCode,
                    dateTimeNow: DateTimeNow_yyyyMMddHHmmss
                    );

                var listRoomTypeCode = da.GetHem051(_userModel.HotelCode, _userModel.IntAccNo).Select(h => h.roomtypecd).ToList();

                da.DeleteHem750(
                    intAccNo: _userModel.IntAccNo,
                    rateTierNo: rateConnectModel.RateTierNo
                    );

                da.InsertHem750(
                    intAccNo: _userModel.IntAccNo,
                    rateTierNo: rateConnectModel.RateTierNo,
                    stsNoAvailUpdate: rateConnectModel.StsNoAvailUpdate ? "Y" : null,
                    stsNoRateUpdate: rateConnectModel.StsNoRateUpdate ? "Y" : null,
                    stsNoUpdate: rateConnectModel.StsNoUpdate ? "Y" : null,
                    listRoomTypeCode: listRoomTypeCode,
                    updater: _userModel.UserCode,
                    dateTimeNow: DateTimeNow_yyyyMMddHHmmss
                    );

                da.Commit();
            }
            catch (Exception e)
            {
                da.Rollback();

                throw e;
            }
            finally
            {
                da.EndTransaction();
            }

            return new PelicanResponseModel<object>
            {
                Success = true
            };
        }

        public PelicanResponseModel<object> UpdateConnectedRate(RateConnectModel rateConnectModel)
        {
            var da = _da.RatePlanMappingDA;

            try
            {
                da.BeginTransaction();

                da.UpdateHem750(
                    intAccNo: _userModel.IntAccNo,
                    rateTierNo: rateConnectModel.RateTierNo,
                    stsNoAvailUpdate: rateConnectModel.StsNoAvailUpdate ? "Y" : null,
                    stsNoRateUpdate: rateConnectModel.StsNoRateUpdate ? "Y" : null,
                    stsNoUpdate: rateConnectModel.StsNoUpdate ? "Y" : null,
                    updater: _userModel.UserCode,
                    dateTimeNow: DateTimeNow_yyyyMMddHHmmss
                    );

                da.Commit();
            }
            catch (Exception e)
            {
                da.Rollback();

                throw e;
            }
            finally
            {
                da.EndTransaction();
            }

            return new PelicanResponseModel<object>
            {
                Success = true
            };
        }

        public PelicanResponseModel<object> DisconnectRate(RateConnectModel rateConnectModel)
        {
            var da = _da.RatePlanMappingDA;

            try
            {
                da.BeginTransaction();

                da.DeleteHem262(
                    intAccNo: _userModel.IntAccNo,
                    rateTierNo: rateConnectModel.RateTierNo
                    );

                da.DeleteHem750(
                    intAccNo: _userModel.IntAccNo,
                    rateTierNo: rateConnectModel.RateTierNo
                    );

                da.Commit();
            }
            catch (Exception e)
            {
                da.Rollback();

                throw e;
            }
            finally
            {
                da.EndTransaction();
            }

            return new PelicanResponseModel<object>
            {
                Success = true
            };
        }
        #endregion

        #region Menu 3
        public PelicanResponseModel<ConnectivityStatusModel> UpdateConnectivityStatus(ConnectivityStatusModel model)
        {
            var success = _da.ConnectivityStatusDA.UpdateConnectivity(
                                                    intAccNo: _userModel.IntAccNo, 
                                                    stsActive: model.StatusActive, 
                                                    stsSendRsv: model.StatusSendReservation, 
                                                    startTime: DateTimeTo_yyyMMddHHmmss(model.StartDateTime));

            return new PelicanResponseModel<ConnectivityStatusModel>
            {
                Success = success
            };
        }
        #endregion

        #region Others
        public PelicanResponseModel<PartnerInfoModel> GetPartnerInfo()
        {
            var hem261 = _da.GetHem261(_userModel.HotelCode);

            var startDateTime = hem261.startTime;
            if (!startDateTime.HasValue)
                startDateTime = new DateTime();

            var response = new PelicanResponseModel<PartnerInfoModel>();

            if (hem261 != null)
                response.Data = new PartnerInfoModel
                {
                    IntAccNo = hem261.intAccNo,
                    AccID = hem261.accID,
                    IntPartner = hem261.intPartner,
                    HotelCode = hem261.hotelCd,
                    StatusActive = hem261.stsactv == "A",
                    StatusSendReservation = hem261.stsSendRsv == "Y",
                    StartDateTime = startDateTime.Value
                };

            response.Success = true;

            return response;
        }
        #endregion
    }
}
