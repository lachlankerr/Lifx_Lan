using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// Note: This feature is experimental and potentially subject to change. 
    /// This feature is only supported by a limited number of devices.
    /// This packet shows the current levels of ambient light as detected by the target device, and is emitted by a device in response to a SensorGetAmbientLight (401) message.
    /// </summary>
    internal class SensorStateAmbientLight : Payload, IReceivable
    {
        /// <summary>
        /// The value of the detected light level. 
        /// If a light is turned on at the time the message is sent, the value will be unreliable and potentially maxed out. 
        /// For best results, only send this message to devices that are currently "soft off".
        /// </summary>
        public float Lux { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="SensorStateAmbientLight"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="SensorStateAmbientLight"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public SensorStateAmbientLight(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 4)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 4");

            Lux = BitConverter.ToSingle(bytes, 0);
        }

        public override string ToString()
        {
            return $@"Lux: {Lux}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                SensorStateAmbientLight sensorStateAmbientLight = (SensorStateAmbientLight)obj;
                return Lux == sensorStateAmbientLight.Lux;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Lux);
        }
    }
}
