using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Get
{
    /// <summary>
    /// Get the power state of a relay on a switch device.
    /// 
    /// Will return one StateRPower (818) message
    /// 
    /// This packet requires the device has the Relays capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class GetRPower : Payload, ISendable
    {
        /// <summary>
        /// The relay on the switch starting from 0.
        /// </summary>
        public byte Relay_Index { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="GetRPower"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="GetRPower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public GetRPower(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 1)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 1");

            Relay_Index = bytes[0];
        }

        public GetRPower(byte relay_index) : base(new byte[] { relay_index })
        {
            Relay_Index = relay_index;
        }

        public override string ToString()
        {
            return $@"Relay_Index: {Relay_Index}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                GetRPower getRPower = (GetRPower)obj;
                return Relay_Index == getRPower.Relay_Index;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Relay_Index);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Relays;
        }
    }
}
