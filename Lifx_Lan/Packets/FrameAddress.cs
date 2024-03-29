﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets
{
    /// <summary>
    /// Frame Address (16 bytes)
    /// </summary>
    internal class FrameAddress
    {
        /// <summary>
        /// 6 byte device serial number or zero (0) means all devices. 
        /// The last two bytes should always be 0 bytes.
        /// 
        /// The target field represents a device serial number and appears as a hex number of the form d073d5xxxxxx. 
        /// When you address a device you should left-justify the first 6 bytes of the target field with the serial number and then zero-fill the last two bytes. 
        /// You should set this value to all zero's if you want to broadcast a message to the network.
        /// </summary>
        public byte[] Target { get; } = new byte[8];

        /// <summary>
        /// Any fields named reserved should be set to all 0s.
        /// </summary>
        public byte[] Reserved2 { get; } = new byte[6];

        /// <summary>
        /// State message required.
        /// However, Get messages will send State messages anyway and State messages to Set messages are usually not useful.
        /// 
        /// res_required set to one 1 will make the device send back one or more State messages
        /// 
        /// It is recommended you set ack_required=1 and res_required=0 when you change the device with a Set message. 
        /// When you send a Get message it is best to set ack_required=0 and res_required=0, because these messages trigger an implicit State response. 
        /// Note that when you ask for a response with a Set message that changes the visual state of the device, you will get the old values in the State message sent back.
        /// </summary>
        public bool Res_Required { get; } = false;

        /// <summary>
        /// Acknowledgement message required.
        /// 
        /// ack_required set to one 1 will cause the device to send an Acknowledgement (45) message
        /// 
        /// It is recommended you set ack_required=1 and res_required=0 when you change the device with a Set message. 
        /// When you send a Get message it is best to set ack_required=0 and res_required=0, because these messages trigger an implicit State response.
        /// </summary>
        public bool Ack_Required { get; } = true;

        /// <summary>
        /// Any fields named reserved should be set to all 0s.
        /// </summary>
        public byte Reserved3 { get; } = 0;

        /// <summary>
        /// Wrap around message sequence number
        /// 
        /// The sequence number allows the client to provide a unique value, which will be included by the LIFX device in any message that is sent in response to a message sent by the client. 
        /// This allows the client to distinguish between different messages sent with the same source identifier in the Frame Header. 
        /// We recommend that your program has one source value and keeps incrementing sequence per device for each message you send. 
        /// Once sequence reaches the maximum value of 255 for that device, roll it back to 0 and keep incrementing from there.
        /// 
        /// You can associate replies to their corresponding requests by matching the (source, sequence, target) values from the request and response packets.
        /// </summary>
        public byte Sequence { get; set; } = 1;

        public FrameAddress(bool res_required = false, bool ack_required = false, byte sequence = 1)
        {
            Res_Required = res_required;
            Ack_Required = ack_required;
            Sequence = sequence;
        }

        public FrameAddress(byte[] target, bool res_required = false, bool ack_required = false, byte sequence = 1)
        {
            Target = target;
            Res_Required = res_required;
            Ack_Required = ack_required;
            Sequence = sequence;
        }

        [JsonConstructor]
        public FrameAddress(byte[] target, byte[] reserved2, bool res_required, bool ack_required, byte reserved3, byte sequence)
        {
            Target = target;
            Reserved2 = reserved2;
            Res_Required = res_required;
            Ack_Required = ack_required;
            Reserved3 = reserved3;
            Sequence = sequence;
        }

        public byte[] ToBytes()
        {
            byte[] reservedByte = new byte[1];
            byte[] sequenceByte = { Sequence };

            BitArray reservedBits = new BitArray(reservedByte);


            BitArray reserved3Bits = new BitArray(Reserved3);
            reserved3Bits.Length = 6; //ignore the last 2 bits

            reservedBits.Set(0, Res_Required);
            reservedBits.Set(1, Ack_Required);
            reservedBits.Set(2, reserved3Bits[5]);
            reservedBits.Set(3, reserved3Bits[4]);
            reservedBits.Set(4, reserved3Bits[3]);
            reservedBits.Set(5, reserved3Bits[2]);
            reservedBits.Set(6, reserved3Bits[1]);
            reservedBits.Set(7, reserved3Bits[0]);
            reservedBits.CopyTo(reservedByte, 0);

            /*for (int i = 0; i < 16; i++)
            {
                if (i % 8 == 0)
                    Console.WriteLine("");
                Console.WriteLine(protocolBits[i]);
            }*/

            return Target.Concat(Reserved2).Concat(reservedByte).Concat(sequenceByte).ToArray();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                FrameAddress frameAddress = (FrameAddress)obj;
                return Target.SequenceEqual(frameAddress.Target) &&
                       Reserved2.SequenceEqual(frameAddress.Reserved2) &&
                       Res_Required == frameAddress.Res_Required &&
                       Ack_Required == frameAddress.Ack_Required &&
                       Reserved3 == frameAddress.Reserved3 &&
                       Sequence == frameAddress.Sequence;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Target, Reserved2, Res_Required, Ack_Required, Reserved3, Sequence);
        }
    }
}
