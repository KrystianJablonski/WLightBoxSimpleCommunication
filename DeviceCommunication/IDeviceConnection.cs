using DeviceCommunication.CommunicationClasses;
using System;
using System.Threading.Tasks;

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
        public Task<DeviceInfoResponse> GetInfoAsync();
        /// <summary>
        /// Send get info about current lighting status to the device
        /// </summary>
        public Task<StateOfLightingChangedResponse> GetLightingStatus();
        /// <summary>
        /// Send set lighting state request to the device
        /// </summary>
        /// <param name="request">Request with new lighting state parameters</param>
        public Task<StateOfLightingChangedResponse> SetLightingState(SetStateOfLightingRequest request);
    }
}
