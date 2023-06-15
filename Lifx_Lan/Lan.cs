using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lifx_Lan
{
    /// <summary>
    /// Based off https://lan.developer.lifx.com/docs
    /// </summary>
    internal class Lan
    {
        public const int DEFAULT_PORT = 56700;
        public const int ONE_SECOND = 1000; 

        UdpClient udpClient;

        static void Main(string[] args)
        {
            LifxPacket testPacket = new LifxPacket(target: new byte[] { 0xD0, 0x73, 0xD5, 0x2D, 0x8D, 0xA2, 0x00, 0x00 },
                                                   pkt_type: Pkt_Type.SetPower, 
                                                   payload: new byte[2] { 0xFF, 0xFF },
                                                   ack_required: true);

            Lan lan = new Lan(ONE_SECOND * 10);

            //lan.SendPacket(testPacket, "192.168.10.25", printMessages: true);
            //lan.ReceivePacket(printMessages: true);

            lan.StartDiscovery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">in milliseconds</param>
        public Lan(int timeout)
        {
            udpClient = new UdpClient(DEFAULT_PORT);
            udpClient.Client.SendTimeout = timeout;
        }

        public void StartDiscovery()
        {
            //construct discovery packet
            LifxPacket discoveryPacket = new LifxPacket(Pkt_Type.GetService, false);

            SendPacket(discoveryPacket, "192.168.10.255", printMessages: true);

            ReceivePacket(printMessages: true); // why won't this receive? wrong port ??
        }

        /// <summary>
        /// Sends the specified <see cref="LifxPacket"/> to the specified ip on the specified port.
        /// </summary>
        /// <param name="packet">The <see cref="LifxPacket"/> to send to the device</param>
        /// <param name="ip">The ip address to send our packet to</param>
        /// <param name="port">The port number we are sending our packet on</param>
        /// <param name="printMessages">Whether to print any text or not</param>
        /// <returns>The number of bytes sent</returns>
        public int SendPacket(LifxPacket packet, string ip, int port = DEFAULT_PORT, bool printMessages = false)
        {
            if (Decoder.IsValid(packet.ToBytes())) //no point sending useless data that might damage our devices
            {
                try
                {
                    udpClient.Connect(ip, port);
                    int bytesSent = udpClient.Send(packet.ToBytes(), packet.ToBytes().Length); //better to use length in case there's an error with our code


                    if (printMessages)
                    {
                        Console.WriteLine("Sent to " + ip + " on port " + port + ":");
                        Console.WriteLine(BitConverter.ToString(packet.ToBytes()));
                        Console.WriteLine();
                        Decoder.PrintFields(packet.ToBytes());
                    }

                    return bytesSent;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">The port number we are waiting for a response on</param>
        /// <returns></returns>
        public LifxPacket? ReceivePacket(int port = DEFAULT_PORT, bool printMessages = false)
        {
            LifxPacket? receivedPacket = null;
            try
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                byte[] receivedBytes = udpClient.Receive(ref RemoteIpEndPoint);

                // Uses the IPEndPoint object to determine which of these two hosts responded.


                if (Decoder.IsValid(receivedBytes))
                {
                    receivedPacket = Decoder.ToLifxPacket(receivedBytes);

                    if (printMessages)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Received:");
                        Console.WriteLine(BitConverter.ToString(receivedBytes));
                        Console.WriteLine();
                        Decoder.PrintFields(receivedBytes);
                        Console.WriteLine();
                        Console.WriteLine("This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
                    }
                }
                else
                {
                    // wait for proper packet to arrive?
                }
                return receivedPacket;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}