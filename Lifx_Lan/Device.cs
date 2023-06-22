using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class Device
    {
        public byte[] Serial_Number { get; } = new byte[6];

        public IPAddress Address { get; }

        public int Port { get; } = Lan.DEFAULT_PORT;

        public Product? Product { get; set; } = null;

        public Device(byte[] serial_number, IPAddress address, int port)
        {
            if (serial_number.Length != 6)
                throw new ArgumentException($"Serial number must be of length 6, given: {BitConverter.ToString(Serial_Number)}");
            Serial_Number = serial_number;
            Address = address;
            Port = port;
        }

        public Device(byte[] serial_number, IPAddress address, int port, int vendor_id, int product_id, int firmware_major, int firmware_minor) 
        {
            if (serial_number.Length != 6)
                throw new ArgumentException($"Serial number must be of length 6, given: {BitConverter.ToString(Serial_Number)}");
            Serial_Number = serial_number;
            Address = address;
            Port = port;

            Product = new Product(vendor_id, product_id, firmware_major, firmware_minor);
        }

        public override string ToString()
        {
            string output = $@"Serial_Number: {BitConverter.ToString(Serial_Number)}
IP: {Address}
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
                       this.Address.Equals(device.Address) &&
                       this.Port == device.Port &&
                       ((this.Product == null && device.Product == null) ||
                       (this.Product != null && device.Product != null && this.Product.Equals(device.Product)));
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Serial_Number, Address, Port, Product);
        }
    }
}
