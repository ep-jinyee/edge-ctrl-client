using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeSerialPayload
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public List<GetFirmwareSerialDTO>? Info { get; set; }
    }

    public class GetFirmwareSerialDTO
    {
        [JsonProperty("mode")]
        public int OSDPMode { get; set; }

        [JsonProperty("baud_rate")]
        public int BaudRate { get; set; }

        [JsonProperty("property")]
        public List<GetSingleFirmwareSerialDTO>? Property { get; set; }
    }
    public class GetSingleFirmwareSerialDTO
    {
        [JsonProperty("type")]
        public int SerialType { get; set; }

        [JsonProperty("scbk")]
        public string? Primary_SCBK { get; set; }

        [JsonProperty("phy_addr")]
        public int PhyAddress { get; set; }

        [JsonProperty("pdid", NullValueHandling = NullValueHandling.Ignore)]
        public int? TiedW2O_8IO { get; set; }

        [JsonProperty("rdid", NullValueHandling = NullValueHandling.Ignore)]
        public int? TiedReader { get; set; }
    }

    public class EdgeSetSerialPayload
    {
        [JsonProperty("mode")]
        public int OSDPMode { get; set; }

        [JsonProperty("baud_rate")]
        public int BaudRate { get; set; }

        [JsonProperty("info")]
        public List<SingleFirmwareSerialDTO>? Info { get; set; }
    }

    public class SingleFirmwareSerialDTO
    {
        [JsonProperty("type")]
        public int SerialType { get; set; }

        [JsonProperty("scbk")]
        public string? Primary_SCBK { get; set; }

        [JsonProperty("phy_addr")]
        public int PhyAddress { get; set; }

        [JsonProperty("property")]
        public TiedProperty? TiedProperty { get; set; }
    }

    public class TiedProperty
    {
        [JsonProperty("pdid", NullValueHandling = NullValueHandling.Ignore)]
        public int? TiedW2O_8IO { get; set; }

        [JsonProperty("rdid", NullValueHandling = NullValueHandling.Ignore)]
        public int? TiedReader { get; set; }
    }
}
