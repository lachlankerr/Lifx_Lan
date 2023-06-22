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

        public Product? Product { get; set; } = null;

        public Device(byte[] serial_number, string ip, int port)
        {
            Serial_Number = serial_number;
            IP = ip;
            Port = port;
        }

        public Device(byte[] serial_number, string ip, int port, int vendor_id, int product_id, int firmware_major, int firmware_minor) 
        {
            Serial_Number = serial_number;
            IP = ip;
            Port = port;

            Product = new Product(vendor_id, product_id, firmware_major, firmware_minor);
        }

        public override string ToString()
        {
            string output = $@"Serial_Number: {BitConverter.ToString(Serial_Number)}
IP: {IP}
Port: {Port}"; output += Product != null ? $@"

{Product}" : "";
            return output;
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Device device = (Device)obj;
                return this.Serial_Number.SequenceEqual(device.Serial_Number) &&
                       this.IP.Equals(device.IP) &&
                       this.Port == device.Port &&
                       ((this.Product == null && device.Product == null) ||
                       (this.Product != null && device.Product != null && this.Product.Equals(device.Product)));
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Serial_Number, IP, Port, Product);
        }
    }
}
