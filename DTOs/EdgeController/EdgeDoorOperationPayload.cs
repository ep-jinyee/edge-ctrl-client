using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeDoorPulseOpenPayload
    {
        [JsonProperty("pdid")]
        public int OSDPAddress = 255;

        [JsonProperty("id")]
        public List<int> DoorId = new();
    }

    public class EdgeDoorSecurityLevelPayload
    {
        [JsonProperty("security_lvl")]
        public FIRMWARE_SECURITY_LEVEL SecurityLevel;

        [JsonProperty("info")]
        public List<EdgeDoorPulseOpenPayload> DoorSecurityLevelInfos = new();
    }

    public class EdgeDoorInhibitPayload : EdgeDoorPulseOpenPayload
    {
        public FIRMWARE_INHIBIT_STATUS InhibitStatus;
    }

    public enum FIRMWARE_SECURITY_LEVEL
    {
        LOW = 0,
        HIGH = 1
    }

    public enum FIRMWARE_INHIBIT_STATUS
    {
        OFF = 0,
        ON = 1,
    }
}
