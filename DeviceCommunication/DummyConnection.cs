using DeviceCommunication.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceCommunication
{
    public class DummyConnection : IDeviceConnection
    {
        private event Action<InfoResponse> _infoResponseEvent;
        private event Action<StateOfLightingChangedResponse> _stateOfLightingChangedEvent;


        public void AttachGetInfoResponse(Action<InfoResponse> action)
        {
            _infoResponseEvent += action;
        }

        public void AttachLightingStateChangedResponse(Action<StateOfLightingChangedResponse> action)
        {
            _stateOfLightingChangedEvent += action;
        }

        public bool Connect(string deviceLocation)
        {
            return true;
        }

        public void SendGetInfo()
        {
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                _infoResponseEvent?.Invoke(new InfoResponse()
                {
                    deviceName = "My BleBox device name",
                    product = "wLightBox_v3",
                    ip = "192.168.1.11",
                });
            });
        }

        public void SendGetLightingStatus()
        {
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                _stateOfLightingChangedEvent?.Invoke(new StateOfLightingChangedResponse()
                {
                    currentColor = "ff00300000",
                    effectID = EffectType.Effect3,
                    colorMode = ColorMode.RGBorW,
                });
            });
        }

        public void SendSetLightingStateColor(SetStateOfLightingRequest request)
        {
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                _stateOfLightingChangedEvent?.Invoke(new StateOfLightingChangedResponse()
                {
                    currentColor = request.desiredColor,
                    effectID = request.effectID,
                    colorMode = ColorMode.RGBorW,
                });
            });
        }
    }
}
