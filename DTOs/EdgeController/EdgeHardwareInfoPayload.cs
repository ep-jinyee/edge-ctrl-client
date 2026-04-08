using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeHardwareInfoResponse
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public FirmwareHardwareInfoDTO? Info { get; set; }
    }
    
    public class FirmwareHardwareInfoDTO
    {
        [JsonProperty("Model")]
        public string? Model { get; set; }

        [JsonProperty("Serial Number")]
        public string? SerialNumber { get; set; }

        [JsonProperty("Firmware Version")]
        public string? FirmwareVersion { get; set; }

        [JsonProperty("MAC Address")]
        public string? MAC { get; set; }

        [JsonProperty("Device Mode")]
        public int OperationMode { get; set; }
    }
}
