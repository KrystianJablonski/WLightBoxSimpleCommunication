using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests.HttpDeviceCommunication
{
    [TestClass]
    public class HandlingGetIncorrectResponsesTest
    {
        [TestMethod]
        public void HttpDeviceConnection_GetTimeOutTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));

            testClient.Mode = TestGetPostClient.ResponseMode.NoResponse;
            Task<DeviceInfoResponse> task = httpDeviceConnection.GetInfoAsync();
            task.Wait();
            Assert.IsNotNull(task.Result);
            Assert.IsNull(task.Result.statusCode);
        }

        [TestMethod]
        public void HttpDeviceConnection_GetExternalErrorTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));

            testClient.Mode = TestGetPostClient.ResponseMode.Error;
            Task<DeviceInfoResponse> task = httpDeviceConnection.GetInfoAsync();
            task.Wait();
            Assert.IsNotNull(task.Result);
            Assert.IsNotNull(task.Result.statusCode);
            Assert.AreNotEqual(System.Net.HttpStatusCode.OK, task.Result.statusCode);
        }
    }
}
