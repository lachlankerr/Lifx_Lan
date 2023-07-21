using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal readonly struct BoolInt : IEquatable<BoolInt>
    {
        private readonly byte value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Value == False : 0, Value == True : 1</param>
        public BoolInt(bool value)
        {
            thisValue = (byte)(value ? 1 : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Value == 0 : False, Value > 0 : True</param>
        public BoolInt(byte value)
        {
            Value = value;
        }

        public override bool Equals(object? obj) => Equals(obj as BoolInt);

        public override int GetHashCode() => Value.GetHashCode();

        public bool Equals(BoolInt other)
        {
            if (other is null) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            if (GetType() != other.GetType()) 
                return false;

            return Value == other.Value;
        }

        public static bool operator ==(BoolInt? left, BoolInt? right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return left.Equals(right);
        }

        public static bool operator !=(BoolInt? left, BoolInt? right) => !(left == right);
    }
}
