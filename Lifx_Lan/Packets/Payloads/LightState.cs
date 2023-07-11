using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// The current visual state of the device and it's label
    /// 
    /// This packet is the reply to GetColor (101), SetColor (102), SetWaveform (103) and SetWaveformOptional (119) messages
    /// </summary>
    internal class LightState : Payload, IReceivable
    {
        /// <summary>
        /// The section of the color spectrum that represents the color of your device. 
        /// So for example red is 0, green is 120, etc
        /// Usually hue is a value between 0 and 360
        /// </summary>
        public ushort Hue { get; } = 0;

        /// <summary>
        /// How strong the color is. 
        /// So a zero saturation is completely white, whilst full saturation is the full color
        /// </summary>
        public ushort Saturation { get; } = 0;

        /// <summary>
        /// How bright the color is. 
        /// So zero brightness is the same as the device is off, while full brightness be just that.
        /// </summary>
        public ushort Brightness { get; } = 0;

        /// <summary>
        /// The "temperature" when the device has zero saturation. 
        /// So a higher value is a cooler white (more blue) whereas a lower value is a warmer white (more yellow)
        /// </summary>
        public ushort Kelvin { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[2];

        /// <summary>
        /// The current power level of the device.
        /// </summary>
        public ushort Power { get; } = 0;

        /// <summary>
        /// The current label on the device.
        /// </summary>
        public string Label { get; } = "";

        public byte[] Reserved8 { get; } = new byte[8];

        /// <summary>
        /// Creates an instance of the <see cref="LightState"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="LightState"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public LightState(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 52)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 52");

            Hue = BitConverter.ToUInt16(bytes, 0);              //2
            Saturation = BitConverter.ToUInt16(bytes, 2);       //2
            Brightness = BitConverter.ToUInt16(bytes, 4);       //2
            Kelvin = BitConverter.ToUInt16(bytes, 6);           //2
            Reserved6 = bytes.Skip(8).Take(2).ToArray();        //2
            Power = BitConverter.ToUInt16(bytes, 10);           //2
            Label = Encoding.ASCII.GetString(bytes, 12, 32);    //32
            Reserved8 = bytes.Skip(44).Take(8).ToArray();       //8
        }

        /// <summary>
        /// From a 0-360 value to a 0-65535 value
        /// </summary>
        /// <param name="hue"></param>
        /// <returns></returns>
        public static ushort HueToUint16(int hue)
        {
            return (ushort)Math.Ceiling((double)(0xFFFF * hue) / 360);
        }

        /// <summary>
        /// From a 0-65535 value to a 0-360 value
        /// </summary>
        /// <param name="uint16_hue"></param>
        /// <returns></returns>
        public static int UInt16ToHue(ushort uint16_hue)
        {
            return uint16_hue * 360 / 0xFFFF;
        }

        /// <summary>
        /// From a 0-1 value to a 0-65535 value
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static ushort PercentageToUInt16(float percentage)
        {
            return (ushort)Math.Round(0xFFFF * percentage);
        }

        /// <summary>
        /// From a 0-65535 value to a 0-1 value
        /// </summary>
        /// <param name="uint16_val"></param>
        /// <returns></returns>
        public static float UInt16ToPercentage(ushort uint16_val)
        {
            return (float)Math.Round((float)uint16_val / 0xFFFF, 4);
        }

        public override string ToString()
        {
            /*to test all hue values will work correctly when converting to uint16 and back
            //we don't bother checking the reverse because multiple uint16 values lead to a single hue value, 
            //so we can never get that precision back
            for (int i = 0; i <= 360; i++)
            {
                if (UInt16ToHue(HueToUint16(i)) != i)
                    throw new Exception(i.ToString());
            }//*/

            /*to test all percentage values will work correctly when converting to uint16 and back
            //we don't bother checking the reverse because multiple uint16 values lead to a single percentage value,
            //so we can never get that precision back
            for (int i = 0; i <= 100; i++)
            {
                float f = i / 100.0f;
                //Console.WriteLine(f);
                if (UInt16ToPercentage(PercentageToUInt16(f)) != f)
                    throw new Exception(i.ToString());
            }//*/

            return $@"Hue: {UInt16ToHue(Hue)} ({Hue})
Saturation: {UInt16ToPercentage(Saturation) * 100.0f}% ({Saturation})
Brightness: {UInt16ToPercentage(Brightness) * 100.0f}% ({Brightness})
Kelvin: {Kelvin}
Reserved6: {BitConverter.ToString(Reserved6)}
Power: {Power}
Label: {Label}
Reserved8: {BitConverter.ToString(Reserved8)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                LightState lightState = (LightState)obj;
                return Hue == lightState.Hue &&
                       Saturation == lightState.Saturation &&
                       Brightness == lightState.Brightness &&
                       Kelvin == lightState.Kelvin &&
                       Reserved6.SequenceEqual(lightState.Reserved6) &&
                       Power == lightState.Power &&
                       Label.Equals(lightState.Label) &&
                       Reserved8.SequenceEqual(lightState.Reserved8);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hue, Saturation, Brightness, Kelvin, Reserved6, Power, Label, Reserved8);
        }
    }
}
