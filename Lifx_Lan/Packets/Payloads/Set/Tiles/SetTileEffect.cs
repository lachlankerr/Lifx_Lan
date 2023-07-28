using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Tiles;
using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Tiles
{
    /// <summary>
    /// This packet will let you start a Firmware Effect on the device.
    /// 
    /// Will return one StateTileEffect (720) message
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class SetTileEffect : Payload, ISendable
    {
        /// <summary>
        /// The length of the Colors array
        /// </summary>
        const int LEN_COLORS = 16;

        public Reserved Reserved8 { get; } = 1;

        public Reserved Reserved9 { get; } = 1;

        /// <summary>
        /// A unique number identifying this effect.
        /// </summary>
        public uint InstanceId { get; } = 0;

        public TileEffectType Type { get; } = TileEffectType.OFF;

        /// <summary>
        /// The time it takes for one cycle of the effect in milliseconds.
        /// </summary>
        public uint Speed { get; } = 0;

        /// <summary>
        /// The time the effect will run for in nanoseconds.
        /// </summary>
        public ulong Duration { get; } = 0;

        public Reserved Reserved6 { get; } = 4;

        public Reserved Reserved7 { get; } = 4;

        /// <summary>
        /// This field is 8 4 byte fields and is currently ignored by all firmware effects.
        /// </summary>
        public byte[] Parameters { get; } = new byte[32];

        /// <summary>
        /// The number of values in palette that you want to use.
        /// </summary>
        public byte Palette_Count { get; } = 0;

        /// <summary>
        /// The HSBK values to be used by the effect. Currently only the MORPH effect uses these values.
        /// </summary>
        public Color[] Palette { get; } = new Color[LEN_COLORS];

        public SetTileEffect(byte[] bytes) : base(bytes)
        {
            if (bytes.Length != 188)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 188");

            Reserved8 = bytes[0];                           //1
            Reserved9 = bytes[1];                           //1
            InstanceId = BitConverter.ToUInt32(bytes, 2);   //4
            Type = (TileEffectType)bytes[6];                //1
            Speed = BitConverter.ToUInt32(bytes, 7);        //4
            Duration = BitConverter.ToUInt64(bytes, 11);    //8
            Reserved6 = bytes[19..(19 + 4)];                //4
            Reserved7 = bytes[23..(23 + 4)];                //4
            Parameters = bytes[27..(27 + 32)];              //32
            Palette_Count = bytes[59];                      //1

            for (int i = 0; i < LEN_COLORS; i++)
            {
                int offset = i * Color.SIZE + 60;
                ushort hue = BitConverter.ToUInt16(bytes, 0 + offset);
                ushort saturation = BitConverter.ToUInt16(bytes, 2 + offset);
                ushort brightness = BitConverter.ToUInt16(bytes, 4 + offset);
                ushort kelvin = BitConverter.ToUInt16(bytes, 6 + offset);
                Palette[i] = new Color(hue, saturation, brightness, kelvin);
            }
        }

        public override string ToString()
        {
            return $@"Reserved8: {Reserved8}
Reserved9: {Reserved9}
InstanceId: {InstanceId}
Type: {Type} ({(byte)Type})
Speed: {Speed}
Duration: {Duration}
Reserved6: {Reserved6}
Reserved7: {Reserved7}
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
                SetTileEffect cast = (SetTileEffect)obj;
                return Reserved8 == cast.Reserved8 &&
                       Reserved9 == cast.Reserved9 &&
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
            hash.Add(Reserved9);
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

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.Matrix;
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(StateTileEffect) };
        }
    }
}
