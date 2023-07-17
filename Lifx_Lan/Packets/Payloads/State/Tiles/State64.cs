using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.Tiless
{
    /// <summary>
    /// The current HSBK values of the zones in a single device.
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the Get64 (707) and Set64 (715) messages
    /// </summary>
    internal class State64 : Payload, IReceivable
    {
        /// <summary>
        /// The length of the Colors array
        /// </summary>
        const int LEN_COLORS = 64;

        /// <summary>
        /// The index of the device in the chain this packet refers to. This is 0 based starting from the device closest to the controller.
        /// </summary>
        public byte Tile_Index { get; } = 0;

        public byte Reserved6 { get; } = 0;

        /// <summary>
        /// The x coordinate the colors start from
        /// </summary>
        public byte X { get; } = 0;

        /// <summary>
        /// The y coordinate the colors start from
        /// </summary>
        public byte Y { get; } = 0;

        /// <summary>
        /// The width of each row
        /// </summary>
        public byte Width { get; } = 0;

        public Color[] Colors { get; } = new Color[LEN_COLORS]; //512 bytes

        /// <summary>
        /// Creates an instance of the <see cref="State64"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="State64"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public State64(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 517)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 517");

            Tile_Index = bytes[0];
            Reserved6 = bytes[1];
            X = bytes[2];
            Y = bytes[3];
            Width = bytes[4];

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

        public override string ToString()
        {
            return $@"Tile_Index: {Tile_Index}
Reserved6: {Reserved6}
X: {X}
Y: {Y}
Width: {Width}
Colors: 
{string.Join($"\n\n", Colors.ToList())}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                State64 state64 = (State64)obj;
                return Tile_Index == state64.Tile_Index &&
                       Reserved6 == state64.Reserved6 &&
                       X == state64.X &&
                       Y == state64.Y &&
                       Width == state64.Width &&
                       Colors.SequenceEqual(state64.Colors);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile_Index, Reserved6, X, Y, Width, Colors);
        }
    }
}
