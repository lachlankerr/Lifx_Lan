using System;
using System.Collections;
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
        /// Also known as reserved1
        /// </summary>
        public BitArray Origin { get; } = new BitArray(2);

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

        public FrameHeader(UInt16 size, UInt16 protocol, bool addressable, bool tagged, byte origin, UInt32 source)
        {
            Size = size;
            Protocol = protocol; 
            Addressable = addressable; 
            Tagged = tagged;
            Origin = new BitArray(new byte[] { origin });
            Origin.Length = 2; //necessary for equals() method to work
            Source = source;
        }

        public byte[] ToBytes()
        {
            byte[] sizeBytes = BitConverter.GetBytes(Size);
            byte[] protocolBytes = BitConverter.GetBytes(Protocol);
            byte[] sourceBytes = BitConverter.GetBytes(Source);

            BitArray protocolBits = new BitArray(protocolBytes);

            protocolBits.Set(8 + 4, Addressable);
            protocolBits.Set(8 + 5, Tagged);
            protocolBits.Set(8 + 6, Origin[1]);
            protocolBits.Set(8 + 7, Origin[0]);
            protocolBits.CopyTo(protocolBytes, 0);

            /*for (int i = 0; i < 16; i++)
            {
                if (i % 8 == 0)
                    Console.WriteLine("");
                Console.WriteLine(protocolBits[i]);
            }*/

            return sizeBytes.Concat(protocolBytes).Concat(sourceBytes).ToArray();
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                FrameHeader frameHeader = (FrameHeader)obj;
                return this.Size == frameHeader.Size &&
                       this.Protocol == frameHeader.Protocol &&
                       this.Addressable == frameHeader.Addressable &&
                       this.Tagged == frameHeader.Tagged &&
                       this.Origin.Xor(frameHeader.Origin).OfType<bool>().All(e => !e) &&
                       this.Source == frameHeader.Source;
            }
        }
    }
}
