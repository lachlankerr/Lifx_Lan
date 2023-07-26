using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Enums
{
    [Flags]
    public enum FeaturesFlags : ushort
    {
        /// <summary>
        /// The product has no features available
        /// </summary>
        None = 0b00000000_00000000, // 0,

        /// <summary>
        /// The light supports emitting HEV light
        /// </summary>
        Hev = 0b00000000_00000001, // 1 << 0,

        /// <summary>
        /// The light changes physical appearance when the Hue value is changed
        /// </summary>
        Color = 0b00000000_00000010, // 1 << 1,

        /// <summary>
        /// The light may be connected to physically separated hardware (currently only the LIFX Tile)
        /// </summary>
        Chain = 0b00000000_00000100, // 1 << 2,

        /// <summary>
        /// The light supports a 2D matrix of LEDs (the Tile and Candle)
        /// </summary>
        Matrix = 0b00000000_00001000, // 1 << 3,

        /// <summary>
        /// The device has relays for controlling physical power to something (the LIFX Switch)
        /// </summary>
        Relays = 0b00000000_00010000, // 1 << 4,

        /// <summary>
        /// The device has physical buttons to press (the LIFX Switch)
        /// </summary>
        Buttons = 0b00000000_00100000, // 1 << 5,

        /// <summary>
        /// The light supports emitting infrared light
        /// </summary>
        Infrared = 0b00000000_01000000, // 1 << 6,

        /// <summary>
        /// "Linear Zones"
        /// The light supports a 1D linear array of LEDs (the Z and Beam)
        /// </summary>
        Multizone = 0b00000000_10000000, // 1 << 7,

        /// <summary>
        /// An array of the minimum and maximum kelvin values this device supports. 
        /// If the numbers are the same then the device does not support variable kelvin values. 
        /// It is null for devices that aren't lighting products (the LIFX Switch)
        /// </summary>
        TemperatureRange = 0b00000001_00000000, // 1 << 8,

        /// <summary>
        /// "Extended Linear Zones"
        /// The more capable extended API for multizone control that lets us control all the zones on the device with a single message instead of many.
        /// </summary>
        ExtendedMultizone = 0b00000010_00000000, // 1 << 9,
    }
}
