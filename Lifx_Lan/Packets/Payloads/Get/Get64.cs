using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads.Get
{
    /// <summary>
    /// Used to get the HSBK values of all the zones in devices connected in the chain.
    /// 
    /// This will return one or more State64 (711) messages. 
    /// The maximum number of messages you will receive is the number specified by length in your request.
    /// 
    /// This packet requires the device has the Matrix Zones capability. 
    /// You may use GetVersion (32), GetHostFirmware (14) and the Product Registry to determine whether your device has this capability
    /// </summary>
    internal class Get64
    {

    }
}
