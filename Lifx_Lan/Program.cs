using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lifx_Lan
{
    /// <summary>
    /// Based off https://lan.developer.lifx.com/docs
    /// </summary>
    internal class Program
    {
        public const int PORT = 56700;
        public const string IP = "192.168.10.25";
        static void Main(string[] args)
        {
            var packet = new LifxPacket(Pkt_Type.SetPower, new byte[2] { 0xFF, 0xFF });
            Console.WriteLine("Sent: \n" + BitConverter.ToString(packet.ToBytes()));

            UdpClient udpClient = new UdpClient(PORT);
            try
            {
                Decoder.IsValid(packet.ToBytes());
                Decoder.PrintFields(packet.ToBytes());

                udpClient.Connect(IP, PORT);
                udpClient.Send(packet.ToBytes(), packet.ToBytes().Length);

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                byte[] receivedBytes = udpClient.Receive(ref RemoteIpEndPoint);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("\nReceived: \n" + BitConverter.ToString(receivedBytes));

                Decoder.IsValid(receivedBytes);
                Decoder.PrintFields(receivedBytes);

                Console.WriteLine("\nThis message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}