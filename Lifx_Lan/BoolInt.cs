using Lifx_Lan.Packets.Payloads.Set.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// A 1 byte integer where 0 represents false and 1 represents true
    /// </summary>
    internal readonly struct BoolInt : IEquatable<BoolInt>
    {
        /// <summary>
        /// The internal value of this struct
        /// </summary>
        private readonly byte value;

        /// <summary>
        /// Creates an instance of the <see cref="BoolInt"/> struct from a <see cref="bool"/> value
        /// </summary>
        /// <param name="value">value == False : 0, value == True : 1</param>
        public BoolInt(bool value)
        {
            this.value = (byte)(value ? 1 : 0);
        }

        /// <summary>
        /// Creates an instance of the <see cref="BoolInt"/> struct from a <see cref="byte"/> value, truncating it to 0 or 1 only
        /// </summary>
        /// <param name="value">value == 0 : False, value > 0 : True</param>
        public BoolInt(byte value)
        {
            this.value = (byte)(value > 1 ? 1 : value);
        }

        /// <summary>
        /// Returns the value as a <see cref="bool"/>
        /// </summary>
        /// <returns></returns>
        public bool Boolean()
        {
            return value != 0;
        }

        /// <summary>
        /// Returns the value as a <see cref="byte"/>, either 0 or 1
        /// </summary>
        /// <returns></returns>
        public byte Byte()
        {
            return value;
        }

        public override string ToString()
        {
            return Boolean().ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                BoolInt boolInt = (BoolInt)obj;
                return value == boolInt.value;
            }
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public bool Equals(BoolInt other)
        {
            return value == other.value;
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

        public static bool operator !=(BoolInt? left, BoolInt? right)
        { 
            return !(left == right);
        }

        /// <summary>
        /// Allows an implicit cast from <see cref="byte"/> to <see cref="BoolInt"/>
        /// </summary>
        /// <param name="boolInt"></param>
        public static implicit operator BoolInt(byte value)
        {
            return new BoolInt(value);
        }

        /// <summary>
        /// Allows a cast from <see cref="BoolInt"/> to <see cref="byte"/>
        /// </summary>
        /// <param name="boolInt"></param>
        public static explicit operator byte(BoolInt boolInt)
        {
            return boolInt.value;
        }

        /// <summary>
        /// Allows an implicit cast from <see cref="bool"/> to <see cref="BoolInt"/>
        /// </summary>
        /// <param name="boolInt"></param>
        public static implicit operator BoolInt(bool value)
        {
            return new BoolInt(value);
        }

        /// <summary>
        /// Allows an implicit cast from <see cref="BoolInt"/> to <see cref="bool"/>
        /// </summary>
        /// <param name="boolInt"></param>
        public static implicit operator bool(BoolInt boolInt)
        {
            return boolInt.Boolean();
        }

        /*public static BoolInt operator +(BoolInt left, BoolInt right)
        {
            return new BoolInt((byte)(left.value + right.value));
        }

        public static BoolInt operator -(BoolInt left, BoolInt right)
        {
            return new BoolInt((byte)(left.value - right.value));
        }*/
    }
}
