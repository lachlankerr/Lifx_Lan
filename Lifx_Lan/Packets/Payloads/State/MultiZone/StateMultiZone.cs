using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.MultiZone
{
    /// <summary>
    /// This represents a segment of 8 HSBK values on your strip.
    /// 
    /// This packet requires the device has the Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetColorZones (502) and SetColorZones (501) messages
    /// </summary>
    internal class StateMultiZone : Payload, IReceivable
    {
        /// <summary>
        /// The length of the Colors array
        /// </summary>
        const int LEN_COLORS = 8;

        /// <summary>
        /// The total number of zones on the strip.
        /// </summary>
        public byte Zones_Count { get; } = 0;

        /// <summary>
        /// The zone this packet refers to.
        /// </summary>
        public byte Zone_Index { get; } = 0;

        /// <summary>
        /// The HSBK values of the zones this packet refers to.
        /// </summary>
        public Color[] Colors { get; } = new Color[LEN_COLORS];

        /// <summary>
        /// Creates an instance of the <see cref="StateMultiZone"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateMultiZone"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateMultiZone(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 66)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 66");

            Zones_Count = bytes[0];
            Zone_Index = bytes[1];

            for (int i = 0; i < LEN_COLORS; i++)
            {
                int offset = i * Color.SIZE;
                ushort hue = BitConverter.ToUInt16(bytes, 2 + offset);
                ushort saturation = BitConverter.ToUInt16(bytes, 4 + offset);
                ushort brightness = BitConverter.ToUInt16(bytes, 6 + offset);
                ushort kelvin = BitConverter.ToUInt16(bytes, 8 + offset);
                Colors[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        /// <summary>
        /// You can determine how many StateMultizone messages you will receive by looking at the request and the first reply.
        /// </summary>
        /// <returns></returns>
        public int ExpectedNumberOfMessages(int start_index, int end_index)
        {
            int number_segments_of_8 = (end_index - start_index) / 8;
            int count_from_request = Math.Max(1, number_segments_of_8 + 1);

            int count_from_response = (int)Math.Ceiling(Zones_Count / 8d);

            return Math.Min(count_from_request, count_from_response);
        }

        public override string ToString()
        {
            return $@"Zones_Count: {Zones_Count}
Zone_Index: {Zone_Index}
Colors: 
{string.Join($"\n\n", Colors.ToList())}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateMultiZone stateMultiZone = (StateMultiZone)obj;
                return Zones_Count == stateMultiZone.Zones_Count &&
                       Zone_Index == stateMultiZone.Zone_Index &&
                       Colors.SequenceEqual(stateMultiZone.Colors);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Zones_Count, Zone_Index, Colors);
        }
    }
}
