﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class Decoder
    {
        /// <summary>
        /// Checks the received bytes are a valid <see cref="LifxPacket"/>, if not throws an exception with the error message.
        /// </summary>
        /// <param name="data">The bytes we wish to check</param>
        /// <returns>True if it's a valid <see cref="LifxPacket"/></returns>
        public static bool IsValid(byte[] data)
        {
            if (data.Length < 36)
                throw new Exception("Not enough bytes for full header, packet size: " + data.Length);
            else if (GetSize(data) != data.Length)
                throw new Exception("Size of packet does not match size field in frame header, packet size: " + data.Length + ", field size: " + GetSize(data));
            else if (GetProtocol(data) != 1024)
                throw new Exception("Protocol not 1024, value:" + GetProtocol(data));
            else if (GetOrigin(data) != 0)
                throw new Exception("Origin not 0, value:" + GetProtocol(data));
            else if (GetTarget(data)[6] != 0 && GetTarget(data)[7] != 0)
                throw new Exception("Last two bytes in target aren't 0, full value:" + GetTarget(data));
            else if (GetTagged(data) == false && GetTarget(data)[0] != 0xD0 && GetTarget(data)[1] != 0x73 && GetTarget(data)[1] != 0xD5)
                throw new Exception("Target is not of the form D0 73 D5 xx xx xx , full value:" + GetTarget(data));
            else if ((GetTagged(data) == true && !(GetTarget(data)[0] == 0 && GetTarget(data)[1] == 0 && GetTarget(data)[2] == 0 && GetTarget(data)[3] == 0 && GetTarget(data)[4] == 0 && GetTarget(data)[5] == 0)) ||
                     (GetTagged(data) == false && (GetTarget(data)[0] == 0 && GetTarget(data)[1] == 0 && GetTarget(data)[2] == 0 && GetTarget(data)[3] == 0 && GetTarget(data)[4] == 0 && GetTarget(data)[5] == 0)))
                throw new Exception("Broadcast message with incorrect values, full value:" + GetTarget(data) + ", tagged: " + GetTagged(data));
            else
                return true;
        }

        public static bool[] MaskStringToBoolArray(string maskString)
        {
            bool[] mask = new bool[maskString.Length];
            for (int i = 0; i < maskString.Length; i++)
            {
                mask[i] = maskString[i] == '0' ? false : true;
            }
            return mask;
        }

        public static byte[] Mask(byte[] data, int index, int n, string maskString)
        {
            byte[] relevantBytes = data.Skip(index).Take(n).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(MaskStringToBoolArray(maskString));
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[n];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        public static UInt16 GetSize(byte[] data)
        {
            return BitConverter.ToUInt16(data, 0);
        }

        public static UInt16 GetProtocol(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(2).Take(2).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                true, true, true, true, true, true, true, true,
                true, true, true, true, false, false, false, false, 
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[2];
            bits.CopyTo(bytes, 0);
            return BitConverter.ToUInt16(bytes, 0);*/

            return BitConverter.ToUInt16(Mask(data, 2, 2, "1111111111110000"), 0);
        }

        public static bool GetAddressable(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(3).Take(1).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                false, false, false, false, true, false, false, false,
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return BitConverter.ToBoolean(bytes, 0);*/

            return BitConverter.ToBoolean(Mask(data, 3, 1, "00001000"), 0);
        }

        public static bool GetTagged(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(3).Take(1).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                false, false, false, false, false, true, false, false,
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return BitConverter.ToBoolean(bytes, 0);*/

            return BitConverter.ToBoolean(Mask(data, 3, 1, "00000100"), 0);
        }

        public static byte GetOrigin(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(3).Take(1).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                false, false, false, false, false, false, true, true,
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];*/

            return Mask(data, 3, 1, "00000011")[0];
        }

        public static UInt32 GetSource(byte[] data)
        {
            return BitConverter.ToUInt32(data, 4);
        }

        public static byte[] GetTarget(byte[] data)
        {
            return data.Skip(8).Take(8).ToArray();
        }
        
        public static byte[] GetReserved2(byte[] data)
        {
            return data.Skip(16).Take(6).ToArray();
        }

        public static bool GetRes_Required(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(22).Take(1).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                true, false, false, false, false, false, false, false
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return BitConverter.ToBoolean(bytes, 0);*/

            return BitConverter.ToBoolean(Mask(data, 22, 1, "10000000"), 0);
        }

        public static bool GetAck_Required(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(22).Take(1).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                false, true, false, false, false, false, false, false
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return BitConverter.ToBoolean(bytes, 0);*/

            return BitConverter.ToBoolean(Mask(data, 22, 1, "01000000"), 0);
        }

        public static byte[] GetReserved3(byte[] data)
        {
            /*byte[] relevantBytes = data.Skip(22).Take(1).ToArray();
            BitArray bitArray = new BitArray(relevantBytes);
            BitArray mask = new BitArray(new bool[]
            {
                false, false, true, true, true, true, true, true
            });
            BitArray bits = bitArray.And(mask);

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes;*/

            return Mask(data, 22, 1, "00111111");
        }

        public static byte GetSequence(byte[] data)
        {
            return data.Skip(23).Take(1).ToArray()[0];
        }

        public static byte[] GetReserved4(byte[] data)
        {
            return data.Skip(24).Take(8).ToArray();
        }

        public static Pkt_Type GetPkt_Type(byte[] data)
        {
            return (Pkt_Type)BitConverter.ToUInt16(data.Skip(32).Take(2).ToArray());
        }

        public static byte[] GetReserved5(byte[] data)
        {
            return data.Skip(34).Take(2).ToArray();
        }

        public static void PrintFields(byte[] data)
        {
            Console.WriteLine("");
            Console.WriteLine("Size:\t\t" + GetSize(data));
            Console.WriteLine("Protocol:\t" + GetProtocol(data));
            Console.WriteLine("Addressable:\t" + GetAddressable(data));
            Console.WriteLine("Tagged:\t\t" + GetTagged(data));
            Console.WriteLine("Origin:\t\t" + GetOrigin(data));
            Console.WriteLine("Source:\t\t" + GetSource(data));
            Console.WriteLine("Target:\t\t" + BitConverter.ToString(GetTarget(data)));
            Console.WriteLine("Reserved2:\t" + BitConverter.ToString(GetReserved2(data)));
            Console.WriteLine("Res_Required:\t" + GetRes_Required(data));
            Console.WriteLine("Ack_Required:\t" + GetAck_Required(data));
            Console.WriteLine("Reserved3:\t" + BitConverter.ToString(GetReserved3(data)));
            Console.WriteLine("Sequence:\t" + GetSequence(data));
            Console.WriteLine("Reserved4:\t" + BitConverter.ToString(GetReserved4(data)));
            Console.WriteLine("Pkt_Type:\t" + GetPkt_Type(data) + " (" + (ushort)GetPkt_Type(data) + ")");
            Console.WriteLine("Reserved5:\t" + BitConverter.ToString(GetReserved5(data)));
        }

        public static void PrintValues(IEnumerable myList, int myWidth)
        {
            int i = myWidth;
            foreach (Object obj in myList)
            {
                if (i <= 0)
                {
                    i = myWidth;
                    Console.WriteLine();
                }
                i--;
                Console.Write("{0,8}", obj);
            }
            Console.WriteLine();
        }
    }
}
