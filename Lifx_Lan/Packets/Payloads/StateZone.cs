using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This represents the HSBK value of a single zone on your strip.
    /// 
    /// This packet requires the device has the Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetColorZones (502) and SetColorZones (501) messages
    /// </summary>
    internal class StateZone : Payload, IReceivable
    {
        /// <summary>
        /// The total number of zones on the strip.
        /// </summary>
        public byte Zones_Count { get; } = 0;

        /// <summary>
        /// The zone this packet refers to.
        /// </summary>
        public byte Zones_Index { get; } = 0;

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
        /// Creates an instance of the <see cref="StateZone"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateZone"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateZone(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 10)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 10");

            Zones_Count = bytes[0];
            Zones_Index = bytes[1];
            Hue = BitConverter.ToUInt16(bytes, 2);              //2
            Saturation = BitConverter.ToUInt16(bytes, 4);       //2
            Brightness = BitConverter.ToUInt16(bytes, 6);       //2
            Kelvin = BitConverter.ToUInt16(bytes, 8);           //2
        }

        public override string ToString()
        {
            return $@"Zones_Count: {Zones_Count}
Zones_Index: {Zones_Index}
Hue: {LightState.UInt16ToHue(Hue)} ({Hue})
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
                StateZone stateZone = (StateZone)obj;
                return Zones_Count == stateZone.Zones_Count &&
                       Zones_Index == stateZone.Zones_Index &&
                       Hue == stateZone.Hue &&
                       Saturation == stateZone.Saturation &&
                       Brightness == stateZone.Brightness &&
                       Kelvin == stateZone.Kelvin;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Zones_Count, Zones_Index, Hue, Saturation, Brightness, Kelvin);
        }
    }
}
