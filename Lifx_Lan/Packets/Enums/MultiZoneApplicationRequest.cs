using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Enums
{
    internal enum MultiZoneApplicationRequest : byte
    {
        NO_APPLY = 0,
        APPLY = 1,
        APPLY_ONLY = 2,
    }
}
