using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.MultiZone;
using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.MultiZone
{
    internal class SetExtendedColorZones : Payload, ISendable
    {
        /// <summary>
        /// The initial number of bytes we need to determine how big this payload should be
        /// </summary>
        public const int INIT_SIZE = 8;

        /// <summary>
        /// The maximum length the Colors array can be
        /// </summary>
        public const int MAX_COLORS = 82;

        /// <summary>
        /// The time it takes to transition to the new values in milliseconds.
        /// </summary>
        public uint Duration { get; } = 0;

        /// <summary>
        /// Whether to make this change now
        /// </summary>
        public MultiZoneExtendedApplicationRequest Apply { get; } = MultiZoneExtendedApplicationRequest.NO_APPLY;

        /// <summary>
        /// The first zone to apply colors from.
        /// </summary>
        public ushort Zone_Index { get; } = 0;

        /// <summary>
        /// The number of colors in the colors field to apply to the strip
        /// </summary>
        public byte Colors_Count { get; } = 0;

        /// <summary>
        /// The HSBK values to change the strip with.
        /// </summary>
        public Color[] Colors { get; } = new Color[MAX_COLORS];

        /// <summary>
        /// Creates an instance of the <see cref="SetExtendedColorZones"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetExtendedColorZones"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetExtendedColorZones(byte[] bytes) : base(bytes) 
        {
            //initial check
            if (bytes.Length >= INIT_SIZE)
                throw new ArgumentException($"Not enough bytes to read the whole structure for this payload type, expected at least {INIT_SIZE}");

            Duration = BitConverter.ToUInt16(bytes, 0);
            Apply = (MultiZoneExtendedApplicationRequest)bytes[4];
            Zone_Index = BitConverter.ToUInt16(bytes, 5);
            Colors_Count = bytes[7];

            //secondary check after we have received the Colors_Count value
            if (bytes.Length != INIT_SIZE + Color.SIZE * Colors_Count)
                throw new ArgumentException($"Wrong number of bytes for this payload type, expected {INIT_SIZE + Color.SIZE * Colors_Count}");

            for (int i = 0; i < Colors_Count; i++)
            {
                int offset = i * Color.SIZE;

                ushort hue = BitConverter.ToUInt16(bytes, 8 + offset);
                ushort saturation = BitConverter.ToUInt16(bytes, 10 + offset);
                ushort brightness = BitConverter.ToUInt16(bytes, 12 + offset);
                ushort kelvin = BitConverter.ToUInt16(bytes, 14 + offset);

                Colors[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        public SetExtendedColorZones(uint duration, MultiZoneExtendedApplicationRequest apply, ushort zone_index, byte colors_count, Color[] colors)
            : base (
                  BitConverter.GetBytes(duration)
                  .Concat(new byte[] {(byte)apply})
                  .Concat(BitConverter.GetBytes(zone_index))
                  .Concat(new byte[] { colors_count })
                  .Concat(ToBytes(colors))
                  .ToArray()
              )
        {
            Duration = duration;
            Apply = apply;
            Zone_Index = zone_index;
            Colors_Count = colors_count;
            Colors = colors;
        }

        /// <summary>
        /// TODO: one liner this
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        private static byte[] ToBytes(Color[] colors)
        {
            byte[] bytes = Array.Empty<byte>();
            foreach (Color c in colors)
            {
                bytes = bytes.Concat(c.ToBytes()).ToArray();
            }
            return bytes;
        }

        public override string ToString()
        {
            return $@"Duration: {Duration}
Apply: {Apply}
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
                SetExtendedColorZones setExtendedColorZones = (SetExtendedColorZones)obj;
                return Duration == setExtendedColorZones.Duration &&
                       Apply == setExtendedColorZones.Apply &&
                       Zone_Index == setExtendedColorZones.Zone_Index &&
                       Colors_Count == setExtendedColorZones.Colors_Count &&
                       Colors.SequenceEqual(setExtendedColorZones.Colors);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Duration, Apply, Zone_Index, Colors_Count, Colors);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.ExtendedMultizone;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateExtendedColorZones) };
        }
    }
}
