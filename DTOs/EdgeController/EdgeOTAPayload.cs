using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeOTADTO
    {
        [JsonProperty("ota_update_status")]
        public int OTAUpdateStatus { get; set; }

        [JsonProperty("file_size")]
        public int FileSize { get; set; }

        [JsonProperty("update_reset_timer")]
        public bool UpdateResetTimer { get; set; }
    }
}
