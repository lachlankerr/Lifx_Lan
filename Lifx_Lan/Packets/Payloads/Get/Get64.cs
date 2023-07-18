using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Get
{
    /// <summary>
    /// Used to get the HSBK values of all the zones in devices connected in the chain.
    /// 
    /// This will return one or more State64 (711) messages. 
    /// The maximum number of messages you will receive is the number specified by length in your request.
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class Get64 : Payload, ISendable
    {
        /// <summary>
        /// The first item in the chain you want zones
        /// </summary>
        public byte Tile_Index { get; } = 0;

        /// <summary>
        /// The number of tiles after tile_index you want HSBK values from.
        /// </summary>
        public byte Length { get; } = 0;

        public byte Reserved6 { get; } = 0;

        /// <summary>
        /// The x value to start from. 
        /// You likely always want this to be 0.
        /// </summary>
        public byte X { get; } = 0;

        /// <summary>
        /// The y value to start from. 
        /// You likely always want this to be 0
        /// </summary>
        public byte Y { get; } = 0;

        /// <summary>
        /// The width of each item in the chain. 
        /// For the LIFX Tile you want this to be 8 and for the LIFX Candle you want this to be 5.
        /// </summary>
        public byte Width { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="Get64"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="Get64"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public Get64(byte[] bytes) : base (bytes) 
        {
            if (bytes.Length != 6)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 6");

            Tile_Index  = bytes[0];
            Length      = bytes[1];
            Reserved6   = bytes[2];
            X           = bytes[3];
            Y           = bytes[4];
            Width       = bytes[5];
        }

        public Get64(byte tile_index, byte length, byte reserved6, byte x, byte y, byte width) : base(new byte[] { tile_index, length, reserved6, x, y, width })
        {
            Tile_Index = tile_index;
            Length = length;
            Reserved6 = reserved6;
            X = x;
            Y = y;
            Width = width;
        }

        public override string ToString()
        {
            return $@"Tile_Index: {Tile_Index}
Length: {Length}
Reserved6: {Reserved6}
X: {X}
Y: {Y}
Width: {Width}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Get64 get64 = (Get64)obj;
                return Tile_Index == get64.Tile_Index &&
                       Length == get64.Length && 
                       Reserved6 == get64.Reserved6 &&
                       X == get64.X &&
                       Y == get64.Y &&
                       Width == get64.Width;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile_Index, Length, Reserved6, X, Y, Width);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Relays;
        }
    }
}
