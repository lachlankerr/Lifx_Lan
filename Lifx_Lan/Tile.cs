using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// This represents the information for a single device in a chain. 
    /// It is used by the StateDeviceChain (702) packet
    /// </summary>
    internal class Tile
    {
        /// <summary>
        /// 
        /// </summary>
        public Int16 Accel_meas_x { get; }

        /// <summary>
        /// 
        /// </summary>
        public Int16 Accel_meas_y { get; }

        /// <summary>
        /// 
        /// </summary>
        public Int16 Accel_meas_z { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved6 { get; } = new byte[2];

        /// <summary>
        /// 
        /// </summary>
        public float User_x { get; }

        /// <summary>
        /// 
        /// </summary>
        public float User_y { get; }

        /// <summary>
        /// The number of zones that make up each row
        /// </summary>
        public byte Width { get; }

        /// <summary>
        /// The number of zones that make up each column
        /// </summary>
        public byte Height { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved7 { get; } = new byte[1];

        /// <summary>
        /// The vendor id of the device (See StateVersion (33))
        /// </summary>
        public UInt32 Device_version_vendor { get; }

        /// <summary>
        /// The product id of the device (See StateVersion (33))
        /// </summary>
        public UInt32 Device_version_product { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved8 { get; } = new byte[4];

        /// <summary>
        /// The epoch of the time the firmware was created (See StateHostFirmware (15))
        /// </summary>
        public UInt64 Firmware_build { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved9 { get; } = new byte[8];

        /// <summary>
        /// The minor component of the firmware version (See StateHostFirmware (15))
        /// </summary>
        public UInt16 Firmware_version_minor { get; }

        /// <summary>
        /// The major component of the firmware version (See StateHostFirmware (15))
        /// </summary>
        public UInt16 Firmware_version_major { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved10 { get; } = new byte[4];

        /// <summary>
        /// 
        /// </summary>
        public Tile() 
        { 

        }
    }
}
