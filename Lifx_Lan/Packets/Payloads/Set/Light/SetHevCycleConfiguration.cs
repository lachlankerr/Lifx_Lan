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
    /// This packet lets you set default values for a HEV cycle on the device
    /// 
    /// Will return one StateHevCycleConfiguration (147) message
    /// 
    /// This packet requires the device has the hev capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetHevCycleConfiguration : Payload, ISendable
    {
        /// <summary>
        /// Set this to true to run a short flashing indication at the end of the HEV cycle
        /// </summary>
        public BoolInt Indication { get; } = 0;

        /// <summary>
        /// This is the default duration that is used when SetHevCycle (143) is given 0 for duration_s.
        /// </summary>
        public uint Duration_S { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetHevCycleConfiguration"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetHevCycleConfiguration"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetHevCycleConfiguration(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 5)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 5");

            Indication = bytes[0];
            Duration_S = BitConverter.ToUInt32(bytes, 1);
        }

        public SetHevCycleConfiguration(BoolInt indication, uint duration_s)
            : base(
                  new byte[] { (byte)indication }
                  .Concat(BitConverter.GetBytes(duration_s))
                  .ToArray()
              )
        {
            Indication = indication;
            Duration_S = duration_s;
        }

        public override string ToString()
        {
            return $@"Indication: {Indication}
Duration_S: {Duration_S}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetHevCycleConfiguration setHevCycleConfiguration = (SetHevCycleConfiguration)obj;
                return Indication == setHevCycleConfiguration.Indication &&
                       Duration_S == setHevCycleConfiguration.Duration_S;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Indication, Duration_S);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Hev;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateHevCycleConfiguration) };
        }
    }
}
