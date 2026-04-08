using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeReaderPayload
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public List<SingleFirmwareReaderDTO>? Info { get; set; }
    }

    public class EdgeSetReaderPayload
    {
        [JsonProperty("pdid")]
        public int PD_Id { get; set; }

        [JsonProperty("info")]
        public List<SingleFirmwareReaderDTO>? Info { get; set; }
    }
    
    public class SingleFirmwareReaderDTO
    {
        [JsonProperty("rdid")]
        public int Rd_Id { get; set; }

        [JsonProperty("type")]
        public int ReaderType { get; set; }

        [JsonProperty("loop_detector")]
        public int Tied_IO { get; set; }

        [JsonProperty("std_wieg_en")]
        public List<int>? StandardWiegand { get; set; }

        [JsonProperty("en_tz")]
        public int ActiveSchedule {  get; set; }

        [JsonProperty("dir")]
        public int Direction {  get; set; }

        [JsonProperty("tied_dr_props")]
        public List<Tied_Door_Props>? Tied_Door_Props { get; set; }

        [JsonProperty("cust_wieg_id")]
        public List<int>? CustomWiegand { get; set; }

        [JsonProperty("access_limit")]
        public Access_Limit_Zone? Access_Limit_Zone { get; set; }
    }

    public class Tied_Door_Props
    {
        [JsonProperty("pdid")]
        public int PD_Id { get; set; }

        [JsonProperty("drid")]
        public int Door_Id { get; set; }
    } 

    public class Access_Limit_Zone
    {
        [JsonProperty("en_tz")]
        public int ScheduleNum { get; set; }

        [JsonProperty("bind_to_zone")]
        public int BindToZone { get; set; }
    }
}
