using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeSchedulePayload
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public SingleFirmwareScheduleDTO? Info { get; set; }
    }

    public class EdgeSetSchedulePayload
    {
        public int Schedule_num { get; set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public SingleFirmwareScheduleDTO? Info { get; set; }
    }
    
    public class SingleFirmwareScheduleDTO
    {
        [JsonProperty("1", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? MondaySchedule { get; set; }

        [JsonProperty("2", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? TuesdaySchedule { get; set; }

        [JsonProperty("3", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? WednesdaySchedule { get; set; }

        [JsonProperty("4", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? ThursdaySchedule { get; set; }

        [JsonProperty("5", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? FridaySchedule { get; set; }

        [JsonProperty("6", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? SaturdaySchedule { get; set; }

        [JsonProperty("7", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? SundaySchedule { get; set; }

        [JsonProperty("8", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingleIntervalDTO>? HolidaySchedule { get; set; }
    }

    public class SingleIntervalDTO
    {
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public string? StartTime { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }
    }
}
