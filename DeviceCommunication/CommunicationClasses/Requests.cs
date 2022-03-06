using System;

namespace DeviceCommunication.CommunicationClasses
{
    /// <summary>
    /// Setting state of lighting request
    /// </summary>
    [Serializable]
    public class SetStateOfLightingRequest
    {
        public EffectType effectID;
        public string desiredColor;
        public DurationMs durationsMs;
    }
}
