using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient.DTOs
{
    internal class EdgePlusResponsePayload
    {
        [JsonProperty("payload")]
        public ResponsePayload? Payload { get; set; }

        [JsonProperty("info")]
        public JToken? Info { get; set; }
    }

    internal class ResponsePayload
    {
        [JsonProperty("errCode")]
        public int ErrCode { get; set; }

        [JsonProperty("info")]
        public JToken? Info { get; set; }
    }
}
