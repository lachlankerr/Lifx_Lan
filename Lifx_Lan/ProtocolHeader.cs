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
        public byte[] Reserved_1 { get; } = new byte[8];

        /// <summary>
        /// Message type determines the payload being used
        /// </summary>
        public MessageType Type { get; }

        /// <summary>
        /// Any fields named reserved should be set to all 0s.
        /// </summary>
        public byte[] Reserved_2 { get; } = new byte[2];

        /// <summary>
        /// 
        /// </summary>
        public ProtocolHeader(MessageType type) 
        {
            Type = type;
        }

        public byte[] ToBytes()
        {
            byte[] typeByte = BitConverter.GetBytes((ushort)Type);

            return Reserved_1.Concat(typeByte).Concat(Reserved_2).ToArray();
        }
    }
}
