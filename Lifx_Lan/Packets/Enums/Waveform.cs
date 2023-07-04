using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Enums
{
    internal enum Waveform : byte
    {
        SAW = 0,
        SINE = 1,
        HALF_SINE = 2,
        TRIANGLE = 3,
        PULSE = 4,
    }
}
