using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads.Get;
using Lifx_Lan.Packets.Payloads.State.Device;
using Lifx_Lan.Packets.Payloads.State.Discovery;
using Lifx_Lan.Packets.Payloads.State.Light;
using Lifx_Lan.Packets.Payloads.State.MultiZone;
using Lifx_Lan.Packets.Payloads.State.Relay;
using Lifx_Lan.Packets.Payloads.State.Tiles;
using Lifx_Lan.Packets.Payloads.State.Tiless;
using Lifx_Lan.Packets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Gets the size of the object in bytes, 
        /// null returns 0, 
        /// undefined types throw a <see cref="NotSupportedException"/>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">For any undefined types</exception>
        public static int Size(this object o)
        {
            if (o == null)
                return 0;

            Type t = o.GetType();

            if (o.GetType().IsArray)
            {
                t = o.GetType().GetElementType()!;
                Array arr = (o as Array)!;
                return arr.Length * CalcSize(t);
            }
            else if (t == typeof(string)) //string arrays will have issues
                return Encoding.Unicode.GetByteCount((string)o);

            return CalcSize(t);
        }

        public static int CalcSize(this Type t)
        {
            if (t == null)
                return 0;

            //primitives
            else if (t == typeof(sbyte))
                return sizeof(sbyte);       //1
            else if (t == typeof(byte))
                return sizeof(byte);        //1
            else if (t == typeof(short))
                return sizeof(short);       //2
            else if (t == typeof(ushort))
                return sizeof(ushort);      //2
            else if (t == typeof(int))
                return sizeof(int);         //4
            else if (t == typeof(uint))
                return sizeof(uint);        //4
            else if (t == typeof(long))
                return sizeof(long);        //8
            else if (t == typeof(ulong))
                return sizeof(ulong);       //8
            else if (t == typeof(char))
                return sizeof(char);        //2
            else if (t == typeof(float))
                return sizeof(float);       //4
            else if (t == typeof(double))
                return sizeof(double);      //8
            else if (t == typeof(decimal))
                return sizeof(decimal);     //16
            else if (t == typeof(bool))
                return sizeof(bool);        //1

            //structs
            else if (t == typeof(Color))
                return Color.SIZE;
            else if (t == typeof(Tile))
                return Tile.SIZE;

            //enums
            else if (t == typeof(Services))
                return sizeof(Services);
            else if (t == typeof(Direction))
                return sizeof(Direction);
            else if (t == typeof(LightLastHevCycleResult))
                return sizeof(LightLastHevCycleResult);
            else if (t == typeof(MultiZoneApplicationRequest))
                return sizeof(MultiZoneApplicationRequest);
            else if (t == typeof(MultiZoneEffectType))
                return sizeof(MultiZoneEffectType);
            else if (t == typeof(MultiZoneExtendedApplicationRequest))
                return sizeof(MultiZoneExtendedApplicationRequest);
            else if (t == typeof(TileEffectType))
                return sizeof(TileEffectType);
            else if (t == typeof(Waveform))
                return sizeof(Waveform);

            throw new NotSupportedException($"The type {t.FullName} has not been defined for CalcSize()");
        }

        public static Type MappedType(this Pkt_Type pkt_type)
        {
            switch (pkt_type)
            {
                //Discovery
                //case Pkt_Type.GetService: return typeof(GetService);

                //Device
                //case Pkt_Type.GetHostFirmware: return typeof(GetHostFirmware);
                //case Pkt_Type.GetWifiInfo: return typeof(GetWifiInfo);
                //case Pkt_Type.GetWifiFirmware: return typeof(GetWifiFirmware);
                //case Pkt_Type.GetPower: return typeof(GetPower);
                //case Pkt_Type.GetLabel: return typeof(GetLabel);
                //case Pkt_Type.GetVersion: return typeof(GetVersion);
                //case Pkt_Type.GetInfo: return typeof(GetInfo);
                //case Pkt_Type.GetLocation: return typeof(GetLocation);
                //case Pkt_Type.GetGroup: return typeof(GetGroup);
                case Pkt_Type.EchoRequest: return typeof(EchoRequest);

                //Light
                //case Pkt_Type.GetColor: return typeof(GetColor);
                //case Pkt_Type.GetLightPower: return typeof(GetLightPower);
                //case Pkt_Type.GetInfrared: return typeof(GetInfrared);
                //case Pkt_Type.GetHevCycle: return typeof(GetHevCycle);
                //case Pkt_Type.GetHevCycleConfiguration: return typeof(GetHevCycleConfiguration);
                //case Pkt_Type.GetLastHevCycleResult: return typeof(GetLastHevCycleResult);

                //MultiZone
                case Pkt_Type.GetColorZones: return typeof(GetColorZones);
                //case Pkt_Type.GetMultiZoneEffect: return typeof(GetMultiZoneEffect);
                //case Pkt_Type.GetExtendedColorZones: return typeof(GetExtendedColorZones);

                //Relay
                case Pkt_Type.GetRPower: return typeof(GetRPower);

                //Tile
                //case Pkt_Type.GetDeviceChain: return typeof(GetDeviceChain);
                case Pkt_Type.Get64: return typeof(Get64);
                case Pkt_Type.GetTileEffect: return typeof(GetTileEffect);
                //case Pkt_Type.SensorGetAmbientLight: return typeof(SensorGetAmbientLight);


                ////////////////
                //SetMessages//
                ////////////////

                //Device
                //case Pkt_Type.SetPower: return typeof(SetPower);
                //case Pkt_Type.SetLabel: return typeof(SetLabel);
                //case Pkt_Type.SetReboot: return typeof(SetReboot);
                //case Pkt_Type.SetLocation: return typeof(SetLocation);
                //case Pkt_Type.SetGroup: return typeof(SetGroup);

                //Light
                //case Pkt_Type.SetColor: return typeof(SetColor);
                //case Pkt_Type.SetWaveform: return typeof(SetWaveform);
                //case Pkt_Type.SetLightPower: return typeof(SetLightPower);
                //case Pkt_Type.SetWaveformOptional: return typeof(SetWaveformOptional);
                //case Pkt_Type.SetInfrared: return typeof(SetInfrared);
                //case Pkt_Type.SetHevCycle: return typeof(SetHevCycle);
                //case Pkt_Type.SetHevCycleConfiguration: return typeof(SetHevCycleConfiguration);

                //MultiZone
                //case Pkt_Type.SetColorZones: return typeof(SetColorZones);
                //case Pkt_Type.SetMultiZoneEffect: return typeof(SetMultiZoneEffect);
                //case Pkt_Type.SetExtendedColorZones: return typeof(SetExtendedColorZones);

                //Relay
                //case Pkt_Type.SetRPower: return typeof(SetRPower);

                //Tile
                //case Pkt_Type.SetUserPosition: return typeof(SetUserPosition);
                //case Pkt_Type.Set64: return typeof(Set64);
                //case Pkt_Type.SetTileEffect: return typeof(SetTileEffect);


                //////////////////
                //StateMessages//
                //////////////////

                //Core
                //case Pkt_Type.Acknowledgement: return typeof(Acknowledgement);

                //Discovery
                case Pkt_Type.StateService: return typeof(StateService);

                //Device
                case Pkt_Type.StateHostFirmware: return typeof(StateHostFirmware);
                case Pkt_Type.StateWifiInfo: return typeof(StateWifiInfo);
                case Pkt_Type.StateWifiFirmware: return typeof(StateWifiFirmware);
                case Pkt_Type.StatePower: return typeof(StatePower);
                case Pkt_Type.StateLabel: return typeof(StateLabel);
                case Pkt_Type.StateVersion: return typeof(StateVersion);
                case Pkt_Type.StateInfo: return typeof(StateInfo);
                case Pkt_Type.StateLocation: return typeof(StateLocation);
                case Pkt_Type.StateGroup: return typeof(StateGroup);
                case Pkt_Type.EchoResponse: return typeof(EchoResponse);
                case Pkt_Type.StateUnhandled: return typeof(StateUnhandled);

                //Light
                case Pkt_Type.LightState: return typeof(LightState);
                case Pkt_Type.StateLightPower: return typeof(StateLightPower);
                case Pkt_Type.StateInfrared: return typeof(StateInfrared);
                case Pkt_Type.StateHevCycle: return typeof(StateHevCycle);
                case Pkt_Type.StateHevCycleConfiguration: return typeof(StateHevCycleConfiguration);
                case Pkt_Type.StateLastHevCycleResult: return typeof(StateLastHevCycleResult);

                //MultiZone
                case Pkt_Type.StateZone: return typeof(StateZone);
                case Pkt_Type.StateMultiZone: return typeof(StateMultiZone);
                case Pkt_Type.StateMultiZoneEffect: return typeof(StateMultiZoneEffect);
                case Pkt_Type.StateExtendedColorZones: return typeof(StateExtendedColorZones);

                //Relay
                case Pkt_Type.StateRPower: return typeof(StateRPower);

                //Tile
                case Pkt_Type.StateDeviceChain: return typeof(StateDeviceChain);
                case Pkt_Type.State64: return typeof(State64);
                case Pkt_Type.StateTileEffect: return typeof(StateTileEffect);
                case Pkt_Type.SensorStateAmbientLight: return typeof(SensorStateAmbientLight);

                default: 
                    throw new NotSupportedException($"{pkt_type} has not been mapped yet.");
            }
        }
    }
}
