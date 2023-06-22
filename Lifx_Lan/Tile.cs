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
        public Int16 Accel_Meas_X { get; }

        /// <summary>
        /// 
        /// </summary>
        public Int16 Accel_Meas_Y { get; }

        /// <summary>
        /// 
        /// </summary>
        public Int16 Accel_Meas_Z { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved6 { get; } = new byte[2];

        /// <summary>
        /// 
        /// </summary>
        public float User_X { get; }

        /// <summary>
        /// 
        /// </summary>
        public float User_Y { get; }

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
        public UInt32 Device_Version_Vendor { get; }

        /// <summary>
        /// The product id of the device (See StateVersion (33))
        /// </summary>
        public UInt32 Device_Version_Product { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved8 { get; } = new byte[4];

        /// <summary>
        /// The epoch of the time the firmware was created (See StateHostFirmware (15))
        /// </summary>
        public UInt64 Firmware_Build { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved9 { get; } = new byte[8];

        /// <summary>
        /// The minor component of the firmware version (See StateHostFirmware (15))
        /// </summary>
        public UInt16 Firmware_Version_Minor { get; }

        /// <summary>
        /// The major component of the firmware version (See StateHostFirmware (15))
        /// </summary>
        public UInt16 Firmware_Version_Major { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved10 { get; } = new byte[4];

        /// <summary>
        /// 
        /// </summary>
        public Tile(Int16 accel_meas_x, Int16 accel_meas_y, Int16 accel_meas_z, byte[] reserved6, float user_x, float user_y, byte width, byte height,
                    byte[] reserved7, UInt32 device_version_vendor, UInt32 device_version_product, byte[] reserved8, UInt64 firmware_build,
                    byte[] reserved9, UInt16 firmware_version_minor, UInt16 firmware_version_major, byte[] reserved10) 
        { 
            Accel_Meas_X = accel_meas_x;
            Accel_Meas_Y = accel_meas_y;
            Accel_Meas_Z = accel_meas_z;
            Reserved6 = reserved6;
            User_X = user_x;
            User_Y = user_y;
            Width = width;
            Height = height;
            Reserved7 = reserved7;
            Device_Version_Vendor = device_version_vendor;
            Device_Version_Product = device_version_product;
            Reserved8 = reserved8;
            Firmware_Build = firmware_build;
            Reserved9 = reserved9;
            Firmware_Version_Minor = firmware_version_minor;
            Firmware_Version_Major = firmware_version_major;
            Reserved10 = reserved10;
        }

        public override string ToString()
        {
            return $@"Accel_Meas_X: {Accel_Meas_X};
Accel_Meas_Y: {Accel_Meas_Y}
Accel_Meas_Z: {Accel_Meas_Z}
Reserved6: {Reserved6}
User_X: {User_X}
User_Y: {User_Y}
Width: {Width}
Height: {Height}
Reserved7: {Reserved7}
Device_Version_Vendor: {Device_Version_Vendor}
Device_Version_Product: {Device_Version_Product}
Reserved8: {Reserved8}
Firmware_Build: {Firmware_Build}
Reserved9: {Reserved9}
Firmware_Version_Minor: {Firmware_Version_Minor}
Firmware_Version_Major: {Firmware_Version_Major}
Reserved10: {Reserved10}";
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Accel_Meas_X);
            hash.Add(Accel_Meas_Y);
            hash.Add(Accel_Meas_Z);
            hash.Add(Reserved6);
            hash.Add(User_X);
            hash.Add(User_Y);
            hash.Add(Width);
            hash.Add(Height);
            hash.Add(Reserved7);
            hash.Add(Device_Version_Vendor);
            hash.Add(Device_Version_Product);
            hash.Add(Reserved8);
            hash.Add(Firmware_Build);
            hash.Add(Reserved9);
            hash.Add(Firmware_Version_Minor);
            hash.Add(Firmware_Version_Major);
            hash.Add(Reserved10);
            return hash.ToHashCode();
        }
    }
}
