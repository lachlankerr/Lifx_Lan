using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// Frame Header (8 bytes)
    /// </summary>
    internal class FrameHeader
    {
        public const UInt16 MIN_SIZE = 36;
        public const UInt16 PROTOCOL = 1024;

        /// <summary>
        /// Size of entire message in bytes including this field.
        /// 36 minimum: frame header 8 + frame address 16 + protocol header 12
        /// </summary>
        public UInt16 Size { get; } = MIN_SIZE;

        /// <summary>
        /// Protocol number: must be 1024 (decimal)
        /// </summary>
        public UInt16 Protocol { get; } = PROTOCOL;

        /// <summary>
        /// Message includes a target address: must be one (1)
        /// </summary>
        public bool Addressable { get; } = true;

        /// <summary>
        /// Determines usage of the Frame Address target field
        /// 
        /// The tagged field is a boolean flag that indicates whether the Frame Address target field is being used to address an individual device or all devices. 
        /// When you broadcast a message to the network (i.e. broadcasting a GetService (2) for discovery) you should set this field to 1 and for all other messages you should set this to 0.
        /// </summary>
        public bool Tagged { get; } = false;

        /// <summary>
        /// Message origin indicator: must be zero (0)
        /// </summary>
        public byte Origin { get; } = 0;

        /// <summary>
        /// Source identifier: unique value set by the client, used by responses.
        /// 
        /// The source identifier allows each client to provide a unique value, which will be included in the corresponding field in Acknowledgement (45) and State packets the device sends back to you.
        /// 
        /// Due to the behavior of some older versions of firmware you should set this value to anything other than zero or one. 
        /// In some versions of the firmware a source value of 1 will be ignored. 
        /// If you set source to 0 then the device may broadcast the reply on port 56700 which can be received by all clients on the same subnet and may not be the port on which your client is listening for replies.
        /// </summary>
        public UInt32 Source { get; } = 2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">Defaults to 36</param>
        /// <param name="tagged">Defaults to false</param>
        public FrameHeader(UInt16 size = MIN_SIZE, bool tagged = false) 
        {
            Size = size;
            Tagged = tagged;
        }
    }
}
