using EdgeCtrlClient.DTOs;
using EdgeCtrlClient.DTOs.EdgeController;
using EdgeCtrlClient.Helpers;
using EdgeCtrlClient.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace EdgeCtrlClient
{
    public sealed class EdgePlusClient : IEdgePlusClient
    {
        private const int ColdResetWaitTimeInSecs = 10;
        private const int ResetWaitTimeInSecs = 5;

        readonly IHttpClientFactoryService _httpClientFactoryService = new HttpClientFactoryService();
        private readonly ILogger _logger;

        public EdgePlusClient()
        {
            _logger = LoggerHelper.GetDefaultLogger();
        }

        public async Task<EdgePlusApiResponse<string>> EdgeLoginAsync(string host, string username, string password)
        {
            EdgePlusApiResponse<string> response;

            if (string.IsNullOrEmpty(username))
            {
                _logger.Error("[Login] An error occurred while login. Username cannot be null or empty.");
                return GetExceptionMessage<string>("An error occurred while login. Username cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(password))
            {
                _logger.Error("[Login] An error occurred while login. Password cannot be null or empty.");
                return GetExceptionMessage<string>("An error occurred while login. Password cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(host))
            {
                _logger.Error("[Login] An error occurred while login. Host cannot be null or empty.");
                return GetExceptionMessage<string>("An error occurred while login. Host cannot be null or empty.");
            }

            EdgePayload<CredentialPayload> payload = new()
            {
                Payload = new CredentialPayload
                {
                    Username = username,
                    Password = password
                }
            };

            try
            {
                string url = string.Format(EdgeFirmware.LoginEndpoint, host);

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[Login] An error occurred while login.");
                response = GetExceptionMessage<string>("An error occurred while login.");
            }

            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeLogoutAsync(string host)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = string.Format(EdgeFirmware.LogoutEndPoint, host);

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[Logout] An error occurred while logout.");
                response = GetExceptionMessage<string>("An error occurred while logout.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<FirmwareHardwareInfoDTO>> EdgeGetHardwareInfoAsync(string host)
        {
            EdgePlusApiResponse<FirmwareHardwareInfoDTO> response;

            try
            {
                string url = BuildUrl(host, EdgeFirmware.HardwareInfoEndpoint);

                response = await _httpClientFactoryService.SendRequestAsync<FirmwareHardwareInfoDTO>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetHardwareInfo] An error occurred while get hardware info.");
                response = GetExceptionMessage<FirmwareHardwareInfoDTO>("An error occurred while get hardware info.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<EdgeOTADTO>> EdgeOverTheAirUpdateAsync(string host, byte[] firmwareData)
        {
            EdgePlusApiResponse<EdgeOTADTO> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.UploadEndpoint);

                response = firmwareData is not null && firmwareData?.Length > 0
                    ? await _httpClientFactoryService.SendRequestAsync<EdgeOTADTO>(HttpMethod.Post, url, firmwareData)
                    : GetExceptionMessage<EdgeOTADTO>("An error occurred while updating firmware.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[OTA Update] An error occurred while updating firmware.");
                response = GetExceptionMessage<EdgeOTADTO>("An error occurred while updating firmware.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> ResetAsync(string host)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.ResetEndpoint);

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url);

                if (response.Payload?.ErrCode == 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(ResetWaitTimeInSecs));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[Reset] An error occurred while reset.");
                response = GetExceptionMessage<string>("An error occurred while reset.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> ColdResetAsync(string host)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.ColdResetEndpoint);

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url);

                if (response.Payload?.ErrCode == 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(ColdResetWaitTimeInSecs));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[ColdReset] An error occurred while cold reset.");
                response = GetExceptionMessage<string>("An error occurred while cold reset.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> FactoryDefaultAsync(string host)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.FacDefEndpoint);

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[FactoryReset] An error occurred while factory reset.");
                response = GetExceptionMessage<string>("An error occurred while factory reset.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetRTCAsync(string host)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SetRTCEndpoint);

                var now = DateTime.Now;
                var offset = TimeZoneInfo.Local.GetUtcOffset(now);
                var timezoneString = $"UTC{(offset.Hours >= 0 ? "+" : "")}{offset.Hours}";

                EdgeRTCPayload? setRTCPayload = new()
                {
                    DayofTheWeek = (int)now.DayOfWeek,
                    Day = now.Day,
                    Month = now.Month,
                    Year = now.Year,
                    Hour = now.Hour,
                    Minute = now.Minute,
                    Second = now.Second,
                    Timezone = timezoneString
                };

                EdgePayload<EdgeRTCPayload> payload = new()
                {
                    Payload = setRTCPayload
                };

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetRTC] An error occurred while set RTC.");
                response = GetExceptionMessage<string>("An error occurred while set RTC.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> SetNetworkAsync(string host, SetNetworkPayload setNetworkPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                if (setNetworkPayload is not null)
                {
                    string url = BuildUrl(host, EdgeFirmware.NetworkEndpoint);

                    EdgePayload<SetNetworkPayload> payload = new()
                    {
                        Payload = setNetworkPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);

                    //if (response.Payload?.ErrCode == 1)
                    //{
                    //    url = BuildUrl(host, EdgeFirmware.ResetEndpoint);
                    //    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url);
                    //}
                }
                else
                {
                    _logger.Error("[SetNetwork] An error occurred while set network. setNetworkPayload is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while set network. setNetworkPayload is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetNetwork] An error occurred while set network.");
                response = GetExceptionMessage<string>("An error occurred while set network.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<GetFirmwareNetworkResponseDTO>> GetNetworkAsync(string host)
        {
            EdgePlusApiResponse<GetFirmwareNetworkResponseDTO> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.NetworkEndpoint);

                response = await _httpClientFactoryService.SendRequestAsync<GetFirmwareNetworkResponseDTO>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetNetwork] An error occurred while get network.");
                response = GetExceptionMessage<GetFirmwareNetworkResponseDTO>("An error occurred while set serial.");
            }

            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetSerialAsync(string host, List<EdgeSetSerialPayload> setSerialPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SerialEndpoint);

                if (setSerialPayload is not null && setSerialPayload.Count > 0)
                {
                    EdgeArrayPayload<EdgeSetSerialPayload> payload = new()
                    {
                        Payload = setSerialPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    _logger.Error("[SetSerial] An error occurred while set serial. setSerialPayload is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while set serial. setSerialPayload is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetSerial] An error occurred while set serial.");
                response = GetExceptionMessage<string>("An error occurred while set serial.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<List<GetFirmwareSerialDTO>>> EdgeGetSerialAsync(string host)
        {
            EdgePlusApiResponse<List<GetFirmwareSerialDTO>> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SerialEndpoint);

                response = await _httpClientFactoryService.SendRequestAsync<List<GetFirmwareSerialDTO>>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetSerial] An error occurred while get serial.");
                response = GetExceptionMessage<List<GetFirmwareSerialDTO>>("An error occurred while get serial.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetScheduleAsync(string host, int scheduleId, SingleFirmwareScheduleDTO setSchedulePayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                if (setSchedulePayload is not null)
                {
                    string url = BuildUrl(host, EdgeFirmware.SetScheduleEndpoint, scheduleId);

                    EdgePayload<SingleFirmwareScheduleDTO> payload = new()
                    {
                        Payload = setSchedulePayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    _logger.Error("[SetSerial] An error occurred while set schedule. setSchedulePayload is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while set schedule. setSchedulePayload is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetSchedule] An error occurred while set schedule.");
                response = GetExceptionMessage<string>("An error occurred while set schedule.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<SingleFirmwareScheduleDTO>> EdgeGetScheduleAsync(string host, int id)
        {
            EdgePlusApiResponse<SingleFirmwareScheduleDTO> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.GetScheduleEndpoint, id);

                response = await _httpClientFactoryService.SendRequestAsync<SingleFirmwareScheduleDTO>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetSchedule] An error occurred while get schedule.");
                response = GetExceptionMessage<SingleFirmwareScheduleDTO>("An error occurred while get schedule.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetDoorAccessGroupAsync(string host, int id, SingleFirmwareDoorAccessGroupDTO setDoorAGPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                if (setDoorAGPayload is not null)
                {
                    string url = BuildUrl(host, EdgeFirmware.SetDoorAccessGroupEndpoint, id);

                    EdgePayload<SingleFirmwareDoorAccessGroupDTO> payload = new()
                    {
                        Payload = setDoorAGPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    _logger.Error("[SetDoorAccessGroup] An error occurred while set door access group. setDoorAGPayload is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while set door access group. setDoorAGPayload is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetDoorAccessGroup] An error occurred while set door access group.");
                response = GetExceptionMessage<string>("An error occurred while set door access group.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<SingleFirmwareDoorAccessGroupDTO>> EdgeGetDoorAccessGroupAsync(string host, int id)
        {
            EdgePlusApiResponse<SingleFirmwareDoorAccessGroupDTO> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.GetDoorAccessGroupEndpoint, id);

                response = await _httpClientFactoryService.SendRequestAsync<SingleFirmwareDoorAccessGroupDTO>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetDoorAccessGroup] An error occurred while get door access group.");
                response = GetExceptionMessage<SingleFirmwareDoorAccessGroupDTO>("An error occurred while get door access group.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetCardAsync(string host, EdgeSetCardPayload setCardPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                if (setCardPayload is not null)
                {
                    string url = BuildUrl(host, EdgeFirmware.SetCardEndpoint);

                    EdgePayload<EdgeSetCardPayload> payload = new()
                    {
                        Payload = setCardPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    _logger.Error("[SetCard] An error occurred while set card. setCardPayload is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while set card. setCardPayload is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetCard] An error occurred while set card.");
                response = GetExceptionMessage<string>("An error occurred while set card.");
            }

            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeUpdateCardAsync(string host, EdgeSetCardPayload setCardPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                if (setCardPayload is not null)
                {
                    string url = BuildUrl(host, EdgeFirmware.SetCardEndpoint);

                    EdgePayload<EdgeSetCardPayload> payload = new()
                    {
                        Payload = setCardPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    _logger.Error("[UpdateCard] An error occurred while update card. setCardPayload is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while update card. setCardPayload is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[UpdateCard] An error occurred while update card.");
                response = GetExceptionMessage<string>("An error occurred while update card.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeDeleteCardAsync(string host, EdgeCardInfo cardInfo)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                if (cardInfo is not null)
                {
                    string url = BuildUrl(host, EdgeFirmware.SetCardEndpoint);

                    EdgePayload<EdgeCardInfo> payload = new()
                    {
                        Payload = cardInfo
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Delete, url, payload);
                }
                else
                {
                    _logger.Error("[DeleteCard] An error occurred while delete card. cardInfo is required but was not provided.");
                    response = GetExceptionMessage<string>("An error occurred while delete card. cardInfo is required but was not provided.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[DeleteCard] An error occurred while delete card.");
                response = GetExceptionMessage<string>("An error occurred while delete card.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<SingleFirmwareCardDTO>> EdgeGetCardAsync(string host, string cardNumber, int cardNumberBase)
        {
            EdgePlusApiResponse<SingleFirmwareCardDTO> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.GetCardEndpoint, cardNumber, cardNumberBase);
                response = await _httpClientFactoryService.SendRequestAsync<SingleFirmwareCardDTO>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetCard] An error occurred while get card.");
                response = GetExceptionMessage<SingleFirmwareCardDTO>("An error occurred while get card.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<CardCountInfoDTO>> EdgeCardCountAsync(string host)
        {
            EdgePlusApiResponse<CardCountInfoDTO> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.CardCount);

                response = await _httpClientFactoryService.SendRequestAsync<CardCountInfoDTO>(HttpMethod.Get, url);
               
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[CardCount] An error occurred while get Card Count.");
                response = GetExceptionMessage<CardCountInfoDTO>("An error occurred while get Card Count.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetWiegandAsync(string host, int id, EdgeWiegandPayload setWiegandPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SetWiegandEndpoint, id);

                if (setWiegandPayload is not null)
                {
                    EdgePayload<EdgeWiegandPayload> payload = new()
                    {
                        Payload = setWiegandPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    response = GetExceptionMessage<string>("An error occurred while set IO. Invalid payload.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetWiegand] An error occurred while set Wiegand.");
                response = GetExceptionMessage<string>("An error occurred while set Wiegand.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<EdgeWiegandPayload>> EdgeGetWiegandAsync(string host, int id)
        {
            EdgePlusApiResponse<EdgeWiegandPayload> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.GetWiegandEndpoint, id);

                response = await _httpClientFactoryService.SendRequestAsync<EdgeWiegandPayload>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetWiegand] An error occurred while get wiegand.");
                response = GetExceptionMessage<EdgeWiegandPayload>("An error occurred while get wiegand.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeDeleteWiegandAsync(string host, int id)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.GetWiegandEndpoint, id);

                response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Delete, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[DeleteWiegand] An error occurred while delete wiegand.");
                response = GetExceptionMessage<string>("An error occurred while delete wiegand.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetIOAsync(string host, List<EdgeSetIOPayload> setIOPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SetIOEndpoint);

                if (setIOPayload is not null && setIOPayload.Count > 0)
                {
                    EdgeArrayPayload<EdgeSetIOPayload> payload = new()
                    {
                        Payload = setIOPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    response = GetExceptionMessage<string>("An error occurred while set IO. Invalid payload.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetIO] An error occurred while set IO.");
                response = GetExceptionMessage<string>("An error occurred while set IO.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<List<SingleFirmwareIO_DTO>>> EdgeGetIOAsync(string host, int pdid)
        {
            EdgePlusApiResponse<List<SingleFirmwareIO_DTO>> response;

            try
            {
                string url = BuildUrl(host, EdgeFirmware.GetIOEndpoint, pdid);

                response = await _httpClientFactoryService.SendRequestAsync<List<SingleFirmwareIO_DTO>>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[GetIO] An error occurred while get IO.");
                response = GetExceptionMessage<List<SingleFirmwareIO_DTO>>("An error occurred while set IO.");
            }

            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetDoorAsync(string host, List<EdgeSetDoorPayload>? setDoorPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SetDoorEndpoint);

                if (setDoorPayload is not null && setDoorPayload.Count > 0)
                {
                    EdgeArrayPayload<EdgeSetDoorPayload> payload = new()
                    {
                        Payload = setDoorPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    response = GetExceptionMessage<string>("An error occurred while set door. Invalid payload.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetDoor] An error occurred while set door.");
                response = GetExceptionMessage<string>("An error occurred while set door.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<List<SingleFirmwareDoorDTO>>> EdgeGetDoorAsync(string host, int pdid)
        {
            List<SingleFirmwareDoorDTO> door = new();
            EdgePlusApiResponse<List<SingleFirmwareDoorDTO>> response = new();
            StringBuilder sb = new();
            string errMessage;

            int max_drid = pdid == 255 ? 4 : 2; // Maximum door onboard is 4, W2O are 2

            for (int drid = 0; drid < max_drid; drid++)
            {
                string url = BuildUrl(host, EdgeFirmware.GetDoorEndpoint, pdid, drid);

                var apiResponse = await _httpClientFactoryService.SendRequestAsync<SingleFirmwareDoorDTO>(HttpMethod.Get, url);

                if (apiResponse.Payload?.ErrCode == 1)
                {
                    if (apiResponse.Info is not null)
                    {
                        door.Add(apiResponse.Info);
                    }
                }
                else
                {
                    errMessage = string.Format("drid {0}: {1}", drid, response.Payload?.Info);
                    sb.AppendLine(errMessage);
                    _logger.Error("[GetDoor] An error occurred while get door. {errMsg}", errMessage);
                }
            }

            if (door.Count > 0)
            {
                response = new EdgePlusApiResponse<List<SingleFirmwareDoorDTO>>()
                {
                    Payload = new Payload()
                    {
                        ErrCode = 1,
                        Info = sb.ToString() == "" ? null : sb.ToString()
                    },
                    Info = door
                };
            }

            return response;
        }

        public async Task<EdgePlusApiResponse<string>> EdgeSetReaderAsync(string host, List<EdgeSetReaderPayload> setReaderPayload)
        {
            EdgePlusApiResponse<string> response;
            try
            {
                string url = BuildUrl(host, EdgeFirmware.SetReaderEndpoint);

                if (setReaderPayload is not null && setReaderPayload.Count > 0)
                {
                    EdgeArrayPayload<EdgeSetReaderPayload> payload = new()
                    {
                        Payload = setReaderPayload
                    };

                    response = await _httpClientFactoryService.SendRequestAsync<string>(HttpMethod.Post, url, payload);
                }
                else
                {
                    response = GetExceptionMessage<string>("An error occurred while set reader: Invalid payload.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[SetReader] An error occurred while set reader.");
                response = GetExceptionMessage<string>("An error occurred while set reader.");
            }
            return response;
        }

        public async Task<EdgePlusApiResponse<List<SingleFirmwareReaderDTO>>?> EdgeGetReaderAsync(string host, int pdid)
        {
            EdgePlusApiResponse<List<SingleFirmwareReaderDTO>> response;

            string url = BuildUrl(host, EdgeFirmware.GetReaderEndpoint, pdid);

            response = await _httpClientFactoryService.SendRequestAsync<List<SingleFirmwareReaderDTO>>(HttpMethod.Get, url);

            return response;
        }

        private static string BuildBaseUrl(string host)
        {
            return string.Format(EdgeFirmware.BaseAddress, host);
        }

        private static string BuildUrl(string host, string relativeUrl, params object[] args)
        {
            return $"{BuildBaseUrl(host)}{string.Format(relativeUrl, args)}";
        }

        private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.OK;
        }

        private T? DeserializePayload<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while deserialize payload.");
                return default;
            }
        }

        private T? DeserializeJToken<T>(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return default;

            return token.ToObject<T>();
        }

        private static EdgePlusApiResponse<T> GetExceptionMessage<T>(string message)
        {
            return new EdgePlusApiResponse<T>()
            {
                Payload = new Payload()
                {
                    ErrCode = 0,
                    Info = message
                },
                Info = default
            };
        }
    }
}
