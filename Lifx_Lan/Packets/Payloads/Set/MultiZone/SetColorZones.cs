using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.MultiZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.MultiZone
{
    /// <summary>
    /// Set a segment of your strip to a HSBK value. 
    /// If your devices supports extended multizone messages it is recommended you use those messages instead.
    /// 
    /// Will return one StateMultiZone (506) message
    /// 
    /// This packet requires the device has the Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetColorZones : Payload, ISendable
    {
        /// <summary>
        /// The first zone in the segment we are changing.
        /// </summary>
        public byte Start_Index { get; } = 0;

        /// <summary>
        /// The last zone in the segment we are changing
        /// </summary>
        public byte End_Index { get; } = 0;

        public ushort Hue { get; } = 0;

        public ushort Saturation { get; } = 0;

        public ushort Brightness { get; } = 0;

        public ushort Kelvin { get; } = 0;

        /// <summary>
        /// The amount of time it takes to transition to the new values in milliseconds.
        /// </summary>
        public uint Duration { get; } = 0;

        public MultiZoneApplicationRequest Apply { get; } = MultiZoneApplicationRequest.NO_APPLY;

        /// <summary>
        /// Creates an instance of the <see cref="SetColorZones"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetColorZones"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetColorZones(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 15)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 15");

            Start_Index = bytes[0];
            End_Index = bytes[1];
            Hue = BitConverter.ToUInt16(bytes, 2);
            Saturation = BitConverter.ToUInt16(bytes, 4);
            Brightness = BitConverter.ToUInt16(bytes, 6);
            Kelvin = BitConverter.ToUInt16(bytes, 8);
            Duration = BitConverter.ToUInt32(bytes, 10);
            Apply = (MultiZoneApplicationRequest)bytes[14];
        }

        public SetColorZones(byte start_index, byte end_index, ushort hue, ushort saturation, ushort brightness, ushort kelvin, uint duration, MultiZoneApplicationRequest apply)
            : base(
                  new byte[] { start_index, end_index }
                  .Concat(BitConverter.GetBytes(hue))
                  .Concat(BitConverter.GetBytes(saturation))
                  .Concat(BitConverter.GetBytes(brightness))
                  .Concat(BitConverter.GetBytes(kelvin))
                  .Concat(BitConverter.GetBytes(duration))
                  .Concat(new byte[] { (byte)apply })
                  .ToArray()
              ) 
        { 
            Start_Index = start_index;
            End_Index = end_index;
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
            Kelvin = kelvin;
            Duration = duration;
            Apply = apply;
        }

        public override string ToString()
        {
            return $@"Start_Index: {Start_Index}
End_Index: {End_Index}
Hue: {Hue}
Saturation: {Saturation}
Brightness: {Brightness}
Kelvin: {Kelvin}
Duration: {Duration}
Apply: {Apply} ({(byte)Apply})";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetColorZones setColorZones = (SetColorZones)obj;
                return Start_Index == setColorZones.Start_Index &&
                       End_Index == setColorZones.End_Index &&
                       Hue == setColorZones.Hue &&
                       Saturation == setColorZones.Saturation &&
                       Brightness == setColorZones.Brightness &&
                       Kelvin == setColorZones.Kelvin &&
                       Duration == setColorZones.Duration &&
                       Apply == setColorZones.Apply;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start_Index, End_Index, Hue, Saturation, Brightness, Kelvin, Duration, Apply);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Multizone;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateMultiZone) };
        }
    }
}
