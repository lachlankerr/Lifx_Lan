using System;
using System.Collections.Generic;
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
        public Types Type { get; }

        /// <summary>
        /// Any fields named reserved should be set to all 0s.
        /// </summary>
        public byte[] Reserved_2 { get; } = new byte[2];

        /// <summary>
        /// 
        /// </summary>
        public ProtocolHeader() 
        { 

        }
    }
}
