using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal enum Types : ushort
    {
        ////////////////
        //Get Messages//
        ////////////////

        //Discovery
        GetService = 2,

        //Device
        GetHostFirmware = 14,
        GetWifiInfo = 16,
        GetWifiFirmware = 18,
        GetPower = 20,
        GetLabel = 23,
        GetVersion = 32,
        GetInfo = 34,
        GetLocation = 48,
        GetGroup = 51,
        EchoRequest = 58,

        //Light
        GetColor = 101,
        GetLightPower = 116,
        GetInfrared = 120,
        GetHevCycle = 142,
        GetHevCycleConfiguration = 145,
        GetLastHevCycleResult = 148,

        //MultiZone
        GetColorZones = 502,
        GetMultiZoneEffect = 507,
        GetExtendedColorZones = 511,

        //Relay
        GetRPower = 816,

        //Tile
        GetDeviceChain = 701,
        Get64 = 707,
        GetTileEffect = 718,
        SensorGetAmbientLight = 401,


        ////////////////
        //Set Messages//
        ////////////////

        //Device
        SetPower = 21,
        SetLabel = 24,
        SetReboot = 38,
        SetLocation = 49,
        SetGroup = 52,

        //Light
        SetColor = 102,
        SetWaveform = 103,
        SetLightPower = 117,
        SetWaveformOptional = 119,
        SetInfrared = 122,
        SetHevCycle = 143,
        SetHevCycleConfiguration = 146,

        //MultiZone
        SetColorZones = 501,
        SetMultiZoneEffect = 508,
        SetExtendedColorZones = 510,

        //Relay
        SetRPower = 817,

        //Tile
        SetUserPosition = 703,
        Set64 = 715,
        SetTileEffect = 719,


        //////////////////
        //State Messages//
        //////////////////

        //Core
        Acknowledgement = 45,

        //Discovery
        StateService = 3,

        //Device
        StateHostFirmware = 15,
        StateWifiInfo = 17,
        StateWifiFirmware = 19,
        StatePower = 22,
        StateLabel = 25,
        StateVersion = 33,
        StateInfo = 35,
        StateLocation = 50,
        StateGroup = 53,
        EchoResponse = 59,
        StateUnhandled = 223,

        //Light
        LightState = 107,
        StateLightPower = 118,
        StateInfrared = 121,
        StateHevCycle = 144,
        StateHevCycleConfiguration = 147,
        StateLastHevCycleResult = 149,

        //MultiZone
        StateZone = 503,
        StateMultiZone = 506,
        StateMultiZoneEffect = 509,
        StateExtendedColorZones = 512,

        //Relay
        StateRPower = 818,

        //Tile
        StateDeviceChain = 702,
        State64 = 711,
        StateTileEffect = 720,
        SensorStateAmbientLight = 402
    }
}
