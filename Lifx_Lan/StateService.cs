using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// This packet is used to tell you what services are available and the port each service is on.
    /// 
    /// This packet is the reply to the GetService (2) message
    /// </summary>
    internal class StateService
    {
        /// <summary>
        /// Using Services Enum
        /// </summary>
        public Services Service { get; } = 0;

        /// <summary>
        /// The port of the service.
        /// This value is usually 56700 but you should not assume this is always the case.
        /// </summary>
        public UInt32 Port { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateService"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateService"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateService(byte[] bytes)
        {
            if (bytes.Length != 5)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 5");

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
