using DeviceCommunication.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceCommunication
{
    /// <summary>
    /// Dummy connection class with <seealso cref="IDeviceConnection"/> interface implementation.
    /// Simulates device with hard coded values.
    /// </summary>
    public class DummyConnection : IDeviceConnection
    {
        // Responses events
        private event Action<InfoResponse> _infoResponseEvent;
        private event Action<StateOfLightingChangedResponse> _stateOfLightingChangedEvent;

        /// <summary>
        /// Attach <paramref name="action"/> to <seealso cref="_infoResponseEvent"/>.
        /// </summary>
        /// <param name="action">Action to invoke when receiving device info response</param>
        public void AttachGetInfoResponse(Action<InfoResponse> action)
        {
            _infoResponseEvent += action;
        }

        /// <summary>
        /// Attach <paramref name="action"/> to <seealso cref="_stateOfLightingChangedEvent"/>.
        /// </summary>
        /// <param name="action">Action to invoke when receiving state of lighting response</param>
        public void AttachLightingStateChangedResponse(Action<StateOfLightingChangedResponse> action)
        {
            _stateOfLightingChangedEvent += action;
        }

        /// <summary>
        /// Dummy implementation of the Connect method.
        /// Always returns true.
        /// </summary>
        /// <param name="deviceLocation">This parameter doesn't metter</param>
        /// <returns>Always: true</returns>
        public bool Connect(string deviceLocation)
        {
            return true;
        }

        /// <summary>
        /// Simulates sending get info request.
        /// Invokes <seealso cref="_infoResponseEvent"/> after 2s with sample data.
        /// </summary>
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

        /// <summary>
        /// Simulates sending get lighting status request.
        /// Invokes <seealso cref="_stateOfLightingChangedEvent"/> after 2s with sample data.
        /// </summary>
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

        /// <summary>
        /// Simulates sending change lighting state <paramref name="request"/>.
        /// Invokes <seealso cref="_stateOfLightingChangedEvent"/> after 2s with data from <paramref name="request"/>.
        /// </summary>
        /// <param name="request">Request with new lighting state</param>
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
