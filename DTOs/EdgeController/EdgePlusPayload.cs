using Newtonsoft.Json;

namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgePayload<T>
    {
        [JsonProperty("payload")]
        public required T Payload { get; set; }
    }
    public class EdgeArrayPayload<T>
    {
        [JsonProperty("payload")]
        public required List<T> Payload { get; set; }
    }

    #region API Login / Set Credential
    public class EdgeCredentialRequestDTO
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? IpAddress { get; set; }
    }
    public class CredentialPayload
    {
        [JsonProperty("username")]
        public required string Username { get; set; }

        [JsonProperty("password")]
        public required string Password { get; set; }
    }
    #endregion 

    #region Set Network 
    public class SetNetworkPayload
    {
        [JsonProperty("dhcp_en")]
        public bool DhcpEnabled { get; set; }

        [JsonProperty("addr")]
        public string? Ipv4 { get; set; }

        [JsonProperty("mask")]
        public string? Mask { get; set; }

        [JsonProperty("gateway")]
        public string? Gateway { get; set; }

        //[JsonProperty("hcb2")]
        //public List<string>? Hcb2 { get; set; }

        [JsonProperty("dns")]
        public List<string>? Dns { get; set; }

        //[JsonProperty("hcb2hostname")]
        //public string? Hcb2hostname { get; set; }

        [JsonProperty("up")]
        public List<Up>? Up { get; set; }
    }

    public class Up
    {
        [JsonProperty("protocol")]
        public string? Protocol { get; set; }

        [JsonProperty("addr")]
        public string? Addr { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }
    #endregion

    #region Insert CARD
    public class InsertCardPayload
    {
        [JsonProperty("cardno")]
        public required string CardNumber { get; set; }

        [JsonProperty("base")]
        public int Base { get; set; } = 16;

        [JsonProperty("info")]
        public required CardInfo CardInfo { get; set; }
    }

    public class CardInfo
    {
        [JsonProperty("type")]
        public required int CardType { get; set; }

        [JsonProperty("start")]
        public required DateOnly Start { get; set; }

        [JsonProperty("end")]
        public required DateOnly End { get; set; }

        [JsonProperty("pin")]
        public required List<string> Pin { get; set; }

        [JsonProperty("access_grp_id")]
        public required List<int> DoorAccessGroupId { get; set; }
    }
    #endregion

    #region RTC
    public class EdgeRTCPayload
    {
        [JsonProperty("weekday")]
        public int DayofTheWeek { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("date")]
        public int Day { get; set; }

        [JsonProperty("hour")]
        public int Hour { get; set; }

        [JsonProperty("minute")]
        public int Minute { get; set; }

        [JsonProperty("seconds")]
        public int Second { get; set; }

        [JsonProperty("timezone")]
        public string? Timezone { get; set; }
    }
    #endregion
}
