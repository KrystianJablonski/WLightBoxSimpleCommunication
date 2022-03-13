using DeviceCommunication.CommunicationClasses;
using System.Threading.Tasks;

namespace DeviceCommunication
{
    /// <summary>
    /// Dummy connection class with <seealso cref="IDeviceConnection"/> interface implementation.
    /// Simulates device with hard coded values.
    /// </summary>
    public class DummyConnection : IDeviceConnection
    {
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
        public async Task<DeviceInfoResponse> GetInfoAsync()
        {
            await Task.Delay(2000);
            return new DeviceInfoResponse()
            {
                statusCode = System.Net.HttpStatusCode.OK,
                device = new DeviceInfoResponseContent()
                {
                    deviceName = "My BleBox device name",
                    product = "wLightBox_v3",
                    ip = "192.168.1.11",
                }
            };
        }

        /// <summary>
        /// Simulates sending get lighting status request.
        /// Invokes <seealso cref="_stateOfLightingChangedEvent"/> after 2s with sample data.
        /// </summary>
        public async Task<StateOfLightingChangedResponse> GetLightingStatus()
        {
            await Task.Delay(1500);
            return new StateOfLightingChangedResponse()
            {
                statusCode = System.Net.HttpStatusCode.OK,
                rgbw = new StateOfLightingChangedResponseContent()
                {
                    currentColor = "ff00300000",
                    effectID = EffectType.Effect3,
                    colorMode = ColorMode.RGBorW,
                },
            };
        }

        /// <summary>
        /// Simulates sending change lighting state <paramref name="request"/>.
        /// Invokes <seealso cref="_stateOfLightingChangedEvent"/> after 2s with data from <paramref name="request"/>.
        /// </summary>
        /// <param name="request">Request with new lighting state</param>
        public async Task<StateOfLightingChangedResponse> SetLightingState(SetStateOfLightingRequest request)
        {
            await Task.Delay(1500);
            return new StateOfLightingChangedResponse()
            {
                statusCode = System.Net.HttpStatusCode.OK,
                rgbw = new StateOfLightingChangedResponseContent()
                {
                    currentColor = request.rgbw.desiredColor,
                    effectID = request.rgbw.effectID,
                    colorMode = ColorMode.RGBorW,
                },
            };
        }
    }
}
