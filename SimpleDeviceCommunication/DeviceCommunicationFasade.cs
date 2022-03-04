using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using System;

namespace SimpleDeviceCommunication
{
    public class DeviceCommunicationFasade
    {
        // Reference to the device connection implementation object
        private readonly IDeviceConnection _deviceConnection = null;
        // Device info response delegate
        public delegate void GetDeviceInfoDelegate(string deviceName, string product, string ip);
        // Device info response event
        public event GetDeviceInfoDelegate deviceInfoEvent;
        // New lighting state response delegate
        public delegate void GetNewLightingStateDelegate(string colorMode, EffectType effect, string currentColor);
        // New lighting state response event
        public event GetNewLightingStateDelegate newLightingStateEvent;


        public DeviceCommunicationFasade(IDeviceConnection deviceConnection) 
        {
            if (deviceConnection is null)
                throw new ArgumentNullException(nameof(deviceConnection));
            _deviceConnection = deviceConnection;
            _deviceConnection.AttachGetInfoResponse(DeviceInfoResponseHandler);
            _deviceConnection.AttachLightingStateChangedResponse(StateOfLightingResponseHandler);
        }

        private void DeviceInfoResponseHandler(InfoResponse response)
        {
            deviceInfoEvent?.Invoke(response.deviceName, response.product, response.ip);
        }

        private void StateOfLightingResponseHandler(StateOfLightingChangedResponse response)
        {
            newLightingStateEvent?.Invoke(response.colorMode.ToString(), response.effectID, response.currentColor);
        }

        public void SetColor(string color, uint colorFade)
        {
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                effectID = EffectType.None,
                desiredColor = color,
                durationsMs = new EffectInfo()
                {
                    colorFade = colorFade,
                    effectFade = 0,
                    effectStep = 0,
                },
            };
            _deviceConnection.SendSetLightingStateColor(request);
        }
        public void SetEffect(EffectType effectID, uint effectFade, uint effectStep)
        {
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                effectID = effectID,
                desiredColor = "--",
                durationsMs = new EffectInfo()
                {
                    colorFade = 0,
                    effectFade = effectFade,
                    effectStep = effectStep,
                },
            };
            _deviceConnection.SendSetLightingStateColor(request);
        }
        public void GetDeviceInfo()
        {
            _deviceConnection.SendGetInfo();
        }
    }
}
