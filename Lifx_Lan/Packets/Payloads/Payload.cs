﻿using System;
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
        /// 
        /// </summary>
        public byte[] Data { get; } = new byte[0];

        public Payload()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public Payload(byte[] data)
        {
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Data;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Payload payload = (Payload)obj;
                return Data.SequenceEqual(payload.Data);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data);
        }
    }
}