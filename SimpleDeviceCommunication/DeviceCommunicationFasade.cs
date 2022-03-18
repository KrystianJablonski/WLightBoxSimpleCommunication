using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using System;
using System.Threading.Tasks;

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
        // Show message event
        public event Action<string> showMessage;


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
        }

        /// <summary>
        /// On device info response event handler.
        /// Invokes <seealso cref="deviceInfoEvent"/>.
        /// </summary>
        private void DeviceInfoResponseHandler(DeviceInfoResponse response)
        {
            deviceInfoEvent?.Invoke(response.device.deviceName, response.device.product, response.device.ip);
        }

        /// <summary>
        /// On state of lighting response event handler.
        /// Invokes <seealso cref="newLightingStateEvent"/>.
        /// </summary>
        private void StateOfLightingResponseHandler(StateOfLightingChangedResponse response)
        {
            newLightingStateEvent?.Invoke(response.rgbw.colorMode.ToString(), response.rgbw.effectID, response.rgbw.currentColor);
        }

        /// <summary>
        /// Send set color request to the device with given parameters.
        /// </summary>
        /// <param name="color">New color</param>
        /// <param name="colorFade">Duration of color transition</param>
        public async Task SetColor(string color, uint colorFade)
        {
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                rgbw = new SetStateOfLightingRequestContent()
                {
                    effectID = EffectType.None,
                    desiredColor = color,
                    durationsMs = new DurationMs()
                    {
                        colorFade = colorFade,
                        effectFade = 0,
                        effectStep = 0,
                    },
                }
            };
            StateOfLightingChangedResponse lightingState = await _deviceConnection.SetLightingState(request);
            if (lightingState == null || lightingState.statusCode != System.Net.HttpStatusCode.OK)
            {
                showMessage?.Invoke("Set color failed. " + (lightingState?.statusCode == null ?
                    "No response from device."
                    : "StatusCode = " + lightingState.statusCode.ToString() + "."));
            }
            else
                StateOfLightingResponseHandler(lightingState);
        }

        /// <summary>
        /// Send set effect request to the device with given parameters.
        /// </summary>
        /// <param name="effectID">New effect</param>
        /// <param name="effectFade">Duration of effect transition</param>
        /// <param name="effectStep">Time of effect step</param>
        public async Task SetEffect(EffectType effectID, uint effectFade, uint effectStep)
        {
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                rgbw = new SetStateOfLightingRequestContent()
                {
                    effectID = effectID,
                    desiredColor = "--",
                    durationsMs = new DurationMs()
                    {
                        colorFade = 0,
                        effectFade = effectFade,
                        effectStep = effectStep,
                    },
                }
            };
            StateOfLightingChangedResponse lightingState = await _deviceConnection.SetLightingState(request);
            if (lightingState == null || lightingState.statusCode != System.Net.HttpStatusCode.OK)
            {
                showMessage?.Invoke("Set effect failed. " + (lightingState?.statusCode == null ?
                    "No response from device."
                    : "StatusCode = " + lightingState.statusCode.ToString() + "."));
            }
            else
                StateOfLightingResponseHandler(lightingState);
        }

        /// <summary>
        /// Send get info request to the device.
        /// </summary>
        public async Task GetDeviceInfo()
        {
            DeviceInfoResponse deviceState = await _deviceConnection.GetInfoAsync();
            if (deviceState == null || deviceState.statusCode != System.Net.HttpStatusCode.OK)
            {
                showMessage?.Invoke("Get device info failed. " + (deviceState?.statusCode == null ?
                    "No response from device."
                    : "StatusCode = " + deviceState.statusCode.ToString() + "."));
            }
            else
                DeviceInfoResponseHandler(deviceState);
        }

        /// <summary>
        /// Send get lighting status request to the device.
        /// </summary>
        public async Task GetLightingStatusAsync()
        {
            StateOfLightingChangedResponse lightingState = await _deviceConnection.GetLightingStatus();
            if (lightingState == null || lightingState.statusCode != System.Net.HttpStatusCode.OK)
            {
                showMessage?.Invoke("Get lighting status failed. " + (lightingState?.statusCode == null ?
                    "No response from device."
                    : "StatusCode = " + lightingState.statusCode.ToString() + "."));
            }
            else
                StateOfLightingResponseHandler(lightingState);
        }
    }
}
