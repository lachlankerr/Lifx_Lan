using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal enum LightLastHevCycleResult : byte
    {
        SUCCESS = 0,
        BUSY = 1,
        INTERRUPTED_BY_RESET = 2,
        INTERRUPTED_BY_HOMEKIT = 3,
        INTERRUPTED_BY_LAN = 4,
        INTERRUPTED_BY_CLOUD = 5,
        NONE = 255,
    }
}
