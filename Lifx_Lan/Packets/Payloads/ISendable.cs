using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This payload type can be sent to a device.
    /// </summary>
    internal interface ISendable
    {
        static abstract FeaturesFlags NeededCapabilities();
    }
}
