using Lifx_Lan.Packets.Payloads.State.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Get
{
    /// <summary>
    /// This packet can be used to check that a device is online and responding to you.
    /// 
    /// Will return one EchoResponse (59) message
    /// </summary>
    internal class EchoRequest : Payload, ISendable
    {
        /// <summary>
        /// The bytes you want to receive in the EchoResponse (59) message.
        /// </summary>
        public byte[] Echoing { get; } = new byte[64];

        /// <summary>
        /// Creates an instance of the <see cref="EchoRequest"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="EchoRequest"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public EchoRequest(byte[] bytes) : base(bytes)
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
                EchoRequest echoRequest = (EchoRequest)obj;
                return Echoing.SequenceEqual(echoRequest.Echoing);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Echoing);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(EchoResponse) };
        }
    }
}
