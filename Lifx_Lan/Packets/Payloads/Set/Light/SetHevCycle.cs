using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Set.Light
{
    internal class SetHevCycle
    {
        public byte Enable { get; } = 0;
        public uint Duration_S { get; } = 0;
    }
}
