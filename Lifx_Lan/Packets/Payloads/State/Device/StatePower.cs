using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.Device
{
    /// <summary>
    /// This packet tells us the current power level of the device. 
    /// 0 means off and any other value means on. 
    /// Note that 65535 is full power and during a power transition (i.e. via SetLightPower (117)) the value may be any value between 0 and 65535.
    /// 
    /// This packet is the reply to the GetPower (20) and SetPower (21) messages
    /// </summary>
    internal class StatePower : Payload, IReceivable
    {
        /// <summary>
        /// The current level of the device's power.
        /// </summary>
        public ushort Level { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StatePower"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StatePower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StatePower(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 2)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 2");

            Level = BitConverter.ToUInt16(bytes, 0);
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
                StatePower statePower = (StatePower)obj;
                return Level == statePower.Level;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level);
        }
    }
}
