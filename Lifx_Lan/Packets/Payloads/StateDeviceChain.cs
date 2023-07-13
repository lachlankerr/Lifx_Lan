using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// Information about each device in the chain.
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetDeviceChain (701) message
    /// </summary>
    internal class StateDeviceChain : Payload, IReceivable
    {
        /// <summary>
        /// The initial number of bytes we need to determine how big this payload should be
        /// </summary>
        public const int INIT_SIZE = 1;

        /// <summary>
        /// The maximum length the Tile_Devices array can be
        /// </summary>
        public const int MAX_TILE_DEVICES = 16;

        /// <summary>
        /// The index of the first device in the chain this packet refers to
        /// </summary>
        public byte Start_Index { get; } = 0;

        /// <summary>
        /// The information for each device in the chain
        /// </summary>
        public Tile[] Tile_Devices { get; } = new Tile[MAX_TILE_DEVICES];

        /// <summary>
        /// The number of device in tile_devices that map to devices in the chain.
        /// </summary>
        public byte Tile_Devices_Count { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateDeviceChain"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateDeviceChain"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateDeviceChain(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length >= INIT_SIZE)
                throw new ArgumentException($"Not enough bytes to read the whole structure for this payload type, expected at least {INIT_SIZE}");

            Start_Index = bytes[0];
            Tile_Devices_Count = bytes[^1]; //last element of array

            //secondary check after we have received the Colors_Count value
            if (bytes.Length != INIT_SIZE + Tile.SIZE * Tile_Devices_Count)
                throw new ArgumentException($"Wrong number of bytes for this payload type, expected {INIT_SIZE + Color.SIZE * Tile_Devices_Count}");

            for (int i = 0; i < Tile_Devices_Count; i++)
            {
                int offset = i * Tile.SIZE;

                short Accel_Meas_X = BitConverter.ToInt16(bytes, 1 + offset);               //2
                short Accel_Meas_Y = BitConverter.ToInt16(bytes, 3 + offset);               //2
                short Accel_Meas_Z = BitConverter.ToInt16(bytes, 5 + offset);               //2
                byte[] Reserved6 = bytes.Skip(7 + offset).Take(2).ToArray();                //2
                float User_X = BitConverter.ToSingle(bytes, 9 + offset);                    //4
                float User_Y = BitConverter.ToSingle(bytes, 13 + offset);                   //4
                byte Width = bytes[17 + offset];                                            //1
                byte Height = bytes[18 + offset];                                           //1
                byte Reserved7 = bytes[19 + offset];                                        //1
                uint Device_Version_Vendor = BitConverter.ToUInt32(bytes, 20 + offset);     //4
                uint Device_Version_Product = BitConverter.ToUInt32(bytes, 24 + offset);    //4
                byte[] Reserved8 = bytes.Skip(28 + offset).Take(4).ToArray();               //4
                ulong Firmware_Build = BitConverter.ToUInt64(bytes, 32 + offset);           //8
                byte[] Reserved9 = bytes.Skip(40 + offset).Take(8).ToArray();               //8
                ushort Firmware_Version_Minor = BitConverter.ToUInt16(bytes, 48 + offset);  //2
                ushort Firmware_Version_Major = BitConverter.ToUInt16(bytes, 50 + offset);  //2
                byte[] Reserved10 = bytes.Skip(52 + offset).Take(4).ToArray();              //4
                
                Tile_Devices[i] = new Tile(Accel_Meas_X, Accel_Meas_Y, Accel_Meas_Z, 
                    Reserved6, User_X, User_Y, Width, Height, Reserved7, 
                    Device_Version_Vendor, Device_Version_Product, Reserved8, 
                    Firmware_Build, Reserved9, 
                    Firmware_Version_Minor, Firmware_Version_Major, Reserved10);
            }
        }

        public override string ToString()
        {
            return $@"Start_Index: {Start_Index}
Tile_Devices_Count: {Tile_Devices_Count}
Tile_Devices: 
{string.Join($"\n\n", Tile_Devices.ToList())}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateDeviceChain stateDeviceChain = (StateDeviceChain)obj;
                return Start_Index == stateDeviceChain.Start_Index &&
                       Tile_Devices.SequenceEqual(stateDeviceChain.Tile_Devices) &&
                       Tile_Devices_Count == stateDeviceChain.Tile_Devices_Count;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start_Index, Tile_Devices, Tile_Devices_Count);
        }
    }
}
