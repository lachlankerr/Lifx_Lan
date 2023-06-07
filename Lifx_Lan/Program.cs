using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lifx_Lan
{
    internal class Program
    {
        public const int PORT = 56700;
        public const string IP = "192.168.10.25";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var packet = new LifxPacket(MessageType.SetPower, new byte[2] { 0xFF, 0xFF });
            Console.WriteLine(BitConverter.ToString(packet.ToBytes()));

            UdpClient udpClient = new UdpClient(PORT);
            try
            {
                udpClient.Connect(IP, PORT);
                udpClient.Send(packet.ToBytes(), packet.ToBytes().Length);

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received \n" + BitConverter.ToString(receiveBytes));
                Console.WriteLine("This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}