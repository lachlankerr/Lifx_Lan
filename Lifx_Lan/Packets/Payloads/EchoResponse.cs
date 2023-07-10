using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This tells you the same value you specified when you sent an EchoRequest (58) to the device.
    /// </summary>
    internal class EchoResponse : Payload, IReceivable
    {
        public byte[] Echoing { get; } = new byte[64];

        /// <summary>
        /// Creates an instance of the <see cref="EchoResponse"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="EchoResponse"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public EchoResponse(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 64)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 64");

            Echoing = bytes;
        }

        public override string ToString()
        {
            return $@"Echoing: {BitConverter.ToString(Echoing)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                EchoResponse echoResponse = (EchoResponse)obj;
                return Echoing.SequenceEqual(echoResponse.Echoing);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Echoing);
        }
    }
}
