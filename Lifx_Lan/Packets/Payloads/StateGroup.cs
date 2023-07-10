using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet provides the details of the group set on the device.
    /// 
    /// To determine the label of a group you need to send a GetGroup (51) to all the devices you can find and for each group uuid determine which label is accompanied by the latest updated_at value.
    /// 
    /// This packet is the reply to the GetGroup (51) and SetGroup (52) messages
    /// </summary>
    internal class StateGroup : Payload, IReceivable
    {
        /// <summary>
        /// The unique identifier of this group as a uuid.
        /// </summary>
        public byte[] Group { get; } = new byte[16];

        /// <summary>
        /// The name assigned to this group
        /// </summary>
        public string Label { get; } = "";

        /// <summary>
        /// An epoch in nanoseconds of when this group was set on the device
        /// </summary>
        public ulong Updated_At { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateGroup"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateGroup"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateGroup(byte[] bytes) : base(bytes)
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
                StateGroup stateGroup = (StateGroup)obj;
                return Group.SequenceEqual(stateGroup.Group) &&
                       Label.Equals(stateGroup.Label) &&
                       Updated_At == stateGroup.Updated_At;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Group, Label, Updated_At);
        }
    }
}
