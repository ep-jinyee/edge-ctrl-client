using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgePlusApiResponse<T>
    {
        [JsonProperty("payload")]
        public Payload? Payload { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public T? Info { get; set; }
    }

    public class Payload
    {
        [JsonProperty("errCode")]
        public int ErrCode { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public string? Info { get; set; }
    }
}
