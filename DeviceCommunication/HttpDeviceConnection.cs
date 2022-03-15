using DeviceCommunication.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using DeviceCommunication.HttpConnection;

namespace DeviceCommunication
{
    /// <summary>
    /// Implementation of device connection by http protocol
    /// </summary>
    public class HttpDeviceConnection : IDeviceConnection
    {
        // device connection url
        public Uri DeviceUrl { get; private set; }
        private readonly IGetPostClient _client;

        public HttpDeviceConnection(IGetPostClient httpClient = null)
        {
            _client = httpClient;
            if (_client == null)
                _client = new HttpGetPostClient();
        }

        /// <summary>
        /// Implementation of <seealso cref="IDeviceConnection.Connect(string)"/> method.
        /// Saves the <paramref name="deviceLocation"/> uri if it's correct uri address.
        /// </summary>
        /// <param name="deviceLocation">Uri of the device</param>
        /// <returns>Is correct <paramref name="deviceLocation"/> uri address</returns>
        public bool Connect(string deviceLocation)
        {
            if(!string.IsNullOrEmpty(deviceLocation) && Uri.TryCreate(deviceLocation, UriKind.Absolute, out Uri newUri))
            {
                DeviceUrl = newUri;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Implementation of <seealso cref="IDeviceConnection.GetInfoAsync"/> method.
        /// Sends the get device info request and awaits and returnes deserialized response.
        /// </summary>
        /// <returns>Device response</returns>
        public async Task<DeviceInfoResponse> GetInfoAsync()
        {
            DeviceInfoResponse infoResponse;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(DeviceUrl.AbsoluteUri + "info");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                infoResponse = JsonConvert.DeserializeObject<DeviceInfoResponse>(responseBody);
                infoResponse.statusCode = response.StatusCode;
            }
            catch (HttpRequestException e)
            {
                infoResponse = new DeviceInfoResponse()
                {
                    statusCode = e.StatusCode,
                };
            }
            return infoResponse;
        }

        /// <summary>
        /// Implementation of <seealso cref="IDeviceConnection.GetLightingStatus"/> method.
        /// Sends the get lighting status request and awaits and returnes deserialized response.
        /// </summary>
        /// <returns>Lighting status response</returns>
        public async Task<StateOfLightingChangedResponse> GetLightingStatus()
        {
            StateOfLightingChangedResponse stateResponse;
            try
            {
                string path = DeviceUrl.AbsoluteUri + "api/rgbw/state";
                HttpResponseMessage response = await _client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stateResponse = JsonConvert.DeserializeObject<StateOfLightingChangedResponse>(responseBody);
                stateResponse.statusCode = response.StatusCode;
            }
            catch (HttpRequestException e)
            {
                stateResponse = new StateOfLightingChangedResponse()
                {
                    statusCode = e.StatusCode,
                };
            }
            return stateResponse;
        }

        /// <summary>
        /// Implementation of <seealso cref="IDeviceConnection.GetLightingStatus"/> method.
        /// Sends the post set lighting status request and awaits and returnes deserialized response.
        /// </summary>
        /// <returns>New lighting status response</returns>
        public async Task<StateOfLightingChangedResponse> SetLightingState(SetStateOfLightingRequest request)
        {
            StateOfLightingChangedResponse stateResponse;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(DeviceUrl.AbsoluteUri + "api/rgbw/state", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stateResponse = JsonConvert.DeserializeObject<StateOfLightingChangedResponse>(responseBody);
                stateResponse.statusCode = response.StatusCode;
            }
            catch (HttpRequestException e)
            {
                stateResponse = new StateOfLightingChangedResponse()
                {
                    statusCode = e.StatusCode,
                };
            }
            return stateResponse;
        }
    }
}
