using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal readonly struct Reserved : IEquatable<Reserved>, IEqualityComparer<Reserved>, IEnumerable<byte>//, IEnumerable //not needed ?
    {
        /// <summary>
        /// The internal reserved byte array of this struct
        /// </summary>
        private readonly byte[] reservedBytes = Array.Empty<byte>();

        /// <summary>
        /// Creates an instance of the <see cref="Reserved"/> struct from a <see cref="byte"/> array
        /// </summary>
        /// <param name="reservedBytes">The bytes we wish to set</param>
        public Reserved(byte[] reservedBytes)
        {
            this.reservedBytes = reservedBytes;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Reserved"/> struct with an empty <see cref="byte"/> array of specified size
        /// </summary>
        /// <param name="size">The size of the empty <see cref="byte"/> array</param>
        public Reserved(byte size)
        {
            reservedBytes = new byte[size];
        }

        public override string ToString()
        {
            return BitConverter.ToString(reservedBytes);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Reserved reserved = (Reserved)obj;
                return reservedBytes.SequenceEqual(reserved.reservedBytes);
            }
        }

        public override int GetHashCode()
        {
            return reservedBytes.GetHashCode();
        }

        public bool Equals(Reserved other)
        {
            return reservedBytes.SequenceEqual(other.reservedBytes);
        }

        public bool Equals(Reserved x, Reserved y)
        {
            if (x == null || y == null) 
                return false;

            return x.reservedBytes == y.reservedBytes;
        }

        public int GetHashCode([DisallowNull] Reserved obj)
        {
            return obj.GetHashCode();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (byte b in reservedBytes)
            {
                yield return b;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(Reserved? left, Reserved? right)
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

        public static bool operator !=(Reserved? left, Reserved? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Allows an implicit cast from <see cref="byte"/> array to <see cref="Reserved"/>
        /// </summary>
        /// <param name="boolInt"></param>
        public static implicit operator Reserved(byte[] value)
        {
            return new Reserved(value);
        }

        /// <summary>
        /// Allows an implicit cast from <see cref="Reserved"/> to <see cref="byte"/> array
        /// </summary>
        /// <param name="boolInt"></param>
        public static implicit operator byte[](Reserved value)
        {
            return value.reservedBytes;
        }

        /// <summary>
        /// Allows an implicit cast from <see cref="byte"/> to <see cref="Reserved"/>
        /// </summary>
        /// <param name="boolInt"></param>
        public static implicit operator Reserved(byte value)
        {
            return new Reserved(value);
        }
    }
}
