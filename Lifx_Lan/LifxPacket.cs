using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Lifx_Lan
{
    /// <summary>
    /// The LIFX protocol is a binary messaging protocol that is used to communicate with LIFX devices, sending them instructions or querying them for state.
    /// 
    /// LIFX protocol messages are structured to fit into a single UDP packet.
    /// Each LIFX protocol message has two components concatenated together: a header and a payload.
    /// The header contains metadata common to all messages and describes the type of payload to follow; the payload provides the data for the specific action being requested.
    /// Sometimes the action requires no data, in which case the payload will be 0 bytes long.
    /// 
    /// LIFX protocol messages are often used in pairs. 
    /// For querying state, one would send the device a Get message, and the device would respond to the originating IP address with the corresponding State message (e.g. LightGet and LightState). 
    /// For changing state, one would send the device a Set message, and the device would perform the change, optionally respond with a Device Acknowledgement message, and respond with a State message (e.g. DeviceSetPower, Acknowledgement, DeviceStatePower).
    /// 
    /// Numeric data-type byte-order is little-endian, which means that multi-byte fields will start with the least significant byte.
    /// </summary>
    internal class LifxPacket
    {
        public FrameHeader frameHeader;
        public FrameAddress frameAddress;
        public ProtocolHeader protocolHeader;
        public Payload payload;

        public LifxPacket(Pkt_Type type, byte[] data)
        {
            frameAddress = new FrameAddress();
            protocolHeader = new ProtocolHeader(type);

            switch(type)
            {
                case Pkt_Type.GetService:
                    break;
                case Pkt_Type.SetPower:
                    break;
            }

            payload = new Payload(data);
            frameHeader = new FrameHeader((ushort)(Convert.ToUInt16(data.Length) + FrameHeader.MIN_SIZE));
        }

        public byte[] ToBytes()
        {
            return frameHeader.ToBytes().Concat(frameAddress.ToBytes()).Concat(protocolHeader.ToBytes()).Concat(payload.ToBytes()).ToArray();
        }
    }
}
