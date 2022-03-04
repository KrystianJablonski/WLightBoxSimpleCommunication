using DeviceCommunication.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public void AttachLightingStateChangedResponse(Action<StateOfLightingChangedResponse> action)
        {
            throw new NotImplementedException();
        }

        public bool Connect(string deviceLocation)
        {
            if (!Uri.IsWellFormedUriString(deviceLocation, UriKind.RelativeOrAbsolute))
                return false;
            DeviceUrl = new Uri(deviceLocation);

            return true;
        }

        public void SendGetInfo()
        {
            throw new NotImplementedException();
        }

        public void SendSetLightingStateColor(SetStateOfLightingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
