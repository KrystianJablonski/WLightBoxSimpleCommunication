using DeviceCommunication.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeviceCommunication
{
    /// <summary>
    /// Implementation of device connection by http protocol
    /// </summary>
    public class HttpDeviceConnection : IDeviceConnection
    {
        // device connection url
        public Uri DeviceUrl { get; set; }

        public void AttachGetInfoResponse(Action<InfoResponse> action)
        {
        }

        public void AttachLightingStateChangedResponse(Action<StateOfLightingChangedResponse> action)
        {
        }

        public bool Connect(string deviceLocation)
        {
            if(!string.IsNullOrEmpty(deviceLocation) && Uri.TryCreate(deviceLocation, UriKind.Absolute, out Uri newUri))
            {
                DeviceUrl = newUri;
                return true;
            }

            return false;
        }

        public void SendGetInfo()
        {
            throw new NotImplementedException();
        }

        public void SendGetLightingStatus()
        {
            throw new NotImplementedException();
        }

        public void SendSetLightingStateColor(SetStateOfLightingRequest request)
        {
            string serializedRequest = JsonConvert.SerializeObject(request);

        }
    }
}
