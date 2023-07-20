using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Light
{
    /// <summary>
    /// This is the same as SetPower (21) but allows you to specify how long it will take to transition to the new power state.
    /// 
    /// Will return one StateLightPower (118) message
    /// </summary>
    internal class SetLightPower : Payload, ISendable
    {
        /// <summary>
        /// If you specify 0 the light will turn off and if you specify 65535 the device will turn on.
        /// </summary>
        public ushort Level { get; } = 0;

        /// <summary>
        /// The time it will take to transition to the new state in milliseconds.
        /// </summary>
        public uint Duration { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetLightPower"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetLightPower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetLightPower(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 6)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 6");

            Level = BitConverter.ToUInt16(bytes, 0);
            Duration = BitConverter.ToUInt32(bytes, 2);
        }

        public SetLightPower(ushort level, uint duration) 
            : base(
                  BitConverter.GetBytes(level)
                  .Concat(BitConverter.GetBytes(duration))
                  .ToArray()
              )
        {
            Level = level;
            Duration = duration;
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
                StateLightPower stateLightPower = (StateLightPower)obj;
                return Level == stateLightPower.Level;
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
            return new Type[] { typeof(StateLightPower) };
        }
    }
}
