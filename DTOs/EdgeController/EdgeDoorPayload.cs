using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeDoorResponse
    {
        public PayloadNetwork? Payload { get; set; }

        [JsonProperty("info")]
        public SingleFirmwareDoorDTO? Info { get; set; }
    }

    public class EdgeSetDoorPayload
    {
        [JsonProperty("pdid")]
        public int PD_Id { get; set; }

        [JsonProperty("info")]
        public List<SingleFirmwareDoorDTO>? Info { get; set; }
    }
    
    public class SingleFirmwareDoorDTO
    {
        [JsonProperty("drid")]
        public int DoorId { get; set; }

        [JsonProperty("door_open_seconds")]
        public int OpenTimeSeconds { get; set; }

        [JsonProperty("door_release_seconds")]
        public int DoorReleaseSecond { get; set; }

        [JsonProperty("relay_id")]
        public int RelayId { get; set; }

        [JsonProperty("zone_in_id")]
        public int ZoneInId { get; set; }

        [JsonProperty("zone_out_id")]
        public int ZoneOutId { get; set; }

        [JsonProperty("en_tz")]
        public int DoorActiveSchedule {  get; set; }

        [JsonProperty("inhibit_tz")]
        public int InhibitSchedule { get; set; }

        [JsonProperty("inhibit_status")]
        public int InhibitStatus { get; set; }

        [JsonProperty("security_off_tz")]
        public int SecurityOffSchedule { get; set; }

        [JsonProperty("security_lvl")]
        public int SecurityLevel { get; set; }

        [JsonProperty("card_plus_pin_en_tz")]
        public List<int>? CardPlusPinSchedules { get; set; }

        [JsonProperty("card_pin_err_limit")]
        public int CardPinErrorLimit { get; set; }

        [JsonProperty("pin")]
        public List<DoorPinStruct>? DoorPins { get; set; }

        [JsonProperty("pin_lockout_en_tz")]
        public int PinLockoutSchedule { get; set; }

        [JsonProperty("pin_lockout_limit")]
        public int PinLockoutLimit { get; set; }

        [JsonProperty("pin_lockout_counter")]
        public int PinLockoutCounter { get; set; }

        [JsonProperty("exit_button_en_tz")]
        public int ExitButtonSchedule { get; set; }

        [JsonProperty("antipassback_err_limit")]
        public int AntiPassbackErrorLimit { get; set; }

        [JsonProperty("antipassback_en_tz")]
        public int AntiPassbackSchedule { get; set; }

        [JsonProperty("buddy_en_tz")]
        public int BuddyModeSchedule { get; set; }

        [JsonProperty("keyed_card_en_tz")]
        public List<int>? KeyedCardSchedule { get; set; }

        [JsonProperty("keyed_card_plus_pin_en_tz")]
        public List<int>? KeyedCardPlusPinSchedule { get; set; }

        [JsonProperty("door_interlock")]
        public DoorInterLockGroup? DoorInterlock { get; set; }

        [JsonProperty("alarm")]
        public AlarmGroup? Alarm { get; set; }
    }

    public class DoorPinStruct
    {
        [JsonProperty("en_tz")]
        public int DoorpinSchedule { get; set; }

        [JsonProperty("pin")]
        public string? Doorpin { get; set; }
    } 

    public class DoorInterLockGroup
    {
        [JsonProperty("en_tz")]
        public int InterlockSchedule { get; set; }

        [JsonProperty("group")]
        public int InterlockGroup { get; set; }
    }

    public class AlarmGroup
    {
        [JsonProperty("en_tz")]
        public int AlarmSchedule { get; set; }

        [JsonProperty("pdid")]
        public int AlarmPDID { get; set; }

        [JsonProperty("bind_to_zone")]
        public int? AlarmZoneId { get; set; }
    }
}
