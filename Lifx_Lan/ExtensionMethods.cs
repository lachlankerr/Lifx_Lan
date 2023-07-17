using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Gets the size of the object in bytes, 
        /// null returns 0, 
        /// undefined types throw a <see cref="NotSupportedException"/>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">For any undefined types</exception>
        public static int Size(this object o)
        {
            if (o == null)
                return 0;

            Type t = o.GetType();

            if (o.GetType().IsArray)
            {
                t = o.GetType().GetElementType()!;
                Array arr = (o as Array)!;
                return arr.Length * CalcSize(t);
            }
            else if (t == typeof(string)) //string arrays will have issues
                return Encoding.Unicode.GetByteCount((string)o);

            return CalcSize(t);
        }

        public static int CalcSize(this Type t)
        {
            if (t == null)
                return 0;

            //primitives
            else if (t == typeof(sbyte))
                return sizeof(sbyte);
            else if (t == typeof(byte))
                return sizeof(byte);
            else if (t == typeof(short))
                return sizeof(short);
            else if (t == typeof(ushort))
                return sizeof(ushort);
            else if (t == typeof(int))
                return sizeof(int);
            else if (t == typeof(uint))
                return sizeof(uint);
            else if (t == typeof(long))
                return sizeof(long);
            else if (t == typeof(ulong))
                return sizeof(ulong);
            else if (t == typeof(char))
                return sizeof(char);
            else if (t == typeof(float))
                return sizeof(float);
            else if (t == typeof(double))
                return sizeof(double);
            else if (t == typeof(decimal))
                return sizeof(decimal);
            else if (t == typeof(bool))
                return sizeof(bool);

            //structs
            else if (t == typeof(Color))
                return Color.SIZE;
            else if (t == typeof(Tile))
                return Tile.SIZE;

            //enums
            else if (t == typeof(Services))
                return sizeof(Services);
            else if (t == typeof(Direction))
                return sizeof(Direction);
            else if (t == typeof(LightLastHevCycleResult))
                return sizeof(LightLastHevCycleResult);
            else if (t == typeof(MultiZoneApplicationRequest))
                return sizeof(MultiZoneApplicationRequest);
            else if (t == typeof(MultiZoneEffectType))
                return sizeof(MultiZoneEffectType);
            else if (t == typeof(MultiZoneExtendedApplicationRequest))
                return sizeof(MultiZoneExtendedApplicationRequest);
            else if (t == typeof(TileEffectType))
                return sizeof(TileEffectType);
            else if (t == typeof(Waveform))
                return sizeof(Waveform);

            throw new NotSupportedException($"The type {t.FullName} has not been defined for CalcSize()");
        }
    }
}
