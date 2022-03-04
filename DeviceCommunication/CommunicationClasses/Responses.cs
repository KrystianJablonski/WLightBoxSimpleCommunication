
namespace DeviceCommunication.CommunicationClasses
{
    /// <summary>
    /// Get info response
    /// </summary>
    public class InfoResponse
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
    /// Set state of lighting response 
    /// </summary>
    public class StateOfLightingChangedResponse
    {
        public ColorMode colorMode;
        public EffectType effectID;
        public string desiredColor;
        public string currentColor;
        public string lastOnColor;
        public EffectInfo durationsMs;
    }
}
