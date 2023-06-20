using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class Device
    {
        public byte[] SerialNumber { get; } = new byte[6];

        public string IP { get; } = "";

        public int Port { get; } = Lan.DEFAULT_PORT;

        public Product Product { get; } = new Product();

        public Device(byte[] serialNumber, string ip, int port) 
        {
            SerialNumber = serialNumber;
            IP = ip;
            Port = port;
        }
    }
}
