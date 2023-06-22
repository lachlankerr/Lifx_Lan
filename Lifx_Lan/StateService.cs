using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class StateService
    {
        public Services Service { get; } = 0;

        public UInt32 Port { get; } = 0;

        public StateService(byte[] bytes)
        {
            if (bytes.Length != 5)
                throw new ArgumentException("Wrong number of bytes for this payload type");

            Service = (Services)bytes[0];
            Port = BitConverter.ToUInt32(bytes, 1);
        }

        public StateService(Services service = 0, UInt32 port = 0) 
        {
            Service = service;
            Port = port;
        }

        public override string ToString()
        {
            return $@"Service: {Service} ({(byte)Service})
Port: {Port}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateService stateService = (StateService)obj;
                return this.Service == stateService.Service && 
                       this.Port == stateService.Port;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Service, Port);
        }
    }
}
