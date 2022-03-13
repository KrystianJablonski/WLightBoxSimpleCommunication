
using System;
using System.Net;

namespace DeviceCommunication.CommunicationClasses
{
    /// <summary>
    /// Base for response classes
    /// </summary>
    [Serializable]
    public class Response
    {
        public HttpStatusCode? statusCode;
    }

    /// <summary>
    /// Content of get info response
    /// </summary>
    [Serializable]
    public class DeviceInfoResponseContent
    {
        public string deviceName;
        public string product;
        public string type;
        public string apiLevel;
        public string hv;
        public string fv;
        public string id;
        public string ip;
    }

    /// <summary>
    /// Get info response
    /// </summary>
    [Serializable]
    public class DeviceInfoResponse : Response
    {
        public DeviceInfoResponseContent device;
    }

    /// <summary>
    /// Content of get state of lighting response 
    /// </summary>
    [Serializable]
    public class StateOfLightingChangedResponseContent
    {
        public ColorMode colorMode;
        public EffectType effectID;
        public string desiredColor;
        public string currentColor;
        public string lastOnColor;
        public DurationMs durationsMs;
    }

    /// <summary>
    /// Set state of lighting response 
    /// </summary>
    [Serializable]
    public class StateOfLightingChangedResponse : Response
    {
        public StateOfLightingChangedResponseContent rgbw;
    }
}
