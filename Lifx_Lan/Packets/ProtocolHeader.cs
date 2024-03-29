﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Lifx_Lan.Packets.Enums;

namespace Lifx_Lan.Packets
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
        public ProtocolHeader(Pkt_Type pkt_type)
        {
            Pkt_Type = pkt_type;
        }

        [JsonConstructor]
        public ProtocolHeader(byte[] reserved4, Pkt_Type pkt_type, byte[] reserved5)
        {
            Reserved4 = reserved4;
            Pkt_Type = pkt_type;
            Reserved5 = reserved5;
        }

        public byte[] ToBytes()
        {
            byte[] typeByte = BitConverter.GetBytes((ushort)Pkt_Type);

            return Reserved4.Concat(typeByte).Concat(Reserved5).ToArray();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                ProtocolHeader protocolHeader = (ProtocolHeader)obj;
                return Reserved4.SequenceEqual(protocolHeader.Reserved4) &&
                       Pkt_Type.Equals(protocolHeader.Pkt_Type) &&
                       Reserved5.SequenceEqual(protocolHeader.Reserved5);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Reserved4, Pkt_Type, Reserved5);
        }
    }
}
