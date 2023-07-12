using Lifx_Lan.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet tells us what Firmware Effect is current running on the device.
    /// 
    /// This packet requires the device has the Linear Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetMultiZoneEffect (507) and SetMultiZoneEffect (508) messages
    /// </summary>
    internal class StateMultiZoneEffect : Payload, IReceivable
    {
        /// <summary>
        /// The unique value identifying this effect
        /// </summary>
        public uint Instance_Id { get; } = 0;

        public MultiZoneEffectType Type { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[2];

        /// <summary>
        /// The time it takes for one cycle of the effect in milliseconds
        /// </summary>
        public uint Speed { get; } = 0;

        /// <summary>
        /// The amount of time left in the current effect in nanoseconds
        /// </summary>
        public ulong Duration { get; } = 0;

        public byte[] Reserved7 { get; } = new byte[4];

        public byte[] Reserved8 { get; } = new byte[4];

        /// <summary>
        /// The parameters that were used in the request.
        /// </summary>
        public byte[] Parameters { get; } = new byte[32];

        /// <summary>
        /// Creates an instance of the <see cref="StateMultiZoneEffect"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateMultiZoneEffect"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateMultiZoneEffect(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 59)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 59");

            Instance_Id = BitConverter.ToUInt32(bytes, 0);  //4
            Type = (MultiZoneEffectType)bytes[4];           //1
            Reserved6 = bytes.Skip(5).Take(2).ToArray();    //2
            Speed = BitConverter.ToUInt32(bytes, 7);        //4
            Duration = BitConverter.ToUInt64(bytes, 11);    //8
            Reserved7 = bytes.Skip(19).Take(4).ToArray();   //4
            Reserved8 = bytes.Skip(23).Take(4).ToArray();   //4
            Parameters = bytes.Skip(27).Take(32).ToArray(); //32
        }

        public override string ToString()
        {
            return $@"Instance_Id: {Instance_Id}
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
                StateMultiZoneEffect stateMultiZoneEffect = (StateMultiZoneEffect)obj;
                return Instance_Id == stateMultiZoneEffect.Instance_Id &&
                       Type == stateMultiZoneEffect.Type &&
                       Reserved6.SequenceEqual(stateMultiZoneEffect.Reserved6) &&
                       Speed == stateMultiZoneEffect.Speed && 
                       Duration == stateMultiZoneEffect.Duration &&
                       Reserved7.SequenceEqual(stateMultiZoneEffect.Reserved7) &&
                       Reserved8.SequenceEqual(stateMultiZoneEffect.Reserved8);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Instance_Id, Type, Reserved6, Speed, Duration, Reserved7, Reserved8, Parameters);
        }
    }
}
