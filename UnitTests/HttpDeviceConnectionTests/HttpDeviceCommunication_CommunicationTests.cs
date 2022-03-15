using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests.HttpDeviceCommunication
{
    [TestClass]
    public class HttpDeviceCommunication_CommunicationTests
    {
        [TestMethod]
        public void HttpDeviceConnection_ConnectionTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsFalse(httpDeviceConnection.Connect("invalidAddress"));
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));
            Assert.AreEqual(devicePath, httpDeviceConnection.DeviceUrl.AbsoluteUri);
        }
    }
}
