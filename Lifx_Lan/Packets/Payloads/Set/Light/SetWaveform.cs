using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.State.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Light
{
    internal class SetWaveform : Payload, ISendable
    {
        public byte Reserved6 { get; } = 0; //dont use Reserved type for single byte reserved, ends up with more work

        public byte Transient { get; } = 0;

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

        /// <summary>
        /// The duration in milliseconds of one cycle
        /// </summary>
        public uint Period { get; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public float Cycles { get; } = 0;

        /// <summary>
        /// The duty cycle percentage is calculated by applying 1 - skew_ratio as a percentage of the cycle duration and changes the time spent on the original color vs the new color
        /// </summary>
        public short Skew_Ratio { get; } = 0;

        /// <summary>
        /// The shape of waveform to use
        /// </summary>
        public Waveform Waveform { get; } = Waveform.SAW;

        public SetWaveform(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 21)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 21");

            Reserved6 = bytes[0];                               //1
            Transient = bytes[1];                               //1
            Hue = BitConverter.ToUInt16(bytes, 2);              //2
            Saturation = BitConverter.ToUInt16(bytes, 4);       //2
            Brightness = BitConverter.ToUInt16(bytes, 6);       //2
            Kelvin = BitConverter.ToUInt16(bytes, 8);           //2
            Period = BitConverter.ToUInt32(bytes, 10);          //4
            Cycles = BitConverter.ToSingle(bytes, 14);          //4
            Skew_Ratio = BitConverter.ToInt16(bytes, 18);       //2
            Waveform = (Waveform)bytes[20];                     //1
        }

        public SetWaveform(byte reserved6, byte transient, ushort hue, ushort saturation, ushort brightness, ushort kelvin, uint period, float cycles, short skew_ratio, Waveform waveform)
            : base(
                  new byte[] { reserved6, transient }
                  .Concat(BitConverter.GetBytes(hue))
                  .Concat(BitConverter.GetBytes(saturation))
                  .Concat(BitConverter.GetBytes(brightness))
                  .Concat(BitConverter.GetBytes(kelvin))
                  .Concat(BitConverter.GetBytes(period))
                  .Concat(BitConverter.GetBytes(cycles))
                  .Concat(BitConverter.GetBytes(skew_ratio))
                  .Concat(new byte[] { (byte)waveform })
                  .ToArray()
              )
        {
            Reserved6 = reserved6;
            Transient = transient;
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
            Kelvin = kelvin;
            Period = period;
            Cycles = cycles;
            Skew_Ratio = skew_ratio;
            Waveform = waveform;
        }

        public override string ToString()
        {
            return $@"Reserved6: {Reserved6}
Transient: {Transient}
Hue: {LightState.UInt16ToHue(Hue)} ({Hue})
Saturation: {LightState.UInt16ToPercentage(Saturation) * 100.0f}% ({Saturation})
Brightness: {LightState.UInt16ToPercentage(Brightness) * 100.0f}% ({Brightness})
Kelvin: {Kelvin}
Period: {Period}
Cycles: {Cycles}
Skew_Ratio: {Skew_Ratio}
Waveform: {Waveform}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SetWaveform setWaveform = (SetWaveform)obj;
                return Reserved6 == setWaveform.Reserved6 &&
                       Transient == setWaveform.Transient &&
                       Hue == setWaveform.Hue &&
                       Saturation == setWaveform.Saturation &&
                       Brightness == setWaveform.Brightness &&
                       Kelvin == setWaveform.Kelvin &&
                       Period == setWaveform.Period &&
                       Cycles == setWaveform.Cycles &&
                       Skew_Ratio == setWaveform.Skew_Ratio &&
                       Waveform == setWaveform.Waveform;
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Reserved6);
            hash.Add(Transient);
            hash.Add(Hue);
            hash.Add(Saturation);
            hash.Add(Brightness);
            hash.Add(Kelvin);
            hash.Add(Period);
            hash.Add(Cycles);
            hash.Add(Skew_Ratio);
            hash.Add(Waveform);
            return hash.ToHashCode();
        }

        public static FeaturesFlags NeededCapabilities()
        {
            return FeaturesFlags.None; //TODO: really? should be FeaturesFlags.Color
        }

        public static Type[] ReturnMessages()
        {
            return new Type[] { typeof(LightState) };
        }
    }
}
