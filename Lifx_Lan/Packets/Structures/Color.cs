using Lifx_Lan.Packets.Payloads.State.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Structures
{
    /// <summary>
    /// This packet represents a single HSBK value. 
    /// It is used in packets that set a different HSBK value for devices with multiple zones.
    /// </summary>
    internal class Color
    {
        /// <summary>
        /// The size of a <see cref="Color"/> struct in bytes
        /// </summary>
        public const int SIZE = 8;

        /// <summary>
        /// The section of the color spectrum that represents the color of your device. 
        /// So for example red is 0, green is 120, etc
        /// </summary>
        public ushort Hue { get; } = 0;

        /// <summary>
        /// How strong the color is. 
        /// So a zero saturation is completely white, whilst full saturation is the full color
        /// </summary>
        public ushort Saturation { get; } = 0;

        /// <summary>
        /// How bright the color is. 
        /// So zero brightness is the same as the device is off, while full brightness be just that.
        /// </summary>
        public ushort Brightness { get; } = 0;

        /// <summary>
        /// The "temperature" when the device has zero saturation. 
        /// So a higher value is a cooler white (more blue) whereas a lower value is a warmer white (more yellow)
        /// </summary>
        public ushort Kelvin { get; } = 0;

        public Color(ushort hue, ushort saturation, ushort brightness, ushort kelvin)
        {
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
            Kelvin = kelvin;
        }

        public override string ToString()
        {
            return $@"Hue: {LightState.UInt16ToHue(Hue)} ({Hue})
Saturation: {LightState.UInt16ToPercentage(Saturation) * 100.0f}% ({Saturation})
Brightness: {LightState.UInt16ToPercentage(Brightness) * 100.0f}% ({Brightness})
Kelvin: {Kelvin}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Color color = (Color)obj;
                return Hue == color.Hue &&
                       Saturation == color.Saturation &&
                       Brightness == color.Brightness &&
                       Kelvin == color.Kelvin;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hue, Saturation, Brightness, Kelvin);
        }
    }
}
