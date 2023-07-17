using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.Light
{
    /// <summary>
    /// This says whether a HEV cycle is running on the device.
    /// 
    /// This packet requires the device has the hev capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetHevCycle (142) and SetHevCycle (143) messages
    /// </summary>
    internal class StateHevCycle : Payload, IReceivable
    {
        /// <summary>
        /// The duration, in seconds, this cycle was set to.
        /// </summary>
        public uint Duration_S { get; } = 0;

        /// <summary>
        /// The duration, in seconds, remaining in this cycle
        /// </summary>
        public uint Remaining_S { get; } = 0;

        /// <summary>
        /// The power state before the HEV cycle started, which will be the power state once the cycle completes. 
        /// This is only relevant if remaining_s is larger than 0.
        /// </summary>
        public byte Last_Power { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateHevCycle"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateHevCycle"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateHevCycle(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 9)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 9");

            Duration_S = BitConverter.ToUInt32(bytes, 0);
            Remaining_S = BitConverter.ToUInt32(bytes, 4);
            Last_Power = bytes[8];
        }

        public override string ToString()
        {
            return $@"Duration_S: {Duration_S}
Remaining_S: {Remaining_S}
Last_Power: {Last_Power != 0}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateHevCycle stateHevCycle = (StateHevCycle)obj;
                return Duration_S == stateHevCycle.Duration_S &&
                       Remaining_S == stateHevCycle.Remaining_S &&
                       Last_Power == stateHevCycle.Last_Power;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Duration_S, Remaining_S, Last_Power);
        }
    }
}
