using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPoC_2
{
    class OdataServiceSdlResponse
    {
        [JsonProperty(PropertyName = "error")]
        public OdataServiceSdlErrorResponse Error { get; set; }
    }
}
