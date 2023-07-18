using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Get
{
    /// <summary>
    /// There are two possible messages that this request may return. 
    /// If the number of zones in the response is just one then you will get a single StateZone (503). 
    /// Otherwise you will get one or more StateMultiZone (506) replies.
    /// 
    /// You can determine how many StateMultizone messages you will receive by looking at the request and the first reply.
    /// 
    /// Note that if you want all the zones to be returned to you, you should set start_index to 0 and end_index to 255
    /// 
    /// This packet requires the device has the Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class GetColorZones : Payload, ISendable
    {
        /// <summary>
        /// The first zone you want to get information from
        /// </summary>
        public byte Start_Index { get; } = 0;

        /// <summary>
        /// The second zone you want to get information from
        /// </summary>
        public byte End_Index { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="GetColorZones"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="GetColorZones"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public GetColorZones(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 2)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 2");

            Start_Index = bytes[0];
            End_Index = bytes[1];
        }

        public GetColorZones(byte start_index, byte end_index) : base(new byte[] { start_index, end_index })
        {
            Start_Index = start_index;
            End_Index = end_index;
        }

        public override string ToString()
        {
            return $@"Start_Index: {Start_Index}
End_Index: {End_Index}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                GetColorZones getColorZones = (GetColorZones)obj;
                return Start_Index == getColorZones.Start_Index &&
                       End_Index == getColorZones.End_Index;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start_Index, End_Index);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Multizone;
        }
    }
}
