using DeviceCommunication.CommunicationClasses;
using DeviceCommunication.HttpConnection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests
{
    /// <summary>
    /// Mock IGetPostClient implementation for testing
    /// </summary>
    internal class TestGetPostClient : IGetPostClient
    {
        // Simulating response mode
        public enum ResponseMode
        {
            CorrectResponse,
            Error,
            NoResponse
        }

        // Current response mode
        public ResponseMode Mode { get; set; } = ResponseMode.CorrectResponse;

        // Mock get async request handler 
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            string localPath = new Uri(requestUri).LocalPath;
            await Task.Delay(2);
            if (Mode != ResponseMode.CorrectResponse)
            {
                return Mode switch
                {
                    ResponseMode.Error => new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound
                    },
                    _ or ResponseMode.NoResponse => throw new HttpRequestException("Test no response exception."),
                };
            }

            // respond to request if should return correct response
            return localPath switch
            {
                "/info" => new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new DeviceInfoResponse()
                        {
                            device = new DeviceInfoResponseContent()
                            {
                                deviceName = "Test device"
                            }
                        }))
                },
                "/api/rgbw/state" => new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new StateOfLightingChangedResponse()
                        {
                            rgbw = new StateOfLightingChangedResponseContent()
                            {
                                currentColor = "555555ffff"
                            }
                        }))
                },
                _ => null
            };
        }

        // Mock post async request handler 
        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            string localPath = new Uri(requestUri).LocalPath;
            await Task.Delay(2);

            if (Mode == ResponseMode.Error)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
            else if (Mode == ResponseMode.NoResponse)
            {
                throw new HttpRequestException("Test no response exception.");
            }
            else if (localPath == "/api/rgbw/state")
            {
                string postContent = await content.ReadAsStringAsync();
                SetStateOfLightingRequest requestObject = JsonConvert.DeserializeObject<SetStateOfLightingRequest>(postContent);

                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new StateOfLightingChangedResponse()
                        {
                            rgbw = new StateOfLightingChangedResponseContent()
                            {
                                desiredColor = requestObject.rgbw.desiredColor,
                                currentColor = requestObject.rgbw.desiredColor,
                                effectID = requestObject.rgbw.effectID,
                                durationsMs = requestObject.rgbw.durationsMs,
                            }
                        }))
                };
            }
            throw new InvalidOperationException("Post requested unimplemented uri: " + localPath);
        }
    }
}
