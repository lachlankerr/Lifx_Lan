using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// This packet represents a single HSBK value. 
    /// It is used in packets that set a different HSBK value for devices with multiple zones.
    /// </summary>
    internal class Color
    {
        /// <summary>
        /// The section of the color spectrum that represents the color of your device. 
        /// So for example red is 0, green is 120, etc
        /// </summary>
        public UInt16 Hue { get; } = 0;

        /// <summary>
        /// How strong the color is. 
        /// So a zero saturation is completely white, whilst full saturation is the full color
        /// </summary>
        public UInt16 Saturation { get; } = 0;

        /// <summary>
        /// How bright the color is. 
        /// So zero brightness is the same as the device is off, while full brightness be just that.
        /// </summary>
        public UInt16 Brightness { get; } = 0;

        /// <summary>
        /// The "temperature" when the device has zero saturation. 
        /// So a higher value is a cooler white (more blue) whereas a lower value is a warmer white (more yellow)
        /// </summary>
        public UInt16 Kelvin { get; } = 0;

        public Color(UInt16 hue, UInt16 saturation, UInt16 brightness, UInt16 kelvin) 
        { 
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
            Kelvin = kelvin;
        }

        public override string ToString()
        {
            return $@"Hue: {Hue}
Saturation: {Saturation}
Brightness: {Brightness}
Kelvin: {Kelvin}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Color color = (Color)obj;
                return this.Hue == color.Hue &&
                       this.Saturation == color.Saturation &&
                       this.Brightness == color.Brightness && 
                       this.Kelvin == color.Kelvin;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hue, Saturation, Brightness, Kelvin);
        }
    }
}
