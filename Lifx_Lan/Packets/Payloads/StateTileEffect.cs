using Lifx_Lan.Packets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// The current Firmware Effect running on the device
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// 
    /// This packet is the reply to the GetTileEffect (718) and SetTileEffect (719) messages
    /// </summary>
    internal class StateTileEffect
    {
        public byte Reserved8 { get; } = 0;

        /// <summary>
        /// The unique value identifying the request
        /// </summary>
        public uint InstanceId { get; } = 0;

        public TileEffectType Type { get; } = 0;

        /// <summary>
        /// The time it takes for one cycle in milliseconds.
        /// </summary>
        public uint Speed { get; } = 0;

        /// <summary>
        /// The amount of time left in the current effect in nanoseconds
        /// </summary>
        public ulong Duration { get; } = 0;

        public byte[] Reserved6 { get; } = new byte[4];
        
        public byte[] Reserved7 { get; } = new byte[4];

        /// <summary>
        /// The parameters as specified in the request.
        /// </summary>
        public byte[] Parameters { get; } = new byte[32];

        /// <summary>
        /// The number of colors in the palette that are relevant
        /// </summary>
        public byte Palette_Count { get; } = 0;


    }
}
