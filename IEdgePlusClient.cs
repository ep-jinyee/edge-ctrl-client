using EdgeCtrlClient.DTOs.EdgeController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient
{
    public interface IEdgePlusClient
    {
        Task<EdgePlusApiResponse<string>> EdgeLoginAsync(string host, string username, string password);

        Task<EdgePlusApiResponse<string>> EdgeLogoutAsync(string host);

        Task<EdgePlusApiResponse<FirmwareHardwareInfoDTO>> EdgeGetHardwareInfoAsync(string host);

        Task<EdgePlusApiResponse<EdgeOTADTO>> EdgeOverTheAirUpdateAsync(string host, byte[] firmwareData);

        Task<EdgePlusApiResponse<string>> ResetAsync(string host);

        Task<EdgePlusApiResponse<string>> ColdResetAsync(string host);

        Task<EdgePlusApiResponse<string>> FactoryDefaultAsync(string host);

        Task<EdgePlusApiResponse<string>> EdgeSetRTCAsync(string host);

        Task<EdgePlusApiResponse<string>> SetNetworkAsync(string host, SetNetworkPayload setNetworkPayload);

        Task<EdgePlusApiResponse<GetFirmwareNetworkResponseDTO>> GetNetworkAsync(string host);

        Task<EdgePlusApiResponse<string>> EdgeSetSerialAsync(string host, List<EdgeSetSerialPayload> setSerialPayload);

        Task<EdgePlusApiResponse<List<GetFirmwareSerialDTO>>> EdgeGetSerialAsync(string host);

        Task<EdgePlusApiResponse<string>> EdgeSetScheduleAsync(string host, int scheduleId, SingleFirmwareScheduleDTO setSchedulePayload);

        Task<EdgePlusApiResponse<SingleFirmwareScheduleDTO>> EdgeGetScheduleAsync(string host, int id);

        Task<EdgePlusApiResponse<string>> EdgeSetDoorAccessGroupAsync(string host, int id, SingleFirmwareDoorAccessGroupDTO setDoorAGPayload);

        Task<EdgePlusApiResponse<SingleFirmwareDoorAccessGroupDTO>> EdgeGetDoorAccessGroupAsync(string host, int id);

        Task<EdgePlusApiResponse<string>> EdgeSetCardAsync(string host, EdgeSetCardPayload setCardPayload);

        Task<EdgePlusApiResponse<string>> EdgeUpdateCardAsync(string host, EdgeSetCardPayload setCardPayload);

        Task<EdgePlusApiResponse<string>> EdgeDeleteCardAsync(string host, EdgeCardInfo cardInfo);

        Task<EdgePlusApiResponse<SingleFirmwareCardDTO>> EdgeGetCardAsync(string host, string cardNumber, int cardNumberBase);

        Task<EdgePlusApiResponse<CardCountInfoDTO>> EdgeCardCountAsync(string host);

        Task<EdgePlusApiResponse<string>> EdgeSetWiegandAsync(string host, int id, EdgeWiegandPayload setWiegandPayload);

        Task<EdgePlusApiResponse<EdgeWiegandPayload>> EdgeGetWiegandAsync(string host, int id);

        Task<EdgePlusApiResponse<string>> EdgeDeleteWiegandAsync(string host, int id);

        Task<EdgePlusApiResponse<string>> EdgeSetIOAsync(string host, List<EdgeSetIOPayload> setIOPayload);

        Task<EdgePlusApiResponse<List<SingleFirmwareIO_DTO>>> EdgeGetIOAsync(string host, int pdid);

        Task<EdgePlusApiResponse<string>> EdgeSetDoorAsync(string host, List<EdgeSetDoorPayload>? setDoorPayload);

        Task<EdgePlusApiResponse<List<SingleFirmwareDoorDTO>>> EdgeGetDoorAsync(string host, int pdid);

        Task<EdgePlusApiResponse<string>> EdgeSetReaderAsync(string host, List<EdgeSetReaderPayload> setReaderPayload);

        Task<EdgePlusApiResponse<List<SingleFirmwareReaderDTO>>?> EdgeGetReaderAsync(string host, int pdid);
    }
}
