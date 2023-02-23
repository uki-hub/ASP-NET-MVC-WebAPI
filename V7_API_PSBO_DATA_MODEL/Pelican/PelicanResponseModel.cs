using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_API_PSBO_DATA_MODEL.Pelican
{
    public class PelicanResponseModel<T>
    {
        private List<string> _messages { get; set; }

        public T Data { get; set; }
        public bool Success { get; set; }
        public List<string> Messages {
            set => _messages = value;
            get => _messages != null ? _messages : new List<string> { };
        }
    }
}
