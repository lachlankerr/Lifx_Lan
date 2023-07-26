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
    /// This packet lets you set the label on the device. 
    /// The label is a string you assign to the device and will be displayed as the name of the device in the LIFX mobile apps.
    /// </summary>
    internal class SetLabel : Payload, ISendable
    {
        /// <summary>
        /// The string you want to assign.
        /// </summary>
        public string Label { get; } = "";

        /// <summary>
        /// Creates an instance of the <see cref="SetLabel"/> class so we can specify the payload values to send
        /// </summary>
        /// <param name="bytes">The payload data we will send with the <see cref="SetLabel"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SetLabel(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 32)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 32");

            Label = Encoding.ASCII.GetString(bytes);
        }

        public SetLabel(string label) 
            : base( 
                  Encoding.ASCII.GetBytes(label)
              )
        {
            Label = label;
        }

        public override string ToString()
        {
            return $@"Lable: {Label}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetLabel setLabel = (SetLabel)obj;
                return Label.Equals(setLabel.Label);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Label);
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateLabel) };
        }
    }
}
