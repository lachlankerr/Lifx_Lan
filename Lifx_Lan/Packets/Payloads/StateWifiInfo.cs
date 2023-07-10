using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet will give you information about the signal strength of the device.
    /// 
    /// This packet is the reply to the GetWifiInfo (16) message
    /// </summary>
    internal class StateWifiInfo
    {
        /// <summary>
        /// The signal strength of the device.
        /// </summary>
        public float Signal { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[4];

        public byte[] Reserved7 { get; } = new byte[4];

        public byte[] Reserved8 { get; } = new byte[2];

        /// <summary>
        /// Creates an instance of the <see cref="StateWifiInfo"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateWifiInfo"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateWifiInfo(byte[] bytes) 
        {
            if (bytes.Length != 14)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 14");

            Signal = BitConverter.ToSingle(bytes, 0);
            Reserved6 = bytes.Skip(4).Take(4).ToArray();
            Reserved7 = bytes.Skip(8).Take(4).ToArray();
            Reserved8 = bytes.Skip(12).Take(2).ToArray();
        }

        /// <summary>
        /// The units of this field varies between different products, 
        /// You can use the following pseudo code to determine the signal strength of your device.
        /// </summary>
        /// <returns></returns>
        public string GetSignalStrength()
        {
            int rssi = Convert.ToInt32(Math.Floor(10 * Math.Log10(Signal) + 0.5));
            string msg;

            if (rssi < 0 || rssi == 200)
            {
                if (rssi == 200)
                    msg = "No signal";
                else if (rssi <= -80)
                    msg = "Very bag signal";
                else if (rssi <= -70)
                    msg = "Somewhat bad signal";
                else if (rssi <= -60)
                    msg = "Alright signal";
                else
                    msg = "Good signal";
                return $"{msg}, rssi: {rssi}, raw: {Signal}";
            }

            if (rssi == 4 || rssi == 5 || rssi == 6)
                msg = "Very bad signal";
            else if (rssi >= 7 && rssi <= 11)
                msg = "Somewhat bad signal";
            else if (rssi >= 12 && rssi <= 16)
                msg = "Alright signal";
            else if ( rssi > 16)
                msg = "Good signal";
            else
                msg = "No signal";

            return $"{msg}, rssi: {rssi}, raw: {Signal}";
        }

        public override string ToString()
        {
            return $@"Signal: {GetSignalStrength()}
Reserved6: {BitConverter.ToString(Reserved6)}
Reserved7: {BitConverter.ToString(Reserved7)}
Reserved8: {BitConverter.ToString(Reserved8)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateWifiInfo stateWifiInfo = (StateWifiInfo)obj;
                return Signal == stateWifiInfo.Signal &&
                       Reserved6.SequenceEqual(stateWifiInfo.Reserved6) &&
                       Reserved7.SequenceEqual(stateWifiInfo.Reserved7) &&
                       Reserved8.SequenceEqual(stateWifiInfo.Reserved8);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Signal, Reserved6, Reserved7, Reserved8);
        }
    }
}
