using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeCardPayload
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public SingleFirmwareCardDTO? Info { get; set; }
    }

    public class EdgeSetCardPayload : EdgeCardInfo
    {
        [JsonProperty("info")]
        public SingleFirmwareCardDTO? Info { get; set; }
    }
    
    public class SingleFirmwareCardDTO
    {
        [JsonProperty("type")]
        public int CardType { get; set; }

        [JsonProperty("start")]
        public string? CreatedAt { get; set; }

        [JsonProperty("end")]
        public string? ValidUntil { get; set; }

        [JsonProperty("pin")]
        public List<string>? CardPins { get; set; }

        [JsonProperty("access_grp_id")]
        public List<int>? DoorAccessGroups {  get; set; }
    }

    public class EdgeCardInfo
    {
        [JsonProperty("cardno")]
        public string? CardNum { get; set; }

        [JsonProperty("base")]
        public int? CardNumBase { get; set; }
    }

    public class CardCountInfoDTO
    {
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
