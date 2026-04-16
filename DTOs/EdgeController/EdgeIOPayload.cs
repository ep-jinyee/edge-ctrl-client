using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeIOPayload
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public List<SingleFirmwareIO_DTO>? Info { get; set; }
    }

    public class EdgeSetIOPayload
    {
        [JsonProperty("pdid")]
        public int PD_Id { get; set; }

        [JsonProperty("type")]
        public int PD_Type { get; set; }

        [JsonProperty("info")]
        public List<SingleFirmwareIO_DTO>? Info { get; set; }
    }

    public class SingleFirmwareIO_DTO
    {
        [JsonProperty("id")]
        public int IO_Id { get; set; }

        [JsonProperty("en_tz")]
        public int ActiveSchedule { get; set; }

        [JsonProperty("app_type")]
        public int IOType { get; set; }

        [JsonProperty("hw_type")]
        public int Hw_type { get; set; }

        [JsonProperty("input_trig_mode")]
        public int TriggeredMode { get; set; }

        [JsonProperty("trigger_delay")]
        public int TriggerDelaySecond { get; set; }

        [JsonProperty("normal_delay")]
        public int NormalDelaySecond { get; set; }

        [JsonProperty("property")]
        public Tied_Props? Tied_Props { get; set; }

        [JsonProperty("raw-threshold")]
        public List<TriggerRange> OnOffOpenShortThreshold { get; set; }
    }

    public class TriggerRange
    {
        [JsonProperty("min")]
        public int MinRange { get; set; }

        [JsonProperty("max")]
        public int MaxRange { get; set; }
    }

    public class Tied_Props
    {
        [JsonProperty("pdid")]
        public int PD_Id { get; set; }

        [JsonProperty("drid", NullValueHandling = NullValueHandling.Ignore)]
        public int? Door_Id { get; set; }

        [JsonProperty("rdid", NullValueHandling = NullValueHandling.Ignore)]
        public int? Reader_Id { get; set; }

        [JsonProperty("bind_to_zone", NullValueHandling = NullValueHandling.Ignore)]
        public int? BindToZone { get; set; }
    }
}
