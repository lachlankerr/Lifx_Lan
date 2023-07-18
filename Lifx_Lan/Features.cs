using Lifx_Lan.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class Features
    {
        /// <summary>
        /// The light supports emitting HEV light
        /// </summary>
        public bool hev { get; set; } = false;

        /// <summary>
        /// The light changes physical appearance when the Hue value is changed
        /// </summary>
        public bool color { get; set; } = false;

        /// <summary>
        /// The light may be connected to physically separated hardware (currently only the LIFX Tile)
        /// </summary>
        public bool chain { get; set; } = false;

        /// <summary>
        /// The light supports a 2D matrix of LEDs (the Tile and Candle)
        /// </summary>
        public bool matrix { get; set; } = false;

        /// <summary>
        /// The device has relays for controlling physical power to something (the LIFX Switch)
        /// </summary>
        public bool relays { get; set; } = false;

        /// <summary>
        /// The device has physical buttons to press (the LIFX Switch)
        /// </summary>
        public bool buttons { get; set; } = false;

        /// <summary>
        /// The light supports emitting infrared light
        /// </summary>
        public bool infrared { get; set; } = false;

        /// <summary>
        /// The light supports a 1D linear array of LEDs (the Z and Beam)
        /// </summary>
        public bool multizone { get; set; } = false;

        /// <summary>
        /// An array of the minimum and maximum kelvin values this device supports. 
        /// If the numbers are the same then the device does not support variable kelvin values. 
        /// It is null for devices that aren't lighting products (the LIFX Switch)
        /// </summary>
        public int[] temperature_range { get; set; } = new int[2];

        /// <summary>
        /// The more capable extended API for multizone control that lets us control all the zones on the device with a single message instead of many.
        /// </summary>
        public bool extended_multizone { get; set; } = false;

        public override string ToString()
        {
            return $@"Features: {AsFlag()} ({(ushort)AsFlag()})";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Features features = (Features)obj;
                return hev                          == features.hev &&
                       color                        == features.color &&
                       chain                        == features.chain &&
                       matrix                       == features.matrix &&
                       relays                       == features.relays &&
                       buttons                      == features.buttons &&
                       infrared                     == features.infrared &&
                       multizone                    == features.multizone &&
                       temperature_range.SequenceEqual(features.temperature_range) && 
                       extended_multizone           == features.extended_multizone;
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(hev);
            hash.Add(color);
            hash.Add(chain);
            hash.Add(matrix);
            hash.Add(relays);
            hash.Add(buttons);
            hash.Add(infrared);
            hash.Add(multizone);
            hash.Add(temperature_range);
            hash.Add(extended_multizone);
            return hash.ToHashCode();
        }

        public FeaturesFlags AsFlag()
        {
            return (hev                         ? FeaturesFlags.Hev                 : FeaturesFlags.None) |
                   (color                       ? FeaturesFlags.Color               : FeaturesFlags.None) |
                   (chain                       ? FeaturesFlags.Chain               : FeaturesFlags.None) |
                   (matrix                      ? FeaturesFlags.Matrix              : FeaturesFlags.None) |
                   (relays                      ? FeaturesFlags.Relays              : FeaturesFlags.None) |
                   (buttons                     ? FeaturesFlags.Buttons             : FeaturesFlags.None) |
                   (infrared                    ? FeaturesFlags.Infrared            : FeaturesFlags.None) |
                   (multizone                   ? FeaturesFlags.Multizone           : FeaturesFlags.None) |
                   (temperature_range[0] != 0   ? FeaturesFlags.TemperatureRange    : FeaturesFlags.None) | //lowest is currently 1500
                   (extended_multizone          ? FeaturesFlags.ExtendedMultizone   : FeaturesFlags.None);
        }
    }
}
