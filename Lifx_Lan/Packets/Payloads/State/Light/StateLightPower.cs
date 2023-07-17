using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.State.Light
{
    /// <summary>
    /// This says the current power level of the device.
    /// 
    /// This packet is the reply to the GetLightPower (116) and SetLightPower (117) messages
    /// </summary>
    internal class StateLightPower : Payload, IReceivable
    {
        public ushort Level { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateLightPower"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateLightPower"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateLightPower(byte[] bytes) : base(bytes)
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
                StateLightPower stateLightPower = (StateLightPower)obj;
                return Level == stateLightPower.Level;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level);
        }
    }
}
