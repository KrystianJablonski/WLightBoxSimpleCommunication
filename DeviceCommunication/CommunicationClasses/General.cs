using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCommunication.CommunicationClasses
{
    /// <summary>
    /// Color modes enum according to wLightBox API
    /// </summary>
    public enum ColorMode
    {
        RGBW = 1,
        RGB,
        MONO,
        RGBorW,
        CT,
        CTx2,
        RGBWW,
    }

    /// <summary>
    /// Effect types enum according to wLightBox API
    /// </summary>
    public enum EffectType
    {
        None,
        Effect1,
        Effect2,
        Effect3,
        Effect4,
        Effect5,
        Effect6,
        Effect7,
        Effect8,
        Effect9,
        Effect10,
    }

    /// <summary>
    /// Duration ms used in the wLightBox communication
    /// </summary>
    [Serializable]
    public class DurationMs
    {
        public uint colorFade;
        public uint effectFade;
        public uint effectStep;
    }

}
