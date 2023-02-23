using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;

namespace V7_API_PSBO.Base
{
    public abstract class BaseApiController : ApiController
    {       
        internal string GetPostRequestRAW() => Request.Content.ReadAsStringAsync().Result;

        internal T GetPostRequestAs<T>() => JsonConvert.DeserializeObject<T>(GetPostRequestRAW());

        internal T FromJSON<T>(string json) => JsonConvert.DeserializeObject<T>(json);

        internal string ToJSON(object o) => JsonConvert.SerializeObject(o, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });

        //internal ObjectContent JsonContent(string json) => new ObjectContent<string>(json, new JsonMediaTypeFormatter(), "application/json");
        //internal ObjectContent JsonContent(object o) => new ObjectContent<string>(ToJSON(o), new JsonMediaTypeFormatter(), "application/json");

        internal StringContent JsonContent(string json) => new StringContent(json, Encoding.UTF8, "application/json");
        internal StringContent JsonContent(object o) => new StringContent(ToJSON(o), Encoding.UTF8, "application/json");

        internal void ValidateRequest(dynamic req, string[] nonEmptyFields = null)
        {
            string get(string key) => (string)req[key];

            var _nonEmptyFields = nonEmptyFields ?? new string[] { };
            var emptyFields = new List<string>();

            foreach (var field in _nonEmptyFields)
                if (String.IsNullOrEmpty(get(field)))
                    emptyFields.Add(field);

            if (emptyFields.Count > 0)
                throw new Exception($"Parameters {String.Join(", ", emptyFields)} can't be null", ToJSON(req));
        }

        abstract internal void ApiExceptionHandler(HttpResponseMessage response, Exception e);
    }
}