using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lifx_Lan
{
    /// <summary>
    /// This packet tell us the version of the firmware on the device. 
    /// This information can be used with our Product Registry to determine what capabilities are supported by the device.
    /// 
    /// This packet is the reply to the GetVersion (32) message.
    /// </summary>
    internal class StateVersion
    {
        /// <summary>
        /// For LIFX products this value is 1. 
        /// There may be devices in the future with a different vendor value.
        /// </summary>
        public UInt32 Vendor { get; } = 0;

        /// <summary>
        /// The product id of the device. 
        /// The available products can be found in our Product Registry.
        /// </summary>
        public UInt32 Product { get; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved6 { get; } = new byte[4];

        /// <summary>
        /// Creates an instance of the <see cref="StateVersion"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateVersion"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateVersion(byte[] bytes)
        {
            if (bytes.Length != 12)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 12");

            Vendor = BitConverter.ToUInt32(bytes, 0);
            Product = BitConverter.ToUInt32(bytes, 4);
            Reserved6 = bytes.Skip(8).Take(4).ToArray();
        }

        public override string ToString()
        {
            return $@"Vendor: {Vendor}
Product: {Product}
Reserved6: {BitConverter.ToString(Reserved6)}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateVersion stateVersion = (StateVersion)obj;
                return this.Vendor == stateVersion.Vendor &&
                       this.Product == stateVersion.Product &&
                       this.Reserved6.SequenceEqual(stateVersion.Reserved6);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Vendor, Product, Reserved6);
        }
    }
}
