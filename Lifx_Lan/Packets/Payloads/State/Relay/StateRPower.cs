using Lifx_Lan.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.Relay
{
    /// <summary>
    /// Current models of the LIFX switch do not have dimming capability, so the two valid values are 0 for off and 65535 for on.
    /// 
    /// This packet requires the device has the Relays capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetRPower (816) and SetRPower (817) messages
    /// </summary>
    internal class StateRPower : Payload, IReceivable
    {
        /// <summary>
        /// The relay on the switch starting from 0.
        /// </summary>
        public byte Relay_Index { get; } = 0;

        /// <summary>
        /// The new value of the relay
        /// </summary>
        public ushort Level { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateRPower"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateRPower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateRPower(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 3)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 3");

            Relay_Index = bytes[0];
            Level = BitConverter.ToUInt16(bytes, 1);
        }

        public override string ToString()
        {
            return $@"Relay_Index: {Relay_Index}
Level: {Level}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateRPower stateRPower = (StateRPower)obj;
                return Relay_Index == stateRPower.Relay_Index &&
                       Level == stateRPower.Level;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Relay_Index, Level);
        }
    }
}
