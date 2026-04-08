namespace EdgeCtrlClient.DTOs.EdgeController
{
    public class EdgeNetworkResponse
    {
        public PayloadNetwork? Payload { get; set; }

        public GetFirmwareNetworkResponseDTO? Info { get; set; }
    }

    public class PayloadNetwork
    {
        public int ErrCode { get; set; }
    }

    public class GetFirmwareNetworkResponseDTO
    {
        public string? Mac { get; set; }

        public string? Ipv4 { get; set; }

        public string? Mask { get; set; }

        public string? Gateway { get; set; }

        public List<string>? Dns { get; set; }

        public List<Up>? Up { get; set; }

        public bool? Dhcp_en { get; set; }
    }
}
