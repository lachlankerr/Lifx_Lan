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
        public ushort Hue { get; } = 0;

        public ushort Saturation { get; } = 0;

        public ushort Brightness { get; } = 0;

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

        public override string ToString()
        {
            return $@"Hue: {Hue}
Saturation: {Saturation}
Brightness: {Brightness}
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
