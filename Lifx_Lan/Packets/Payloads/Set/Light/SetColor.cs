using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Light
{
    /// <summary>
    /// This packet lets you set the HSBK value for the light. 
    /// For devices that have multiple zones, this will set all Zones on the device to this color.
    /// 
    /// Will return one LightState (107) message
    /// </summary>
    internal class SetColor : Payload, ISendable
    {
        public byte Reserved6 { get; } = 0; //dont use Reserved type for single byte reserved, ends up with more work

        /// <summary>
        /// The section of the color spectrum that represents the color of your device. 
        /// So for example red is 0, green is 120, etc
        /// Usually hue is a value between 0 and 360
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

        /// <summary>
        /// The time it will take to transition to the new HSBK in milliseconds.
        /// </summary>
        public uint Duration { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetColor"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetColor"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetColor(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 13)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 13");

            Reserved6 = bytes[0];                               //1
            Hue = BitConverter.ToUInt16(bytes, 1);              //2
            Saturation = BitConverter.ToUInt16(bytes, 3);       //2
            Brightness = BitConverter.ToUInt16(bytes, 5);       //2
            Kelvin = BitConverter.ToUInt16(bytes, 7);           //2
            Duration = BitConverter.ToUInt32(bytes, 9);         //4
        }

        public SetColor(byte reserved6, ushort hue, ushort saturation, ushort brightness, ushort kelvin, uint duration) 
            : base(
                  new byte[] { reserved6 }
                  .Concat(BitConverter.GetBytes(hue))
                  .Concat(BitConverter.GetBytes(saturation))
                  .Concat(BitConverter.GetBytes(brightness))
                  .Concat(BitConverter.GetBytes(kelvin))
                  .Concat(BitConverter.GetBytes(duration))
                  .ToArray()
              )
        {
            Reserved6 = reserved6;
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
            Kelvin = kelvin;
            Duration = duration;
        }

        public override string ToString()
        {
            return $@"Reserved6: {Reserved6}
Hue: {LightState.UInt16ToHue(Hue)} ({Hue})
Saturation: {LightState.UInt16ToPercentage(Saturation) * 100.0f}% ({Saturation})
Brightness: {LightState.UInt16ToPercentage(Brightness) * 100.0f}% ({Brightness})
Kelvin: {Kelvin}
Duration: {Duration}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetColor setColor = (SetColor)obj;
                return Reserved6 == setColor.Reserved6 && 
                       Hue == setColor.Hue &&
                       Saturation == setColor.Saturation &&
                       Brightness == setColor.Brightness &&
                       Kelvin == setColor.Kelvin &&
                       Duration == setColor.Duration;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Reserved6, Hue, Saturation, Brightness, Kelvin, Duration);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None; //TODO: really? should be FeaturesFlags.Color
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(LightState) };
        }
    }
}
