using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Lifx_Lan
{
    /// <summary>
    /// Based off https://lan.developer.lifx.com/docs
    /// </summary>
    internal class Lan
    {
        public const int DEFAULT_PORT = 56700;
        public const int ONE_SECOND = 1000;

        public byte Sequence = 1;

        UdpClient UdpClient;

        List<NetworkInfo> receivedPackets = new List<NetworkInfo>();
        CancellationTokenSource ReceivingPacketsCancellation = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            LifxPacket testPacket = new LifxPacket(target: new byte[] { 0xD0, 0x73, 0xD5, 0x2D, 0x8D, 0xA2, 0x00, 0x00 },
                                                   pkt_type: Pkt_Type.SetPower, 
                                                   payload: new byte[2] { 0xFF, 0xFF },
                                                   ack_required: true);

            //Decoder.PrintFields(new byte[] { 0x24, 0x00, 0x00, 0x34, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00 });

            Lan lan = new Lan(ONE_SECOND * 10);

            //lan.SendPacket(testPacket, new IPEndPoint(IPAddress.Parse("192.168.10.25"), DEFAULT_PORT), printMessages: true);
            //lan.ReceivePacket(printMessages: true);

            //List<NetworkInfo> networkInfos = lan.Discovery(ONE_SECOND * 1, 5, 5, true);
            //lan.GetProductInfo(networkInfos);
            //Console.WriteLine(new Product(1, 30, 3, 90));
            lan.StartReceivingPackets();
            await Task.Delay(ONE_SECOND * 60);
            lan.StopReceivingPackets();
            await Task.Delay(ONE_SECOND);
        }

        public async void StartReceivingPackets(bool ignoreGetService = false)
        {
            CancellationToken token = ReceivingPacketsCancellation.Token;
            Debug.WriteLine("Waiting for packets...");
            while (!token.IsCancellationRequested)
            {
                UdpReceiveResult receivedResult;
                try
                {
                    receivedResult = await UdpClient.ReceiveAsync(token);
                    LifxPacket receivedPacket = Decoder.ToLifxPacket(receivedResult.Buffer);
                    NetworkInfo networkInfo = new NetworkInfo(receivedPacket.frameAddress.Target, receivedResult.RemoteEndPoint.Address, receivedResult.RemoteEndPoint.Port, receivedPacket);
                    if (!(ignoreGetService && receivedPacket.protocolHeader.Pkt_Type == Pkt_Type.GetService))
                    {
                        Debug.WriteLine($"Received from {receivedResult.RemoteEndPoint.Address}:{receivedResult.RemoteEndPoint.Port} {BitConverter.ToString(receivedResult.Buffer)}");
                        receivedPackets.Add(networkInfo);
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("Receiving packets cancelled");
                }
                catch (SocketException sex)
                {
                    if (sex.SocketErrorCode != SocketError.TimedOut)
                        throw;
                }
            }
        }

        public void StopReceivingPackets()
        {
            Debug.WriteLine("Stop receiving packets");
            ReceivingPacketsCancellation.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">in milliseconds</param>
        public Lan(int timeout)
        {
            UdpClient = new UdpClient(DEFAULT_PORT);
            UdpClient.EnableBroadcast = true;
            UdpClient.Client.SendTimeout = timeout;
            //UdpClient.Client.ReceiveTimeout = timeout;
            //udpClient.Client.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.Broadcast, true);
        }

        /// <summary>
        /// https://stackoverflow.com/a/27376368
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IPAddress GetLocalIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint ?? throw new Exception("Could not get localEndPoint");
                return endPoint.Address;
            }
        }

        /// <summary>
        /// http://www.java2s.com/Code/CSharp/Network/GetSubnetMask.htm
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }

        /// <summary>
        /// https://stackoverflow.com/a/39338188
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
        {
            uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint broadCastIpAddress = ipAddress | ~ipMaskV4;

            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutPerRun">How long to wait for responses in milliseconds until starting next run</param>
        /// <param name="maxRuns">The maximum number of discovery packets to run if we have not found numDevices yet</param>
        /// <param name="numDevices">The total number of devices we are looking for, set to zero if it is an unknown number</param>
        public List<NetworkInfo> Discovery(int timeoutPerRun = ONE_SECOND, int maxRuns = 5, int numDevices = 0, bool printMessages = false)
        {
            if (printMessages)
                Console.WriteLine($"Looking for {numDevices} devices, max amount of runs is {maxRuns}, timeout per run is {timeoutPerRun} milliseconds");

            List<NetworkInfo> networkInfos = new List<NetworkInfo>();
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Stopwatch stopwatch = new Stopwatch();

            LifxPacket discoveryPacket = new LifxPacket(Pkt_Type.GetService, true);
            for (int r = 0; r < maxRuns; r++)
            {
                stopwatch.Restart();
                SendPacket(discoveryPacket, new IPEndPoint(GetBroadcastAddress(GetLocalIP(), GetSubnetMask(GetLocalIP())), DEFAULT_PORT), printMessages: false);
                stopwatch.Start();

                while (networkInfos.Count < numDevices && stopwatch.ElapsedMilliseconds < timeoutPerRun)
                {
                    UdpClient.Client.ReceiveTimeout = Math.Abs(timeoutPerRun - (int)stopwatch.ElapsedMilliseconds);
                    byte[] receivedBytes = new byte[0];
                    try
                    {
                        receivedBytes = UdpClient.Receive(ref RemoteIpEndPoint);
                    }
                    catch (SocketException ex) 
                    {
                        if (ex.SocketErrorCode != SocketError.TimedOut)
                            throw;
                    }

                    bool isValid;

                    try
                    {
                        isValid = Decoder.IsValid(receivedBytes);
                    }
                    catch 
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        LifxPacket receivedPacket = Decoder.ToLifxPacket(receivedBytes);

                        if (receivedPacket.protocolHeader.Pkt_Type == Pkt_Type.StateService)
                        {
                            //Decoder.PrintFields(receivedBytes);
                            StateService stateService = new StateService(receivedPacket.payload.Data);
                            //Console.WriteLine(stateService.Service);
                            //Console.WriteLine(stateService.Port);

                            NetworkInfo newNetworkInfo = new NetworkInfo(receivedPacket.frameAddress.Target, RemoteIpEndPoint.Address, (int)stateService.Port, receivedPacket);

                            if (!networkInfos.Contains(newNetworkInfo))
                                networkInfos.Add(newNetworkInfo);
                        }
                    }
                }
                if (printMessages)
                    Console.WriteLine($"Run {r+1}, devices found so far: {networkInfos.Count}");
                if (networkInfos.Count >= numDevices) //
                    break;
            }

            if (printMessages)
                Console.WriteLine();

            if (printMessages)
                foreach (NetworkInfo networkInfo in networkInfos)
                {
                    Console.WriteLine(networkInfo);
                    Console.WriteLine();
                }

            return networkInfos;
        }

        public void GetProductInfo(List<NetworkInfo> networkInfos)
        {
            foreach (NetworkInfo networkInfo in networkInfos)
            {
                LifxPacket getVersionPacket = new LifxPacket(networkInfo.Serial_Number, Pkt_Type.GetVersion);
                SendPacket(getVersionPacket, new IPEndPoint(networkInfo.Address, networkInfo.Port));
                //ReceivePacket();
            }
        }

        /// <summary>
        /// Sends the specified <see cref="LifxPacket"/> to the specified ip on the specified port.
        /// </summary>
        /// <param name="packet">The <see cref="LifxPacket"/> to send to the device</param>
        /// <param name="ip">The ip address to send our packet to</param>
        /// <param name="port">The port number we are sending our packet on</param>
        /// <param name="printMessages">Whether to print any text or not</param>
        /// <returns>The number of bytes sent</returns>
        public int SendPacket(LifxPacket packet, IPEndPoint addr, bool printMessages = false)
        {
            unchecked { packet.frameAddress.Sequence = Sequence++; }
            if (Decoder.IsValid(packet.ToBytes())) //no point sending useless data that might damage our devices
            {
                try
                {
                    //udpClient.Connect(ip, port); //this was the culprit, was only looking for responses from our broadcast address, which is wrong since any responses would have the ip of the device not broadcast
                    int bytesSent = UdpClient.Send(packet.ToBytes(), packet.ToBytes().Length, addr);
                    //udpClient.Send(packet.ToBytes(), packet.ToBytes().Length, addr);


                    if (printMessages)
                    {
                        Console.WriteLine("Sent to " + addr.Address + " on port " + addr.Port + ":");
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
        public LifxPacket ReceivePacket(bool printMessages = false)
        {
            LifxPacket receivedPacket;
            Console.WriteLine("\nWaiting for response...");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] receivedBytes = UdpClient.Receive(ref RemoteIpEndPoint);

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
                throw new Exception("Received non-lifx packet: " + BitConverter.ToString(receivedBytes) + " from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
            }
            return receivedPacket;
        }
    }
}