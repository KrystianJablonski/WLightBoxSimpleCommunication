using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests.HttpDeviceCommunication
{
    [TestClass]
    public class GetCorrectResponsesTests
    {
        [TestMethod]
        public void HttpDeviceConnection_CorrectGetInfoTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));

            testClient.Mode = TestGetPostClient.ResponseMode.CorrectResponse;
            Task<DeviceInfoResponse> task = httpDeviceConnection.GetInfoAsync();
            task.Wait();
            Assert.IsNotNull(task.Result);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, task.Result.statusCode);
            Assert.AreEqual("Test device", task.Result.device.deviceName);
        }

        [TestMethod]
        public void HttpDeviceConnection_CorrectGetLightingStatusTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));

            testClient.Mode = TestGetPostClient.ResponseMode.CorrectResponse;
            Task<StateOfLightingChangedResponse> task = httpDeviceConnection.GetLightingStatus();
            task.Wait();
            Assert.IsNotNull(task.Result);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, task.Result.statusCode);
            Assert.AreEqual("555555ffff", task.Result.rgbw.currentColor);
        }
    }
}
