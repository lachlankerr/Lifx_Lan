using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// The current Firmware Effect running on the device
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetTileEffect (718) and SetTileEffect (719) messages
    /// </summary>
    internal class StateTileEffect : Payload, IReceivable
    {
        public readonly int Size;

        /// <summary>
        /// The length of the Colors array
        /// </summary>
        const int LEN_COLORS = 16;

        public byte Reserved8 { get; } = 0;

        /// <summary>
        /// The unique value identifying the request
        /// </summary>
        public uint InstanceId { get; } = 0;

        public TileEffectType Type { get; } = 0;

        /// <summary>
        /// The time it takes for one cycle in milliseconds.
        /// </summary>
        public uint Speed { get; } = 0;

        /// <summary>
        /// The amount of time left in the current effect in nanoseconds
        /// </summary>
        public ulong Duration { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[4];
        
        public byte[] Reserved7 { get; } = new byte[4];

        /// <summary>
        /// The parameters as specified in the request.
        /// </summary>
        public byte[] Parameters { get; } = new byte[32];

        /// <summary>
        /// The number of colors in the palette that are relevant
        /// </summary>
        public byte Palette_Count { get; } = 0;

        /// <summary>
        /// The colors specified for the effect.
        /// </summary>
        public Color[] Palette { get; } = new Color[LEN_COLORS];

        /// <summary>
        /// Creates an instance of the <see cref="StateTileEffect"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateTileEffect"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateTileEffect(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 187)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 187");

            Reserved8 = bytes[0];                           //1
            InstanceId = BitConverter.ToUInt32(bytes, 1);   //4
            Type = (TileEffectType)bytes[5];                //1
            Speed = BitConverter.ToUInt32(bytes, 6);        //4
            Duration = BitConverter.ToUInt64(bytes, 10);    //8
            Reserved6 = bytes[18..(18 + 4)];                //4
            Reserved7 = bytes[22..(22 + 4)];                //4
            Parameters = bytes[26..(26 + 32)];              //32
            Palette_Count = bytes[58];                      //1

            for (int i = 0; i < LEN_COLORS; i++)
            {
                int offset = i * Color.SIZE + 59;
                ushort hue = BitConverter.ToUInt16(bytes, 0 + offset);
                ushort saturation = BitConverter.ToUInt16(bytes, 2 + offset);
                ushort brightness = BitConverter.ToUInt16(bytes, 4 + offset);
                ushort kelvin = BitConverter.ToUInt16(bytes, 6 + offset);
                Palette[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        public int CalcSize()
        {
            PropertyInfo[] properties = GetType().GetProperties();
            int size = 0;
            foreach (PropertyInfo propertyInfo in properties)
                if (propertyInfo.DeclaringType == GetType())
                    size += propertyInfo.GetValue(this)!.Size();
            return size;
        }

        /// <summary>
        /// Creates an instance of the <see cref="StateTileEffect"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateTileEffect"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateTileEffect(byte[] bytes, bool unused) : base(bytes)
        {
            Size = CalcSize();
            if (bytes.Length != Size)
                throw new ArgumentException($"Wrong number of bytes for this payload type, expected {Size}");

            Reserved8 = Fill(Reserved8);
            InstanceId = Fill(InstanceId);
            Type = Fill(Type);
            Speed = Fill(Speed);
            Duration = Fill(Duration);
            Reserved6 = Fill(Reserved6);
            Reserved7 = Fill(Reserved7);
            Parameters = Fill(Parameters);
            Palette_Count = Fill(Palette_Count);

            for (int i = 0; i < LEN_COLORS; i++)
            {
                Palette[i] = new Color(0, 0, 0, 0); //wasted
                ushort hue = Fill(Palette[i].Hue);
                ushort saturation = Fill(Palette[i].Saturation);
                ushort brightness = Fill(Palette[i].Brightness);
                ushort kelvin = Fill(Palette[i].Kelvin);
                Palette[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        int Offset = 0;

        public T Fill<T>(T property)
        {
            object returnValue;
            Type type = typeof(T);
            int size = property!.Size();
            //Console.WriteLine(size);

            if (type == typeof(byte))
                returnValue = Bytes[Offset];
            else if (type == typeof(TileEffectType))
                returnValue = (TileEffectType)Bytes[Offset];
            else if (type == typeof(ushort))
                returnValue = BitConverter.ToUInt16(Bytes, Offset);
            else if (type == typeof(uint))
                returnValue = BitConverter.ToUInt32(Bytes, Offset);
            else if (type == typeof(ulong))
                returnValue = BitConverter.ToUInt64(Bytes, Offset);
            else if (type == typeof(byte[]))
                returnValue = Bytes[Offset..(Offset + size)];
            else
                throw new NotSupportedException($"The type {type.FullName} has not been defined for Fill()");

            Offset += size;
            return (T)returnValue;
        }

        public override string ToString()
        {
            return $@"Reserved8: {Reserved8}
InstanceId: {InstanceId}
Type: {Type} ({(byte)Type})
Speed: {Speed}
Duration: {Duration}
Reserved6: {BitConverter.ToString(Reserved6)}
Reserved7: {BitConverter.ToString(Reserved7)}
Parameters: {BitConverter.ToString(Parameters)}
Palette_Count: {Palette_Count}
Palette: 
{string.Join($"\n\n", Palette.ToList())}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateTileEffect cast = (StateTileEffect)obj;
                return Reserved8 == cast.Reserved8 &&
                       InstanceId == cast.InstanceId &&
                       Type == cast.Type &&
                       Speed == cast.Speed &&
                       Duration == cast.Duration &&
                       Reserved6.SequenceEqual(cast.Reserved6) &&
                       Reserved7.SequenceEqual(cast.Reserved7) &&
                       Parameters.SequenceEqual(cast.Parameters) &&
                       Palette_Count == cast.Palette_Count &&
                       Palette.SequenceEqual(cast.Palette);
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Reserved8);
            hash.Add(InstanceId);
            hash.Add(Type);
            hash.Add(Speed);
            hash.Add(Duration);
            hash.Add(Reserved6);
            hash.Add(Reserved7);
            hash.Add(Parameters);
            hash.Add(Palette_Count);
            hash.Add(Palette);
            return hash.ToHashCode();
        }
    }
}
