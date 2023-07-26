using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.MultiZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.MultiZone
{
    /// <summary>
    /// Start a multizone Firmware Effect on the device.
    /// 
    /// Will return one StateMultiZoneEffect (509) message
    /// 
    /// This packet requires the device has the Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetMultiZoneEffect : Payload, ISendable
    {
        /// <summary>
        /// A unique value identifying this effect
        /// </summary>
        public uint InstanceId { get; } = 0;

        public MultiZoneEffectType Type { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[2];

        /// <summary>
        /// The time it takes for one cycle of the effect in milliseconds
        /// </summary>
        public uint Speed { get; } = 0;

        /// <summary>
        /// The time the effect will run for in nanoseconds.
        /// </summary>
        public ulong Duration { get; } = 0;

        public byte[] Reserved7 { get; } = new byte[4];

        public byte[] Reserved8 { get; } = new byte[4];

        /// <summary>
        /// This field is 8 4 byte fields which change meaning based on the effect that is running. 
        /// When the effect is MOVE only the second field is used and is a Uint32 representing the DIRECTION enum. 
        /// This field is currently ignored for all other multizone effects.
        /// </summary>
        public byte[] Parameters { get; } = new byte[32];

        /// <summary>
        /// Creates an instance of the <see cref="SetMultiZoneEffect"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetMultiZoneEffect"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetMultiZoneEffect(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 59)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 59");

            InstanceId = BitConverter.ToUInt32(bytes, 0);   //4
            Type = (MultiZoneEffectType)bytes[4];           //1
            Reserved6 = bytes.Skip(5).Take(2).ToArray();    //2
            Speed = BitConverter.ToUInt32(bytes, 7);        //4
            Duration = BitConverter.ToUInt64(bytes, 11);    //8
            Reserved7 = bytes.Skip(19).Take(4).ToArray();   //4
            Reserved8 = bytes.Skip(23).Take(4).ToArray();   //4
            Parameters = bytes.Skip(27).Take(32).ToArray(); //32
        }

        public SetMultiZoneEffect(uint instanceid, MultiZoneEffectType type, byte[] reserved6, uint speed, ulong duration, byte[] reserved7, byte[] reserved8, byte[] parameters)
            : base(
                  BitConverter.GetBytes(instanceid)
                  .Concat(new byte[] { (byte)type })
                  .Concat(reserved6)
                  .Concat(BitConverter.GetBytes(speed))
                  .Concat(BitConverter.GetBytes(duration))
                  .Concat(reserved7)
                  .Concat(reserved8)
                  .Concat(parameters)
                  .ToArray()
              )
        {
            InstanceId = instanceid;
            Type = type;
            Reserved6 = reserved6;
            Speed = speed;
            Duration = duration;
            Reserved7 = reserved7;
            Reserved8 = reserved8;
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $@"InstanceId: {InstanceId}
Type: {Type} ({(byte)Type})
Reserved6: {BitConverter.ToString(Reserved6)}
Speed: {Speed}
Duration: {Duration}
Reserved7: {BitConverter.ToString(Reserved7)}
Reserved8: {BitConverter.ToString(Reserved8)}
Parameters: {BitConverter.ToString(Parameters)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetMultiZoneEffect setMultiZoneEffect = (SetMultiZoneEffect)obj;
                return InstanceId == setMultiZoneEffect.InstanceId &&
                       Type == setMultiZoneEffect.Type &&
                       Reserved6.SequenceEqual(setMultiZoneEffect.Reserved6) &&
                       Speed == setMultiZoneEffect.Speed &&
                       Duration == setMultiZoneEffect.Duration &&
                       Reserved7.SequenceEqual(setMultiZoneEffect.Reserved7) &&
                       Reserved8.SequenceEqual(setMultiZoneEffect.Reserved8);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InstanceId, Type, Reserved6, Speed, Duration, Reserved7, Reserved8, Parameters);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Multizone;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateMultiZoneEffect) };
        }
    }
}
