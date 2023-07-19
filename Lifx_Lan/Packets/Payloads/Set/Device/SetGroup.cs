using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Device
{
    /// <summary>
    /// This packet lets you set the group information on the device.
    /// 
    /// Will return one StateGroup (53) message
    /// </summary>
    internal class SetGroup : Payload, ISendable
    {
        /// <summary>
        /// 16 bytes representing a UUID of the group. 
        /// You should have the same UUID value for each device in this group
        /// </summary>
        public byte[] Group { get; } = new byte[16];

        /// <summary>
        /// The name of the group.
        /// </summary>
        public string Label { get; } = "";

        /// <summary>
        /// The time you updated the group of this device as an epoch in nanoseconds.
        /// </summary>
        public ulong Updated_At { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetGroup"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetGroup"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetGroup(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 56)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 56");

            Group = bytes.Take(16).ToArray();
            Label = Encoding.ASCII.GetString(bytes, 16, 32);
            Updated_At = BitConverter.ToUInt64(bytes, 48);
        }

        public override string ToString()
        {
            return $@"Group: {BitConverter.ToString(Group)}
Label: {Label}
Updated_At: {Updated_At}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetGroup setGroup = (SetGroup)obj;
                return Group.SequenceEqual(setGroup.Group) &&
                       Label.Equals(setGroup.Label) &&
                       Updated_At == setGroup.Updated_At;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Group, Label, Updated_At);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateGroup) };
        }
    }
}
