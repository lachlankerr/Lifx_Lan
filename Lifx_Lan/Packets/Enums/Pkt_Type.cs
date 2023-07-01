﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Enums
{
    internal enum Pkt_Type : ushort
    {
        ////////////////
        //Get Messages//
        ////////////////

        //Discovery
        /// <summary>
        /// This packet is used for Discovery of devices. 
        /// Typically you would broadcast this message to the network (with tagged field in the header set to 0 and the target field in the header set to all zeros)
        /// 
        /// Each device on the network that receives this packet will send back multiple StateService (3) messages that say what services are available and the port those services are on.
        /// 
        /// The only StateService (3) message you care about will tell you that UDP is available on a port that is usually 56700. 
        /// You can determine the IP address of the device from information your UDP socket should receive when it gets those bytes.
        /// </summary>
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
        /// <summary>
        /// This packet lets you set the current level of power on the device.
        /// </summary>
        /// <param name="level">Uint16 : If you specify 0 the light will turn off and if you specify 65535 the device will turn on.</param>
        /// <returns>StatePower (22) message</returns>
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
        /// <summary>
        /// This packet is returned when you specify ack_required=1.
        /// </summary>
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
