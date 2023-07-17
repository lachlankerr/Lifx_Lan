using Lifx_Lan.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.Light
{
    /// <summary>
    /// This packet tells you the result of the last HEV cycle that was run
    /// 
    /// This packet requires the device has the hev capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetLastHevCycleResult (148) message
    /// </summary>
    internal class StateLastHevCycleResult : Payload, IReceivable
    {
        /// <summary>
        /// An enum saying whether the last cycle completed or interrupted.
        /// </summary>
        public LightLastHevCycleResult Result { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateLastHevCycleResult"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateLastHevCycleResult"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateLastHevCycleResult(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 1)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 1");

            Result = (LightLastHevCycleResult)bytes[0];
        }

        public override string ToString()
        {
            return $@"Brightness: {Result}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateLastHevCycleResult stateLastHevCycleResult = (StateLastHevCycleResult)obj;
                return Result == stateLastHevCycleResult.Result;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Result);
        }
    }
}
