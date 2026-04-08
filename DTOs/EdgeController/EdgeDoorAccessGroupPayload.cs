using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeDoorAccessGroupPayload
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public SingleFirmwareDoorAccessGroupDTO? Info { get; set; }
    }

    public class EdgeSetDoorAccessGroupPayload
    {
        public int DoorAG_num { get; set; }

        [JsonProperty("payload")]
        public SingleFirmwareDoorAccessGroupDTO? Info { get; set; }
    }
    
    public class SingleFirmwareDoorAccessGroupDTO
    {
        [JsonProperty("doors")]
        public List<Tied_Door_Props>? Doors { get; set; }

        [JsonProperty("tzid")]
        public List<int>? Schedules { get; set; }
    }
}
