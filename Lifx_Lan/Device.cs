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
        public NetworkInfo NetworkInfo { get; }

        public Product Product { get; }

        public Device(NetworkInfo networkInfo, Product product) 
        {
            NetworkInfo = networkInfo;
            Product = product;
        }

        public override string ToString()
        {
            return $@"Serial: {BitConverter.ToString(NetworkInfo.Packet.FrameAddress.Target.Take(6).ToArray())}
Address: {NetworkInfo.Address}:{NetworkInfo.Port}
{Product}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Device device = (Device)obj; //don't use NetworkInfo.Equals() for Device.Equals(), just grab relevant data
                return this.NetworkInfo.Packet.FrameAddress.Target.SequenceEqual(device.NetworkInfo.Packet.FrameAddress.Target) &&
                       this.NetworkInfo.Address.Equals(device.NetworkInfo.Address) &&
                       this.NetworkInfo.Port.Equals(device.NetworkInfo.Port) &&
                       this.Product.Equals(device.Product);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NetworkInfo.Packet.FrameAddress.Target, NetworkInfo.Address, NetworkInfo.Port, Product);
        }
    }
}
