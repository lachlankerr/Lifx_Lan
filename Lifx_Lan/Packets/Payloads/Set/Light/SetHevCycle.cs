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
    /// This packet lets you start or stop a HEV cycle on the device.
    /// 
    /// Will return one StateHevCycle (144) message
    /// 
    /// This packet requires the device has the hev capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetHevCycle : Payload, ISendable
    {
        /// <summary>
        /// Set this to false to turn off the cycle and true to start the cycle
        /// </summary>
        public BoolInt Enable { get; } = false;

        /// <summary>
        /// The duration, in seconds that the cycle should last for. 
        /// A value of 0 will use the default duration set by SetHevCycleConfiguration (146).
        /// </summary>
        public uint Duration_S { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetHevCycle"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetHevCycle"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetHevCycle(byte[] bytes) : base (bytes)
        {
            if (bytes.Length != 5)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 5");

            Enable = bytes[0];
            Duration_S = BitConverter.ToUInt32(bytes, 1);
        }

        public SetHevCycle(BoolInt enable, uint duration_s)
            : base(
                  new byte[] { (byte)enable }
                  .Concat(BitConverter.GetBytes(duration_s))
                  .ToArray()
              )
        {
            Enable = enable;
            Duration_S = duration_s;
        }

        public override string ToString()
        {
            return $@"Enable: {Enable}
Duration_S: {Duration_S}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetHevCycle setHevCycle = (SetHevCycle)obj;
                return Enable == setHevCycle.Enable &&
                       Duration_S == setHevCycle.Duration_S;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Enable, Duration_S);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Hev;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateHevCycle) };
        }
    }
}
