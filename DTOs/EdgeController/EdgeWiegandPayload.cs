using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeWiegandPayload
    {
        [JsonProperty("cardcode")]
        public List<int>? Cardcode { get; set; }

        [JsonProperty("evenparity_pos")]
        public List<int>? EvenparityPos { get; set; }

        [JsonProperty("oddparity_pos")]
        public List<int>? OddparityPos { get; set; }

        [JsonProperty("cardlen")]
        public int Cardlen { get; set; }

        [JsonProperty("convertion")]
        public int Convertion { get; set; }
    }
}
