using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// Variable length payload
    /// </summary>
    internal class Payload
    {
        /// <summary>
        /// The payload data as a byte array.
        /// </summary>
        public byte[] Bytes { get; } = new byte[0];

        public Payload()
        {

        }

        /// <summary>
        /// Creates an instance of the <see cref="Payload"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="Payload"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public Payload(byte[] bytes)
        {
            Bytes = bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Bytes;
        }

        public override string ToString()
        {
            return $@"Data: {BitConverter.ToString(Bytes)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Payload payload = (Payload)obj;
                return Bytes.SequenceEqual(payload.Bytes);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Bytes);
        }
    }
}
