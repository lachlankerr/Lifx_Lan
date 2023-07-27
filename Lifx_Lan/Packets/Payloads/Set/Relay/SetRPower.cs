using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Relay
{
    /// <summary>
    /// Set the power state of a relay on a switch device. 
    /// Current models of the LIFX switch do not have dimming capability, so the two valid values are 0 for off and 65535 for on.
    /// 
    /// Will return one StateRPower (818) message
    /// 
    /// This packet requires the device has the Relays capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetRPower : Payload, ISendable
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
        /// Creates an instance of the <see cref="SetRPower"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetRPower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetRPower(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 3)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 3");

            Relay_Index = bytes[0];
            Level = BitConverter.ToUInt16(bytes, 1);
        }

        public SetRPower(byte relay_index, ushort level)
            : base(
                  new byte[] { relay_index }
                  .Concat(BitConverter.GetBytes(level))
                  .ToArray()
                  )
        {
            Relay_Index = relay_index;
            Level = level;
        }

        public override string ToString()
        {
            return $@"Relay_Index: {Relay_Index}
Level: {(Level == 0 ? "Off" : "On")} ({Level})";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetRPower setRPower = (SetRPower)obj;
                return Relay_Index == setRPower.Relay_Index &&
                       Level == setRPower.Level;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Relay_Index, Level);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Relays;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(SetRPower) };
        }
    }
}
