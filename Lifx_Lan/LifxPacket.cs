﻿using System;
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

        public LifxPacket(Pkt_Type pkt_type, bool tagged = false,
                          UInt32 source = FrameHeader.DEFAULT_SOURCE, bool res_required = false, bool ack_required = false,
                          byte sequence = 1)
        {
            this.frameHeader = new FrameHeader(FrameHeader.MIN_SIZE, tagged, source);
            this.frameAddress = new FrameAddress(res_required, ack_required, sequence);
            this.protocolHeader = new ProtocolHeader(pkt_type);
            this.payload = new Payload();
        }

        public LifxPacket(byte[] target, Pkt_Type pkt_type, bool tagged = false,
                          UInt32 source = FrameHeader.DEFAULT_SOURCE, bool res_required = false, bool ack_required = false,
                          byte sequence = 1)
        {
            this.frameHeader = new FrameHeader(FrameHeader.MIN_SIZE, tagged, source);
            this.frameAddress = new FrameAddress(target, res_required, ack_required, sequence);
            this.protocolHeader = new ProtocolHeader(pkt_type);
            this.payload = new Payload();
        }

        public LifxPacket(byte[] target, Pkt_Type pkt_type, byte[] payload, bool tagged = false, 
                          UInt32 source = FrameHeader.DEFAULT_SOURCE, bool res_required = false, bool ack_required = false, 
                          byte sequence = 1)
        {
            this.frameHeader = new FrameHeader((ushort)(FrameHeader.MIN_SIZE + payload.Length), tagged, source);
            this.frameAddress = new FrameAddress(target, res_required, ack_required, sequence);
            this.protocolHeader = new ProtocolHeader(pkt_type);
            this.payload = new Payload(payload);
        }

        public LifxPacket(UInt16 size, UInt16 protocol, bool addressable, bool tagged, byte origin, UInt32 source,
                          byte[] target, byte[] reserved2, bool res_required, bool ack_required, byte[] reserved3, byte sequence,
                          byte[] reserved4, Pkt_Type pkt_type, byte[] reserved5,
                          byte[] payload)
        {
            this.frameHeader = new FrameHeader(size, protocol, addressable, tagged, origin, source);
            this.frameAddress = new FrameAddress(target, reserved2, res_required, ack_required, reserved3, sequence);
            this.protocolHeader = new ProtocolHeader(reserved4, pkt_type, reserved5);
            this.payload = new Payload(payload);
        }

        public byte[] ToBytes()
        {
            return frameHeader.ToBytes().Concat(frameAddress.ToBytes()).Concat(protocolHeader.ToBytes()).Concat(payload.ToBytes()).ToArray();
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) 
                return false;
            else
            {
                LifxPacket packet = (LifxPacket)obj;
                return this.frameHeader.Equals(packet.frameHeader) &&
                       this.frameAddress.Equals(packet.frameAddress) &&
                       this.protocolHeader.Equals(packet.protocolHeader) &&
                       this.payload.Equals(packet.payload);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(frameHeader, frameAddress, protocolHeader, payload);
        }
    }
}
