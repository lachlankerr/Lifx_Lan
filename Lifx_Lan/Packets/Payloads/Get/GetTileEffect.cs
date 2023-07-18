using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Get
{
    /// <summary>
    /// Used to determine if the device is running a Firmware Effect.
    /// 
    /// Will return one StateTileEffect (720) message
    /// </summary>
    internal class GetTileEffect : Payload, ISendable
    {
        public byte Reserved6 { get; } = 0;

        public byte Reserved7 { get; } = 0;

        public GetTileEffect(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 2)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 2");

            Reserved6 = bytes[0];
            Reserved7 = bytes[0];
        }

        public GetTileEffect(byte reserved6, byte reserved7) : base(new byte[] { reserved6, reserved7 })
        {
            Reserved6 = reserved6;
            Reserved7 = reserved7;
        }

        public override string ToString()
        {
            return $@"Reserved6: {Reserved6}
Reserved7: {Reserved7}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                GetTileEffect getTileEffect = (GetTileEffect)obj;
                return Reserved6 == getTileEffect.Reserved6 &&
                       Reserved7 == getTileEffect.Reserved7;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Reserved6, Reserved7);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None;
        }
    }
}
