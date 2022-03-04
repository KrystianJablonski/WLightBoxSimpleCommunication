
namespace DeviceCommunication.CommunicationClasses
{
    /// <summary>
    /// Setting state of lighting request
    /// </summary>
    public class SetStateOfLightingRequest
    {
        public EffectType effectID;
        public string desiredColor;
        public EffectInfo durationsMs;
    }
}
