using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPoC_2
{
    class OdataServiceSdlErrorResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
