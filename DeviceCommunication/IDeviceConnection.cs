using DeviceCommunication.CommunicationClasses;
using System;

namespace DeviceCommunication
{
    /// <summary>
    /// Device connection interface for simple interaction with the device
    /// </summary>
    public interface IDeviceConnection
    {
        /// <summary>
        /// Create connection with the device
        /// </summary>
        /// <param name="deviceLocation">Path to the device - IP location, file location etc.</param>
        /// <returns>Creating connection succeeded</returns>
        public bool Connect(string deviceLocation);
        /// <summary>
        /// Send get info about the device request
        /// </summary>
        public void SendGetInfo();
        /// <summary>
        /// Send set lighting state request to the device
        /// </summary>
        /// <param name="request">Request with new lighting state parameters</param>
        public void SendSetLightingStateColor(SetStateOfLightingRequest request);
        /// <summary>
        /// Attach listener to info response event
        /// </summary>
        /// <param name="action">Action to invoke when receiving device info response</param>
        public void AttachGetInfoResponse(Action<InfoResponse> action);
        /// <summary>
        /// Attach listener to lighting state changed event
        /// </summary>
        /// <param name="action">Action to invoke when receiving new lighting state response</param>
        public void AttachLightingStateChangedResponse(Action<StateOfLightingChangedResponse> action);
    }
}
