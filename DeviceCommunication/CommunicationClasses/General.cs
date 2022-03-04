using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCommunication.CommunicationClasses
{
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

    [Serializable]
    public class EffectInfo
    {
        public uint colorFade;
        public uint effectFade;
        public uint effectStep;
    }

}
