using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient
{
    internal static class EdgeFirmware
    {
        internal static readonly string BaseAddress = "https://{0}/api/";
        internal static readonly string LoginEndpoint = "https://{0}/token";
        internal static readonly string LogoutEndPoint = "https://{0}/remove";
        internal static readonly string HardwareInfoEndpoint = "hardware";
        internal static readonly string UploadEndpoint = "ota-update";
        internal static readonly string ResetEndpoint = "sys/reset";
        internal static readonly string ColdResetEndpoint= "sys/coldstart";
        internal static readonly string FacDefEndpoint = "sys/setdef";
        internal static readonly string NetworkEndpoint = "network";
        internal static readonly string SetScheduleEndpoint = "timezone/{0}";
        internal static readonly string GetScheduleEndpoint = "timezone?id={0}";
        internal static readonly string SetDoorAccessGroupEndpoint = "access-group/{0}";
        internal static readonly string GetDoorAccessGroupEndpoint = "access-group?id={0}";
        internal static readonly string SetCardEndpoint = "card";
        internal static readonly string GetCardEndpoint = "card/{0}?base={1}";
        internal static readonly string CardCount = "card/count";
        internal static readonly string SetWiegandEndpoint = "wiegand/{0}";
        internal static readonly string GetWiegandEndpoint = "wiegand?id={0}";
        internal static readonly string SetIOEndpoint = "io";
        internal static readonly string GetIOEndpoint = "io?pd_id={0}";
        internal static readonly string SetDoorEndpoint = "door";
        internal static readonly string GetDoorEndpoint = "door?pdid={0}&drid={1}";
        internal static readonly string SetReaderEndpoint = "reader";
        internal static readonly string GetReaderEndpoint = "reader?pd_id={0}";
        internal static readonly string SerialEndpoint = "sys/serial";
        internal static readonly string SetRTCEndpoint = "rtc";
        internal static readonly string SetDoorPulseOpenEndPoint = "door/pulse-open";
        internal static readonly string SetDoorSecurityEndPoint = "door/security-lvl";
        internal static readonly string SetDoorInhibitEndPoint = "door/inhibit";
    }
}
