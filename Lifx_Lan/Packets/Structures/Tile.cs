using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Structures
{
    /// <summary>
    /// This represents the information for a single device in a chain. 
    /// It is used by the StateDeviceChain (702) packet
    /// </summary>
    internal class Tile
    {
        /// <summary>
        /// The size of a <see cref="Tile"/> struct in bytes
        /// </summary>
        public const int SIZE = 55;

        /// <summary>
        /// For tile orientation
        /// </summary>
        public short Accel_Meas_X { get; }

        /// <summary>
        /// For tile orientation
        /// </summary>
        public short Accel_Meas_Y { get; }

        /// <summary>
        /// For tile orientation
        /// </summary>
        public short Accel_Meas_Z { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved6 { get; } = new byte[2];

        /// <summary>
        /// For tile positioning
        /// </summary>
        public float User_X { get; }

        /// <summary>
        /// For tile positioning
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
        public byte Reserved7 { get; } = 0;

        /// <summary>
        /// The vendor id of the device (See StateVersion (33))
        /// </summary>
        public uint Device_Version_Vendor { get; }

        /// <summary>
        /// The product id of the device (See StateVersion (33))
        /// </summary>
        public uint Device_Version_Product { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved8 { get; } = new byte[4];

        /// <summary>
        /// The epoch of the time the firmware was created (See StateHostFirmware (15))
        /// </summary>
        public ulong Firmware_Build { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved9 { get; } = new byte[8];

        /// <summary>
        /// The minor component of the firmware version (See StateHostFirmware (15))
        /// </summary>
        public ushort Firmware_Version_Minor { get; }

        /// <summary>
        /// The major component of the firmware version (See StateHostFirmware (15))
        /// </summary>
        public ushort Firmware_Version_Major { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Reserved10 { get; } = new byte[4];

        /// <summary>
        /// 
        /// </summary>
        public Tile(short accel_meas_x, short accel_meas_y, short accel_meas_z, byte[] reserved6, float user_x, float user_y, byte width, byte height,
                    byte reserved7, uint device_version_vendor, uint device_version_product, byte[] reserved8, ulong firmware_build,
                    byte[] reserved9, ushort firmware_version_minor, ushort firmware_version_major, byte[] reserved10)
        {
            Accel_Meas_X = accel_meas_x;                        //2
            Accel_Meas_Y = accel_meas_y;                        //2
            Accel_Meas_Z = accel_meas_z;                        //2
            Reserved6 = reserved6;                              //2
            User_X = user_x;                                    //4
            User_Y = user_y;                                    //4
            Width = width;                                      //1
            Height = height;                                    //1
            Reserved7 = reserved7;                              //1
            Device_Version_Vendor = device_version_vendor;      //4
            Device_Version_Product = device_version_product;    //4
            Reserved8 = reserved8;                              //4
            Firmware_Build = firmware_build;                    //8
            Reserved9 = reserved9;                              //8
            Firmware_Version_Minor = firmware_version_minor;    //2
            Firmware_Version_Major = firmware_version_major;    //2
            Reserved10 = reserved10;                            //4
        }

        public override string ToString()
        {
            return $@"Accel_Meas_X: {Accel_Meas_X};
Accel_Meas_Y: {Accel_Meas_Y}
Accel_Meas_Z: {Accel_Meas_Z}
Reserved6: {BitConverter.ToString(Reserved6)}
User_X: {User_X}
User_Y: {User_Y}
Width: {Width}
Height: {Height}
Reserved7: {Reserved7}
Device_Version_Vendor: {Device_Version_Vendor}
Device_Version_Product: {Device_Version_Product}
Reserved8: {BitConverter.ToString(Reserved8)}
Firmware_Build: {Firmware_Build}
Reserved9: {BitConverter.ToString(Reserved9)}
Firmware_Version_Minor: {Firmware_Version_Minor}
Firmware_Version_Major: {Firmware_Version_Major}
Reserved10: {BitConverter.ToString(Reserved10)}";
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
