using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using System;

namespace SimpleDeviceCommunication
{
    public class DeviceCommunicationFasade
    {
        // Reference to the IDeviceConnection implementation object
        private readonly IDeviceConnection _deviceConnection = null;
        // Device info response delegate
        public delegate void GetDeviceInfoDelegate(string deviceName, string product, string ip);
        // Device info response event
        public event GetDeviceInfoDelegate deviceInfoEvent;
        // New lighting state response delegate
        public delegate void GetNewLightingStateDelegate(string colorMode, EffectType effect, string currentColor);
        // New lighting state response event
        public event GetNewLightingStateDelegate newLightingStateEvent;


        /// <summary>
        /// DeviceCommunicationFasade constructor.
        /// Binds <paramref name="deviceConnection"/> to readonly <seealso cref="_deviceConnection"/> and will operate only on this device.
        /// </summary>
        /// <param name="deviceConnection">Device connection to bind to this object</param>
        public DeviceCommunicationFasade(IDeviceConnection deviceConnection)
        {
            if (deviceConnection is null)
                throw new ArgumentNullException(nameof(deviceConnection));
            _deviceConnection = deviceConnection;
            _deviceConnection.AttachGetInfoResponse(DeviceInfoResponseHandler);
            _deviceConnection.AttachLightingStateChangedResponse(StateOfLightingResponseHandler);
        }

        /// <summary>
        /// On device info response event handler.
        /// Invokes <seealso cref="deviceInfoEvent"/>.
        /// </summary>
        private void DeviceInfoResponseHandler(InfoResponse response)
        {
            deviceInfoEvent?.Invoke(response.deviceName, response.product, response.ip);
        }

        /// <summary>
        /// On state of lighting response event handler.
        /// Invokes <seealso cref="newLightingStateEvent"/>.
        /// </summary>
        private void StateOfLightingResponseHandler(StateOfLightingChangedResponse response)
        {
            newLightingStateEvent?.Invoke(response.colorMode.ToString(), response.effectID, response.currentColor);
        }

        /// <summary>
        /// Send set color request to the device with given parameters.
        /// </summary>
        /// <param name="color">New color</param>
        /// <param name="colorFade">Duration of color transition</param>
        public void SetColor(string color, uint colorFade)
        {
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                effectID = EffectType.None,
                desiredColor = color,
                durationsMs = new DurationMs()
                {
                    colorFade = colorFade,
                    effectFade = 0,
                    effectStep = 0,
                },
            };
            _deviceConnection.SendSetLightingStateColor(request);
        }

        /// <summary>
        /// Send set effect request to the device with given parameters.
        /// </summary>
        /// <param name="effectID">New effect</param>
        /// <param name="effectFade">Duration of effect transition</param>
        /// <param name="effectStep">Time of effect step</param>
        public void SetEffect(EffectType effectID, uint effectFade, uint effectStep)
        {
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                effectID = effectID,
                desiredColor = "--",
                durationsMs = new DurationMs()
                {
                    colorFade = 0,
                    effectFade = effectFade,
                    effectStep = effectStep,
                },
            };
            _deviceConnection.SendSetLightingStateColor(request);
        }

        /// <summary>
        /// Send get info request to the device.
        /// </summary>
        public void GetDeviceInfo()
        {
            _deviceConnection.SendGetInfo();
        }

        /// <summary>
        /// Send get lighting status request to the device.
        /// </summary>
        public void GetLightingStatus()
        {
            _deviceConnection.SendGetLightingStatus();
        }
    }
}
