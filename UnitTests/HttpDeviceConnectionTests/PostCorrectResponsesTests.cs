using DeviceCommunication;
using DeviceCommunication.CommunicationClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests.HttpDeviceCommunication
{
    [TestClass]
    public class PostCorrectResponsesTests
    {
        [TestMethod]
        public void HttpDeviceConnection_CorrectPostSetColorTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));

            testClient.Mode = TestGetPostClient.ResponseMode.CorrectResponse;
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                rgbw = new SetStateOfLightingRequestContent()
                {
                    desiredColor = "4444440000",
                    durationsMs = new DurationMs()
                    {
                        colorFade = 1200U
                    }
                }
            };
            Task<StateOfLightingChangedResponse> task = httpDeviceConnection.SetLightingState(request);
            task.Wait();
            Assert.IsNotNull(task.Result);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, task.Result.statusCode);
            Assert.AreEqual("4444440000", task.Result.rgbw.currentColor);
            Assert.AreEqual(1200U, task.Result.rgbw.durationsMs.colorFade);
        }
        [TestMethod]
        public void HttpDeviceConnection_CorrectPostPostSetEffectTest()
        {
            string devicePath = "http://domain/";
            TestGetPostClient testClient = new TestGetPostClient();
            HttpDeviceConnection httpDeviceConnection = new HttpDeviceConnection(testClient);
            Assert.IsTrue(httpDeviceConnection.Connect(devicePath));

            testClient.Mode = TestGetPostClient.ResponseMode.CorrectResponse;
            SetStateOfLightingRequest request = new SetStateOfLightingRequest()
            {
                rgbw = new SetStateOfLightingRequestContent()
                {
                    effectID = EffectType.Effect4,
                    durationsMs = new DurationMs()
                    {
                        effectFade = 1200U,
                        effectStep = 1500U
                    }
                }
            };
            Task<StateOfLightingChangedResponse> task = httpDeviceConnection.SetLightingState(request);
            task.Wait();
            Assert.IsNotNull(task.Result);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, task.Result.statusCode);
            Assert.AreEqual(EffectType.Effect4, task.Result.rgbw.effectID);
            Assert.AreEqual(1200U, task.Result.rgbw.durationsMs.effectFade);
            Assert.AreEqual(1500U, task.Result.rgbw.durationsMs.effectStep);
        }
    }
}
