using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.Get;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Device
{
    /// <summary>
    /// This packet lets you set the current level of power on the device.
    /// 
    /// Will return one StatePower (22) message
    /// </summary>
    internal class SetPower : Payload, ISendable
    {
        /// <summary>
        /// If you specify 0 the light will turn off and if you specify 65535 the device will turn on.
        /// </summary>
        public ushort Level { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetPower"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetPower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetPower(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 2)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 2");

            Level = BitConverter.ToUInt16(bytes, 0);
        }

        public SetPower(ushort level) 
            : base(
                  BitConverter.GetBytes(level)
              )
        {
            Level = level;
        }

        public override string ToString()
        {
            return $@"Level: {Level}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetPower setPower = (SetPower)obj;
                return Level == setPower.Level;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StatePower) };
        }
    }
}
