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
        public byte[] Serial_Number { get; } = new byte[6];

        public IPAddress Address { get; }

        public int Port { get; } = Lan.DEFAULT_PORT;

        public NetworkInfo(byte[] serial_number, IPAddress address, int port)
        {
            if (serial_number.Length != 6)
                throw new ArgumentException($"Serial number must be of length 6, given: {BitConverter.ToString(Serial_Number)}");
            Serial_Number = serial_number;
            Address = address;
            Port = port;
        }

        public override string ToString()
        {
            return $@"Serial_Number: {BitConverter.ToString(Serial_Number)}
IP: {Address}
Port: {Port}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                NetworkInfo networkInfo = (NetworkInfo)obj;
                return this.Serial_Number.SequenceEqual(networkInfo.Serial_Number) &&
                       this.Address.Equals(networkInfo.Address) &&
                       this.Port == networkInfo.Port;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Serial_Number, Address, Port);
        }
    }
}
