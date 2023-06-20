using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class Device
    {
        public byte[] Serial_Number { get; } = new byte[6];

        public string IP { get; } = "";

        public int Port { get; } = Lan.DEFAULT_PORT;

        public Product Product { get; }

        public Device(byte[] serial_number, string ip, int port, int vendor_id, int product_id, int firmware_major, int firmware_minor) 
        {
            Serial_Number = serial_number;
            IP = ip;
            Port = port;

            Product = new Product(vendor_id, product_id, firmware_major, firmware_minor);
        }

        public override string ToString()
        {
            return $@"Serial_Number: {BitConverter.ToString(Serial_Number)}
IP: {IP}
Port: {Port}

{Product}";
        }
    }
}
