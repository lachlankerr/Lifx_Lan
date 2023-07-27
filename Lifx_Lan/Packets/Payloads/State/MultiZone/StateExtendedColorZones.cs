using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.MultiZone
{
    /// <summary>
    /// The HSBK values of the zones specified in the request
    /// 
    /// This packet requires the device has the Extended Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetExtendedColorZones (511) and SetExtendedColorZones (510) messages
    /// </summary>
    internal class StateExtendedColorZones : Payload, IReceivable
    {
        /// <summary>
        /// The initial number of bytes we need to determine how big this payload should be
        /// </summary>
        public const int INIT_SIZE = 5;

        /// <summary>
        /// The maximum length the Colors array can be
        /// </summary>
        public const int MAX_COLORS = 82;

        /// <summary>
        /// The number of zones on your strip
        /// </summary>
        public ushort Zones_Count { get; } = 0;

        /// <summary>
        /// The first zone represented in the packet
        /// </summary>
        public ushort Zone_Index { get; } = 0;

        /// <summary>
        /// The number of HSBK values in the colors array that map to zones.
        /// </summary>
        public byte Colors_Count { get; } = 0;

        /// <summary>
        /// The HSBK values currently set on each zone.
        /// </summary>
        public Color[] Colors { get; } = new Color[MAX_COLORS];

        /// <summary>
        /// Creates an instance of the <see cref="StateExtendedColorZones"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateExtendedColorZones"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateExtendedColorZones(byte[] bytes) : base(bytes)
        {
            //initial check
            if (bytes.Length >= INIT_SIZE)
                throw new ArgumentException($"Not enough bytes to read the whole structure for this payload type, expected at least {INIT_SIZE}");

            Zones_Count = BitConverter.ToUInt16(bytes, 0);
            Zone_Index = BitConverter.ToUInt16(bytes, 2);
            Colors_Count = bytes[4];

            //secondary check after we have received the Colors_Count value
            if (bytes.Length != INIT_SIZE + Color.SIZE * Colors_Count)
                throw new ArgumentException($"Wrong number of bytes for this payload type, expected {INIT_SIZE + Color.SIZE * Colors_Count}");

            for (int i = 0; i < Colors_Count; i++)
            {
                int offset = i * Color.SIZE;

                ushort hue = BitConverter.ToUInt16(bytes, 5 + offset);
                ushort saturation = BitConverter.ToUInt16(bytes, 7 + offset);
                ushort brightness = BitConverter.ToUInt16(bytes, 9 + offset);
                ushort kelvin = BitConverter.ToUInt16(bytes, 11 + offset);

                Colors[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        public override string ToString()
        {
            return $@"Zones_Count: {Zones_Count}
Zone_Index: {Zone_Index}
Colors_Count: {Colors_Count}
Colors: 
{string.Join($"\n\n", Colors.ToList())}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateExtendedColorZones stateExtendedColorZones = (StateExtendedColorZones)obj;
                return Zones_Count == stateExtendedColorZones.Zones_Count &&
                       Zone_Index == stateExtendedColorZones.Zone_Index &&
                       Colors_Count == stateExtendedColorZones.Colors_Count &&
                       Colors.SequenceEqual(stateExtendedColorZones.Colors);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Zones_Count, Zone_Index, Colors_Count, Colors);
        }
    }
}
