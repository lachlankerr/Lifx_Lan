using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet tells us the label of the device.
    /// 
    /// This packet is the reply to the GetLabel (23) and SetLabel (24) messages
    /// </summary>
    internal class StateLabel
    {
        /// <summary>
        /// The label of the device
        /// </summary>
        public string Label { get; } = "";

        /// <summary>
        /// Creates an instance of the <see cref="StateLabel"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateLabel"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateLabel(byte[] bytes)
        {
            if (bytes.Length != 32)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 32");

            Label = Encoding.ASCII.GetString(bytes);
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
                StateLabel stateLabel = (StateLabel)obj;
                return Label.Equals(stateLabel.Label);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Label);
        }
    }
}
