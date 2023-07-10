using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet provides the details of the location set on the device.
    /// 
    /// To determine the label of a location you need to send a GetLocation (48) to all the devices you can find and for each location uuid determine which label is accompanied by the latest updated_at value.
    /// 
    /// This packet is the reply to the GetLocation (48) and SetLocation (49) messages
    /// </summary>
    internal class StateLocation : Payload, IReceivable
    {
        /// <summary>
        /// The unique identifier of this location as a uuid.
        /// </summary>
        public byte[] Location { get; } = new byte[16];

        /// <summary>
        /// The name assigned to this location
        /// </summary>
        public string Label { get; } = "";

        /// <summary>
        /// An epoch in nanoseconds of when this location was set on the device
        /// </summary>
        public ulong Updated_At { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateLocation"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateLocation"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateLocation(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 56)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 56");

            Location = bytes.Take(16).ToArray();
            Label = Encoding.ASCII.GetString(bytes, 16, 32);
            Updated_At = BitConverter.ToUInt64(bytes, 48);
        }

        public override string ToString()
        {
            return $@"Location: {BitConverter.ToString(Location)}
Label: {Label}
Updated_At: {Updated_At}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateLocation stateLocation = (StateLocation)obj;
                return Location.SequenceEqual(stateLocation.Location) &&
                       Label.Equals(stateLocation.Label) &&
                       Updated_At == stateLocation.Updated_At;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, Label, Updated_At);
        }
    }
}
