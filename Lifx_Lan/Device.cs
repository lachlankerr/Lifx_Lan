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
            return $@"{NetworkInfo}
{Product}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Device device = (Device)obj;
                return this.NetworkInfo.Equals(device.NetworkInfo) &&
                       this.Product.Equals(device.Product);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NetworkInfo, Product);
        }
    }
}
