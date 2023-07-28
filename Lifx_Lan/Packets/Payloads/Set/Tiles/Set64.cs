using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Tiless;
using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Tiles
{
    /// <summary>
    /// This lets you set up to 64 HSBK values on the device.
    /// 
    /// This message has no response packet even if you set res_required=1.
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class Set64 : Payload, ISendable
    {
        /// <summary>
        /// The length of the Colors array
        /// </summary>
        const int LEN_COLORS = 64;

        /// <summary>
        /// The device to change. 
        /// This is 0 indexed and starts from the device closest to the controller.
        /// </summary>
        public byte Tile_Index { get; } = 0;

        /// <summary>
        /// The number of devices in the chain to change starting from tile_index
        /// </summary>
        public byte Length { get; } = 0;

        public Reserved Reserved6 { get; } = 1;

        /// <summary>
        /// The x co-ordinate to start applying colors from. 
        /// You likely want this to be 0.
        /// </summary>
        public byte X { get; } = 0;

        /// <summary>
        /// The y co-ordinate to start applying colors from. You likely want this to be 0.
        /// </summary>
        public byte Y { get; } = 0;

        /// <summary>
        /// The width of the square you're applying colors to. 
        /// This should be 8 for the LIFX Tile and 5 for the LIFX Candle.
        /// </summary>
        public byte Width { get; } = 0;

        /// <summary>
        /// The time it will take to transition to new state in milliseconds.
        /// </summary>
        public uint Duration { get; } = 0;

        /// <summary>
        /// The HSBK values to assign to each zone specified by this packet.
        /// </summary>
        public Color[] Colors { get; } = new Color[LEN_COLORS]; //512 bytes

        /// <summary>
        /// Creates an instance of the <see cref="Set64"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="Set64"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public Set64(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 522)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 522");

            Tile_Index = bytes[0];
            Length = bytes[1];
            Reserved6 = bytes[2];
            X = bytes[3];
            Y = bytes[4];
            Width = bytes[5];
            Duration = BitConverter.ToUInt32(bytes, 6);

            for (int i = 0; i < LEN_COLORS; i++)
            {
                int offset = i * Color.SIZE;
                ushort hue = BitConverter.ToUInt16(bytes, 10 + offset);
                ushort saturation = BitConverter.ToUInt16(bytes, 12 + offset);
                ushort brightness = BitConverter.ToUInt16(bytes, 14 + offset);
                ushort kelvin = BitConverter.ToUInt16(bytes, 16 + offset);
                Colors[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        public Set64(byte tile_index, byte length, Reserved reserved6, byte x, byte y, byte width, uint duration, Color[] colors)
            : base(
                  new byte[] { tile_index, length }
                  .Concat(reserved6)
                  .Concat(new byte[] { x, y, width })
                  .Concat(BitConverter.GetBytes(duration))
                  .Concat(ToBytes(colors))
                  .ToArray()
              )
        {
            Tile_Index = tile_index;
            Length = length;
            Reserved6 = reserved6;
            X = x; 
            Y = y;
            Width = width;
            Duration = duration;
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
            return $@"Tile_Index: {Tile_Index}
Length: {Length}
Reserved6: {Reserved6}
X: {X}
Y: {Y}
Width: {Width}
Duration: {Duration}
Colors: 
{string.Join($"\n\n", Colors.ToList())}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Set64 set64 = (Set64)obj;
                return Tile_Index == set64.Tile_Index &&
                       Length == set64.Length &&
                       Reserved6 == set64.Reserved6 &&
                       X == set64.X &&
                       Y == set64.Y &&
                       Width == set64.Width &&
                       Duration == set64.Duration &&
                       Colors.SequenceEqual(set64.Colors);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile_Index, Length, Reserved6, X, Y, Width, Duration, Colors);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Matrix;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(State64) }; //TODO: double check 
        }
    }
}
