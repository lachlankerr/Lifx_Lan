using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This says the current brightness of the infrared output from the device
    /// 
    /// This packet requires the device has the infrared capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetInfrared (120) and SetInfrared (122) messages
    /// </summary>
    internal class StateInfrared : Payload, IReceivable
    {
        /// <summary>
        /// The amount of infrared. 
        /// 0 is no infrared output and 65535 is full infrared output.
        /// </summary>
        public ushort Brightness { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateInfrared"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateInfrared"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateInfrared(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 2)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 2");

            Brightness = BitConverter.ToUInt16(bytes, 0);
        }

        public override string ToString()
        {
            return $@"Brightness: {Brightness}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateInfrared stateInfrared = (StateInfrared)obj;
                return Brightness == stateInfrared.Brightness;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Brightness);
        }
    }
}
