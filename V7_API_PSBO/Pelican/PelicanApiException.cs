using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace V7_API_PSBO.Pelican
{
    public class PelicanApiException : Exception
    {
        public enum exceptionType
        {
            WARNING,
            ERROR,
            UNAUTHORIZED
        }

        private string _message { get; set; }
        private string _json { get; set; }

        public exceptionType Type { get; set; }
        public string PMessage { get => String.IsNullOrEmpty(_json) ? _message : $"{_message}\njson details:\n{_json}"; }

        public PelicanApiException(string errorMessage, exceptionType type = exceptionType.ERROR, string json = "")
        {
            _message = errorMessage;
            Type = type;
            _json = json;
        }
    }
}