using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.MultiZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Tiles
{
    /// <summary>
    /// Allows you to specify the position of this device in the chain relative to other device in the chain.
    /// 
    /// You can find more information about this data by looking at Tile Positions.
    /// 
    /// This message has no response packet even if you set res_required=1.
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetUserPosition : Payload, ISendable
    {
        /// <summary>
        /// The device to change. 
        /// This is 0 indexed and starts from the device closest to the controller.
        /// </summary>
        public byte Tile_Index { get; } = 0;

        public Reserved Reserved6 { get; } = 2;

        public float X { get; } = 0;

        public float Y { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetUserPosition"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetUserPosition"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetUserPosition(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 11)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 11");

            Tile_Index = bytes[0];
            Reserved6 = bytes.Skip(1).Take(2).ToArray();
            X = BitConverter.ToSingle(bytes, 3);
            Y = BitConverter.ToSingle(bytes, 7);
        }

        public SetUserPosition(byte tile_index, Reserved reserved6, float x, float y)
            : base(
                  new byte[] { tile_index }
                  .Concat(reserved6)
                  .Concat(BitConverter.GetBytes(x))
                  .Concat(BitConverter.GetBytes(y))
                  .ToArray()
              )
        {
            Tile_Index = tile_index;
            Reserved6 = reserved6;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $@"Tile_Index: {Tile_Index}
Reserved6: {Reserved6}
X: {X}
Y: {Y}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetUserPosition setUserPosition = (SetUserPosition)obj;
                return Tile_Index == setUserPosition.Tile_Index &&
                       Reserved6 == setUserPosition.Reserved6 &&
                       X == setUserPosition.X &&
                       Y == setUserPosition.Y;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile_Index, Reserved6, X, Y);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Matrix;
        }

        public static Type[] ReturnMessages()
        {
            return Array.Empty<Type>();
        }
    }
}
