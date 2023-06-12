﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
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
    }
}
