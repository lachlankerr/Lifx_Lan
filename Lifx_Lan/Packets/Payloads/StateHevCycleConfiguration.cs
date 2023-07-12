using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet lets you get the default values of a HEV cycle on the device.
    /// 
    /// This packet requires the device has the hev capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetHevCycleConfiguration (145) and SetHevCycleConfiguration (146) messages
    /// </summary>
    internal class StateHevCycleConfiguration : Payload, IReceivable
    {
        /// <summary>
        /// Whether a short flashing indication is run at the end of an HEV cycle.
        /// </summary>
        public byte Indication { get; } = 0;

        /// <summary>
        /// This is the default duration that is used when SetHevCycle (143) is given 0 for duration_s.
        /// </summary>
        public uint Duration_S { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateHevCycleConfiguration"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateHevCycleConfiguration"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateHevCycleConfiguration(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 5)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 5");

            Indication = bytes[0];
            Duration_S = BitConverter.ToUInt32(bytes, 1);
        }

        public override string ToString()
        {
            return $@"Indication: {Indication != 0}
Duration_S: {Duration_S}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateHevCycleConfiguration stateHevCycle = (StateHevCycleConfiguration)obj;
                return Indication == stateHevCycle.Indication &&
                       Duration_S == stateHevCycle.Duration_S;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Indication, Duration_S);
        }
    }
}
