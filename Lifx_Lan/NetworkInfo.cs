using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class NetworkInfo
    {
        public IPAddress Address { get; }

        public int Port { get; } = Lan.DEFAULT_PORT;

        public LifxPacket Packet { get; } 

        public NetworkInfo(IPAddress address, int port, LifxPacket packet)
        {
            Address = address;
            Port = port;
            Packet = packet;
        }

        public override string ToString()
        {
            return $@"IP: {Address}
Port: {Port}
Packet: 
{Packet}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                NetworkInfo networkInfo = (NetworkInfo)obj;
                return this.Address.Equals(networkInfo.Address) &&
                       this.Port == networkInfo.Port &&
                       this.Packet == networkInfo.Packet;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Address, Port, Packet);
        }
    }
}
