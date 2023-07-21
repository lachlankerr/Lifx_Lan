using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Light
{
    /// <summary>
    /// This packet lets you change the current infrared value on the device
    /// 
    /// Will return one StateInfrared (121) message
    /// 
    /// This packet requires the device has the infrared capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetInfrared : Payload, ISendable
    {
        /// <summary>
        /// The amount of infrared emitted by the device. 
        /// 0 is no infrared and 65535 is the most infrared.
        /// </summary>
        public ushort Brightness { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetInfrared"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetInfrared"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetInfrared(byte[] bytes) : base(bytes)
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
                SetInfrared setInfrared = (SetInfrared)obj;
                return Brightness == setInfrared.Brightness;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Brightness);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Infrared;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateInfrared) };
        }
    }
}
