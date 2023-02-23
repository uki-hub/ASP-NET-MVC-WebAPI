using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using V7_API_PSBO.Base;
using V7_API_PSBO_DATA_MODEL.Auth;
using V7_API_PSBO_DATA_MODEL.Pelican;

namespace V7_API_PSBO.Pelican
{
    public class PelicanApiController : BaseApiController
    {
        internal UserModel user { get; private set; }

        internal void AuthUser()
        {
            var req = GetPostRequestAs<dynamic>();
            var userCode = (string)req.UserCode;
            var hotelCode = (string)req.HotelCode;
            var intAccNo = (string)req.IntAccNo;

            if (String.IsNullOrEmpty(userCode))
                throw new PelicanApiException("Invalid User", PelicanApiException.exceptionType.UNAUTHORIZED);

            user = new UserModel
            {
                UserCode = userCode,
                HotelCode = hotelCode,
                IntAccNo = intAccNo
            };
        }

        internal override void ApiExceptionHandler(HttpResponseMessage response, Exception e)
        {
            string errorMessage;
            HttpStatusCode statusCode;

            if (e is PelicanApiException)
            {
                var ep = (PelicanApiException)e;

                switch (ep.Type)
                {
                    case PelicanApiException.exceptionType.WARNING:
                    case PelicanApiException.exceptionType.ERROR:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;

                    case PelicanApiException.exceptionType.UNAUTHORIZED:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;

                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                errorMessage = ep.PMessage;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                errorMessage = e.ToString();
            }

            var model = new PelicanResponseModel<string>
            {
                Data = "",
                Success = false,
                Messages = new List<string> { errorMessage }
            };

            response.StatusCode = statusCode;
            response.Content = JsonContent(model);
        }
    }
}