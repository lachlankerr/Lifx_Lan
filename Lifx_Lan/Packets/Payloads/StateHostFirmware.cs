using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet will tell you what version of firmware is on the device.
    /// 
    /// Typically you would use this information along with StateVersion (33) to determine the capabilities of your device as specified in our Product Registry.
    /// 
    /// The version_major and version_minor should be thought of as a pair of (major, minor). 
    /// So say major is 3 and minor is 60, then the version is (3, 60). 
    /// This means that (2, 80) is considered less than (3, 60) and (3, 70) is considered greater.
    /// 
    /// LIFX products will specify a different major for each generation of our devices.
    /// 
    /// This packet is the reply to the GetHostFirmware (14) message
    /// </summary>
    internal class StateHostFirmware
    {
        /// <summary>
        /// The timestamp of the firmware that is on the device as an epoch
        /// </summary>
        public ulong Build { get; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved6 { get; } = new byte[8];

        /// <summary>
        /// The minor component of the firmware version
        /// </summary>
        public ushort Version_Minor { get; } = 0;

        /// <summary>
        /// The major component of the firmware version
        /// </summary>
        public ushort Version_Major { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateHostFirmware"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateHostFirmware"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateHostFirmware(byte[] bytes)
        {
            if (bytes.Length != 20)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 20");

            Build = BitConverter.ToUInt64(bytes, 0);
            Reserved6 = bytes.Skip(8).Take(8).ToArray();
            Version_Minor = BitConverter.ToUInt16(bytes, 16);
            Version_Major = BitConverter.ToUInt16(bytes, 18);
        }

        public override string ToString()
        {
            return $@"Build: {Build}
Reserved6: {BitConverter.ToString(Reserved6)}
Version_Minor: {Version_Minor}
Version_Major: {Version_Major}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateHostFirmware stateHostFirmware = (StateHostFirmware)obj;
                return Build == stateHostFirmware.Build &&
                       Reserved6.SequenceEqual(stateHostFirmware.Reserved6) &&
                       Version_Minor == stateHostFirmware.Version_Minor &&
                       Version_Major == stateHostFirmware.Version_Major;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Build, Reserved6, Version_Minor, Version_Major);
        }
    }
}
