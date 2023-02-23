using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using V7_API_PSBO.Base;
using V7_API_PSBO.Pelican;
using V7_API_PSBO_BUSINESS_LAYER.M766;
using V7_API_PSBO_DATA_MODEL.M766;
using V7_API_PSBO_DATA_MODEL.Pelican;

namespace V7_API_PSBO.Controllers
{
    [RoutePrefix("api/m766")]
    public class M766Controller : PelicanApiController
    {
        [HttpPost]
        [Route("{ROUTE}")]
        public HttpResponseMessage Main(string ROUTE)
        {
            var response = new HttpResponseMessage();

            try
            {
                base.AuthUser();

                var bl = new M766BusinessLayer(base.user);

                object responseModel;

                switch (ROUTE)
                {
                    case "get-room-mapping":
                        responseModel = bl.GetRoomTypeMapping();
                        break;

                    case "connect-room":
                        responseModel = bl.ConnectRoom(GetPostRequestAs<RoomConnectModel>());
                        break;

                    case "update-room":
                        responseModel = bl.UpdateConnectedRoom(GetPostRequestAs<RoomConnectModel>());
                        break;

                    case "disconnect-room":
                        responseModel = bl.DisconnetRoom(GetPostRequestAs<RoomConnectModel>());
                        break;

                    case "get-rate-mapping":
                        responseModel = bl.GetRatePlanMapping();
                        break;

                    case "connect-rate":
                        responseModel = bl.ConnectRate(GetPostRequestAs<RateConnectModel>());
                        break;

                    case "update-rate":
                        responseModel = bl.UpdateConnectedRate(GetPostRequestAs<RateConnectModel>());
                        break;

                    case "disconnect-rate":
                        responseModel = bl.DisconnectRate(GetPostRequestAs<RateConnectModel>());
                        break;

                    case "update-connectivity-status":
                        responseModel = bl.UpdateConnectivityStatus(GetPostRequestAs<ConnectivityStatusModel>());
                        break;

                    case "get-partner-info":
                        responseModel = bl.GetPartnerInfo();
                        break;

                    default:
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound
                        };

                }

                response.Content = JsonContent(responseModel);
            }
            catch (Exception e)
            {
                ApiExceptionHandler(response, e);
            }


            return response;
        }

        //[HttpPost]
        //[Route("disconnect-rate")]
        //public HttpResponseMessage DisconnectRate()
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();

        //    try
        //    {
        //        base.AuthUser();

        //        var req = GetPostRequestAs<RateConnectModel>();

        //        var result = new M766BusinessLayer(base.user).DisconnectRate(req);

        //        response.Content = JsonContent(result);
        //    }
        //    catch (Exception e)
        //    {
        //        ApiExceptionHandler(response, e);
        //    }

        //    return response;
        //}     
    }
}
