using System;

namespace DeviceCommunication.CommunicationClasses
{
    /// <summary>
    /// Content of set state of lighting request
    /// </summary>
    [Serializable]
    public class SetStateOfLightingRequestContent
    {
        public EffectType effectID;
        public string desiredColor;
        public DurationMs durationsMs;
    }

    /// <summary>
    /// Setting state of lighting request
    /// </summary>
    [Serializable]
    public class SetStateOfLightingRequest
    {
        public SetStateOfLightingRequestContent rgbw;
    }
}
