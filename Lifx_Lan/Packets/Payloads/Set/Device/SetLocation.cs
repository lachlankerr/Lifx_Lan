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
    /// This packet lets you set the location information on the device.
    /// 
    /// Will return one StateLocation (50) message
    /// </summary>
    internal class SetLocation : Payload, ISendable
    {
        /// <summary>
        /// 16 bytes representing a UUID of the location. 
        /// You should have the same UUID value for each device in this location
        /// </summary>
        public byte[] Location { get; } = new byte[16];

        /// <summary>
        /// The name of the location.
        /// </summary>
        public string Label { get; } = "";

        /// <summary>
        /// The time you updated the location of this device as an epoch in nanoseconds.
        /// </summary>
        public ulong Updated_At { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SetLocation"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetLocation"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetLocation(byte[] bytes) : base(bytes)
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
                SetLocation setLocation = (SetLocation)obj;
                return Location.SequenceEqual(setLocation.Location) &&
                       Label.Equals(setLocation.Label) &&
                       Updated_At == setLocation.Updated_At;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, Label, Updated_At);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateLocation) };
        }
    }
}
