using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet is the reply to the GetWifiFirmware (18) message
    /// </summary>
    internal class StateWifiFirmware
    {
        /// <summary>
        /// The timestamp when the wifi firmware was created as an epoch, This is only relevant for the first two generations of our products.
        /// </summary>
        public ulong Build { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[8];

        /// <summary>
        /// The minor component of the version.
        /// </summary>
        public ushort Version_Minor { get; } = 0;

        /// <summary>
        /// The major component of the version.
        /// </summary>
        public ushort Version_Major { get; } = 0;

        public StateWifiFirmware(byte[] bytes)
        {
            if (bytes.Length != 20)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 20");

            Build = BitConverter.ToUInt64(bytes, 0);
            Reserved6 = bytes.Skip(8).Take(8).ToArray();
            Version_Minor = BitConverter.ToUInt16(bytes, 16);
            Version_Major = BitConverter.ToUInt16(bytes, 18);
        }

        public override string ToString()
        {
            return $@"Build: {Build}
Reserved6: {BitConverter.ToString(Reserved6)}
Version_Minor: {Version_Minor}
Version_Major: {Version_Major}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateWifiFirmware stateWifiFirmware = (StateWifiFirmware)obj;
                return Build == stateWifiFirmware.Build &&
                       Reserved6.SequenceEqual(stateWifiFirmware.Reserved6) &&
                       Version_Minor == stateWifiFirmware.Version_Minor &&
                       Version_Major == stateWifiFirmware.Version_Major;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Build, Reserved6, Version_Minor, Version_Major);
        }
    }
}
