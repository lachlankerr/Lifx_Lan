using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Enums
{
    /// <summary>
    /// The LIFX LAN protocol supports changing the color of a device over time in accordance with the shape of a waveform. 
    /// For devices that support multiple zones, these effects will treat all zones as one zone and the entire device will perform the waveform effect as a single color.
    /// 
    /// These waveforms allow us to combine functions such as fading, pulsing, etc by applying waveform interpolation on the modulation between two colors.
    /// 
    /// See https://lan.developer.lifx.com/docs/waveforms for diagrams of each waveform
    /// </summary>
    internal enum Waveform : byte
    {
        /// <summary>
        /// Light interpolates linearly from current color to new color
        /// </summary>
        SAW = 0,

        /// <summary>
        /// The color will cycle smoothly from current color to new color and then end back at current color
        /// </summary>
        SINE = 1,

        /// <summary>
        /// Light interpolates smoothly from current color to new color
        /// </summary>
        HALF_SINE = 2,

        /// <summary>
        /// Light interpolates linearly from current color to color, then back to current color
        /// </summary>
        TRIANGLE = 3,

        /// <summary>
        /// The color will be set immediately to new color, then back to current color after the duty cycle fraction expires
        /// </summary>
        PULSE = 4,
    }
}
