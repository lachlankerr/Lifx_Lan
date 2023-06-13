using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// Protocol Header (12 bytes)
    /// </summary>
    internal class ProtocolHeader
    {
        /// <summary>
        /// Any fields named reserved should be set to all 0s.
        /// </summary>
        public byte[] Reserved4 { get; } = new byte[8];

        /// <summary>
        /// Message type determines the payload being used
        /// </summary>
        public Pkt_Type Pkt_Type { get; }

        /// <summary>
        /// Any fields named reserved should be set to all 0s.
        /// </summary>
        public byte[] Reserved5 { get; } = new byte[2];

        /// <summary>
        /// 
        /// </summary>
        public ProtocolHeader(Pkt_Type type) 
        {
            Pkt_Type = type;
        }

        public ProtocolHeader(byte[] reserved4, Pkt_Type type, byte[] reserved5)
        {
            Reserved4 = reserved4;
            Pkt_Type = type;
            Reserved5 = reserved5;
        }

        public byte[] ToBytes()
        {
            byte[] typeByte = BitConverter.GetBytes((ushort)Pkt_Type);

            return Reserved4.Concat(typeByte).Concat(Reserved5).ToArray();
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                ProtocolHeader protocolHeader = (ProtocolHeader)obj;
                return this.Reserved4.SequenceEqual(protocolHeader.Reserved4) &&
                       this.Pkt_Type.Equals(protocolHeader.Pkt_Type) && 
                       this.Reserved5.SequenceEqual(protocolHeader.Reserved5);
            }
        }
    }
}
