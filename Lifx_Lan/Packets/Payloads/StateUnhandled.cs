using Lifx_Lan.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// For some firmware, this packet is returned when the device receives a packet it does not know how to handle. 
    /// For now, only the LIFX Switch has this behaviour.
    /// 
    /// It will return the type of packet it couldn't handle. 
    /// For example, if you send a GetColor (101) to a LIFX switch, then you would receive one of these with a unhandled_type of 101.
    /// </summary>
    internal class StateUnhandled : Payload, IReceivable
    {
        /// <summary>
        /// The type of the packet that was ignored.
        /// </summary>
        public Pkt_Type Unhandled_Type { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateUnhandled"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateUnhandled"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateUnhandled(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 2)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 2");

            Unhandled_Type = (Pkt_Type)BitConverter.ToUInt16(bytes);
        }

        public override string ToString()
        {
            return $@"Unhandled_Type: {Unhandled_Type} ({(ushort)Unhandled_Type})";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateUnhandled stateUnhandled = (StateUnhandled)obj;
                return Unhandled_Type == stateUnhandled.Unhandled_Type;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Unhandled_Type);
        }
    }
}
