using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lifx_Lan
{
    internal class StateVersion
    {
        public UInt32 Vendor { get; } = 0;

        public UInt32 Product { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[4];

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
