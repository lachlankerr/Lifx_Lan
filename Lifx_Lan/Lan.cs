using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using Lifx_Lan.Packets;
using Lifx_Lan.Packets.Enums;
using Lifx_Lan.Packets.Payloads;

namespace Lifx_Lan
{
    /// <summary>
    /// Based off https://lan.developer.lifx.com/docs
    /// </summary>
    internal class Lan
    {
        public const int DEFAULT_PORT = 56700;
        public const int ONE_SECOND = 1000;
        public const int ADDR_SPACE = 15;
        public const int PORT_SPACE = 5;
        public const int PKT_SPACE = 26; //26 max
        public const string DEVICES_JSON_FILENAME = @"devices.json";

        public byte Sequence = 1;

        UdpClient UdpClient;

        List<NetworkInfo> ReceivedPackets = new List<NetworkInfo>();

        CancellationTokenSource ReceivingPacketsCancellation = new CancellationTokenSource();
        CancellationTokenSource SendingPacketsCancellation = new CancellationTokenSource();
        CancellationTokenSource SendingDiscoveryPacketsCancellation = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            //NetworkInfo info = new NetworkInfo("", 0, new LifxPacket(Pkt_Type.GetService, true));
            //Product prod = new Product("", 1, 1, 1, 1);
            //Device dev1 = new Device(info, prod);

            //SaveFoundDevicesToFileAsync(new List<Device>() { dev1 });
            List<Device> devices = await ReadSavedDevicesFromFileAsync();


            //Console.WriteLine(product);
            //LifxPacket testPacket = new LifxPacket(target: new byte[] { 0xD0, 0x73, 0xD5, 0x2D, 0x8D, 0xA2, 0x00, 0x00 },
            //                                       pkt_type: Pkt_Type.SetPower, 
            //                                       payload: new byte[2] { 0xFF, 0xFF },
            //                                       ack_required: true);

            //Decoder.PrintFields(new byte[] { 0x24, 0x00, 0x00, 0x34, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00 });

            Lan lan = new Lan();

            //lan.SendPacket(testPacket, new IPEndPoint(IPAddress.Parse("192.168.10.25"), DEFAULT_PORT), printMessages: true);
            //lan.ReceivePacket(printMessages: true);

            //List<NetworkInfo> networkInfos = lan.Discovery(ONE_SECOND * 1, 5, 5, true);
            //lan.GetProductInfo(networkInfos);
            //Console.WriteLine(new Product(1, 30, 3, 90));
            lan.StartReceivingPacketsAsync();
            //lan.StartSendingDiscoveryPacketsAsync();

            /*Console.WriteLine("Press enter to stop sending discovery packets");
            Console.ReadLine();

            lan.StopSendingDiscoveryPackets();

            List<string> deviceIPs = new List<string>();
            List<Device> devices = new List<Device>();

            foreach (NetworkInfo netinfo in lan.ReceivedPackets.ToList())
            {
                if (netinfo.Packet.ProtocolHeader.Pkt_Type == Pkt_Type.StateService && !deviceIPs.Contains(netinfo.Address))
                {
                    Device device = await lan.CreateDeviceFromStateServiceAsync(netinfo);
                    Console.WriteLine($"{device}\n");
                    devices.Add(device);
                    deviceIPs.Add(netinfo.Address.ToString());
                }
            }

            SaveFoundDevicesToFileAsync(devices);*/
            foreach (Device dev in devices)
            {
                Console.WriteLine(dev.Product.Label);
                try
                {
                    Console.WriteLine(new LightState(await lan.SendToDeviceThenReceiveAsync(dev, Pkt_Type.GetColor, new byte[] { })));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Still receiving packets, press enter to exit");
            Console.ReadLine(); 

            lan.StopReceivingPackets();
            await Task.Delay(ONE_SECOND);

            //Decoder.PrintFields(pkt.ToBytes());
            //Decoder.PrintFields(lan.ReceivedPackets[0].Packet.ToBytes());



            /*List<Device> savedDevices = await ReadSavedDevicesFromFileAsync();

            for (int i = 0; i < savedDevices.Count; i++)
            {
                Console.WriteLine(savedDevices[i].Equals(devices[i]));
            }*/
            //devices.ForEach(Console.WriteLine);*/

            Console.ReadLine();
        }

        public async Task<byte[]> SendToDeviceThenReceiveAsync(Device device, Pkt_Type pkt_type, byte[] payload, int waits = 10)
        {
            LifxPacket pkt = new LifxPacket(device.NetworkInfo.Packet.FrameAddress.Target, pkt_type, payload);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(device.NetworkInfo.Address), device.NetworkInfo.Port);
            await SendPacketAsync(pkt, ipep, SendingPacketsCancellation.Token);

            List<NetworkInfo> responses = new List<NetworkInfo>();
            do
            {
                await Task.Delay(ONE_SECOND / 2);
                responses = MatchReplyToRequest(pkt.FrameHeader.Source, pkt.FrameAddress.Sequence, pkt.FrameAddress.Target);
                waits--;
            }
            while (waits > 0 && responses.Count == 0);

            if (waits <= 0)
                throw new TimeoutException("No response received");

            return responses[0].Packet.Payload.ToBytes();
        }

        public async Task<byte[]> SendToDeviceThenReceiveAsync(Device device, Pkt_Type pkt_type, int waits = 10)
        {
            return await SendToDeviceThenReceiveAsync(device, pkt_type, new byte[0], waits);
        }

        public Lan()
        {
            try
            {
                UdpClient = new UdpClient(DEFAULT_PORT);
                UdpClient.EnableBroadcast = true;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Console.WriteLine($"Port {DEFAULT_PORT} already in use by a different program, close it and try again.\n");
                }
                throw;
            }
        }

        public async void StartReceivingPacketsAsync(bool ignoreGetService = false)
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
                    NetworkInfo networkInfo;
                    if (receivedPacket.ProtocolHeader.Pkt_Type == Pkt_Type.StateService) //not sure if this is actually needed, but better to be safe
                        networkInfo = new NetworkInfo(receivedResult.RemoteEndPoint.Address.ToString(), (int)new StateService(receivedPacket.Payload.ToBytes()).Port, receivedPacket);
                    else
                        networkInfo = new NetworkInfo(receivedResult.RemoteEndPoint.Address.ToString(), receivedResult.RemoteEndPoint.Port, receivedPacket);
                    if (!(ignoreGetService && receivedPacket.ProtocolHeader.Pkt_Type == Pkt_Type.GetService))
                    {
                        Debug.WriteLine($"Received {receivedResult.RemoteEndPoint.Address,-ADDR_SPACE}:{receivedResult.RemoteEndPoint.Port,-PORT_SPACE} {receivedPacket.ProtocolHeader.Pkt_Type,-PKT_SPACE} {BitConverter.ToString(receivedResult.Buffer)}");
                        ReceivedPackets.Add(networkInfo);
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

        public async void StartSendingDiscoveryPacketsAsync(int delay = ONE_SECOND * 1)
        {
            CancellationToken token = SendingDiscoveryPacketsCancellation.Token;
            IPEndPoint broadcastIpEndPoint = new IPEndPoint(GetBroadcastAddress(GetLocalIP(), GetSubnetMask(GetLocalIP())), DEFAULT_PORT);
            LifxPacket discoveryPacket = new LifxPacket(Pkt_Type.GetService, true);

            Debug.WriteLine("Starting to send discovery packets...");
            while (!token.IsCancellationRequested)
            {
                await SendPacketAsync(discoveryPacket, broadcastIpEndPoint, token);
                await Task.Delay(delay);
            }
        }

        public void StopSendingDiscoveryPackets()
        {
            Debug.WriteLine("Stop sending discovery packets");
            SendingDiscoveryPacketsCancellation.Cancel();
        }

        public async Task<Device> CreateDeviceFromStateServiceAsync(NetworkInfo networkInfo, int retries = 5)
        {
            if (networkInfo.Packet.ProtocolHeader.Pkt_Type != Pkt_Type.StateService)
                throw new ArgumentException($"Specified packet was not a StateService, received: {networkInfo.Packet.ProtocolHeader.Pkt_Type}");
            
            LifxPacket getLabelPacket           = new LifxPacket(networkInfo.Packet.FrameAddress.Target, Pkt_Type.GetLabel);
            LifxPacket getVersionPacket         = new LifxPacket(networkInfo.Packet.FrameAddress.Target, Pkt_Type.GetVersion);
            LifxPacket getHostFirmwarePacket    = new LifxPacket(networkInfo.Packet.FrameAddress.Target, Pkt_Type.GetHostFirmware);

            NetworkInfo? getLabelResponse = null;
            NetworkInfo? getVersionResponse = null;
            NetworkInfo? getHostFirmwareResponse = null;

            do
            {
                if (getLabelResponse == null) await SendPacketAsync(getLabelPacket, new IPEndPoint(IPAddress.Parse(networkInfo.Address), networkInfo.Port), default);
                if (getVersionResponse == null) await SendPacketAsync(getVersionPacket, new IPEndPoint(IPAddress.Parse(networkInfo.Address), networkInfo.Port), default);
                if (getHostFirmwareResponse == null) await SendPacketAsync(getHostFirmwarePacket, new IPEndPoint(IPAddress.Parse(networkInfo.Address), networkInfo.Port), default);

                //todo add a proper delay
                await Task.Delay(ONE_SECOND / 2);

                getLabelResponse = MatchReplyToRequest(getLabelPacket.FrameHeader.Source, getLabelPacket.FrameAddress.Sequence, getLabelPacket.FrameAddress.Target).FirstOrDefault();
                getVersionResponse = MatchReplyToRequest(getVersionPacket.FrameHeader.Source, getVersionPacket.FrameAddress.Sequence, getVersionPacket.FrameAddress.Target).FirstOrDefault();
                getHostFirmwareResponse = MatchReplyToRequest(getHostFirmwarePacket.FrameHeader.Source, getHostFirmwarePacket.FrameAddress.Sequence, getHostFirmwarePacket.FrameAddress.Target).FirstOrDefault();
                retries--;
            }
            while (retries > 0 && (getLabelResponse == null || getVersionResponse == null || getHostFirmwareResponse == null));

            if (retries <= 0)
            {
                string errorMsg = "";
                if (getLabelResponse == null) errorMsg += "No response from GetLabel\n";
                if (getLabelResponse == null) errorMsg += "No response from GetVersion\n";
                if (getLabelResponse == null) errorMsg += "No response from GetHostFirmware\n";
                errorMsg += $"on {BitConverter.ToString(networkInfo.Packet.FrameAddress.Target)}";
                throw new Exception(errorMsg);
            }

            StateLabel stateLabel               = new StateLabel(getLabelResponse!.Packet.Payload.ToBytes());
            StateVersion stateVersion           = new StateVersion(getVersionResponse!.Packet.Payload.ToBytes());
            StateHostFirmware stateHostFirmware = new StateHostFirmware(getHostFirmwareResponse!.Packet.Payload.ToBytes());

            Product product = new Product(stateLabel.Label, (int)stateVersion.Vendor, (int)stateVersion.Product, stateHostFirmware.Version_Major, stateHostFirmware.Version_Minor);
            Device device = new Device(networkInfo, product);
            return device;
        }

        public static async void SaveFoundDevicesToFileAsync(List<Device> devices)
        {
            using FileStream createStream = File.Create(DEVICES_JSON_FILENAME);
            await JsonSerializer.SerializeAsync(createStream, devices, new JsonSerializerOptions { WriteIndented = true });
            await createStream.DisposeAsync();

            //string json = JsonSerializer.Serialize(devices, new JsonSerializerOptions { WriteIndented = true });

            //Console.WriteLine(json);

            //List<Device> newDevices = JsonSerializer.Deserialize<List<Device>>(json)!;
            //Console.WriteLine(newDevices[0]);
            //Console.WriteLine(devices[0].Equals(newDevices[0]));
        }

        public static async Task<List<Device>> ReadSavedDevicesFromFileAsync()
        {
            using FileStream readStream = File.OpenRead(DEVICES_JSON_FILENAME);
            List<Device>? newDevices = await JsonSerializer.DeserializeAsync<List<Device>>(readStream);
            await readStream.DisposeAsync();
            return newDevices!;
        }

        public List<NetworkInfo> MatchDiscoveryReplyToRequest(UInt32 source, byte sequence)
        {
            List<NetworkInfo> matchingReplies = new List<NetworkInfo>(); //sometimes there are multiple replies

            foreach (NetworkInfo response in ReceivedPackets)
            {
                if (response.Packet.FrameHeader.Source == source &&
                    response.Packet.FrameAddress.Sequence == sequence &&
                    response.Packet.ProtocolHeader.Pkt_Type == Pkt_Type.StateService)
                {
                    matchingReplies.Add(response);
                }
            }
            return matchingReplies;
        }

        public List<NetworkInfo> MatchReplyToRequest(UInt32 source, byte sequence, byte[] target)
        {
            if (target.Length != 8)
                throw new ArgumentException($"Serial number must be of length 8, given: {BitConverter.ToString(target)}");

            List<NetworkInfo> matchingReplies = new List<NetworkInfo>(); //sometimes there are multiple replies

            foreach (NetworkInfo response in ReceivedPackets)
            {
                if (response.Packet.FrameHeader.Source == source && 
                    response.Packet.FrameAddress.Sequence == sequence && 
                    response.Packet.FrameAddress.Target.SequenceEqual(target))
                {
                    matchingReplies.Add(response);
                }
            }
            return matchingReplies;
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
        /*public List<NetworkInfo> Discovery(int timeoutPerRun = ONE_SECOND, int maxRuns = 5, int numDevices = 0, bool printMessages = false)
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
                SendPacket(discoveryPacket, new IPEndPoint(GetBroadcastAddress(GetLocalIP(), GetSubnetMask(GetLocalIP())), DEFAULT_PORT));
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

                        if (receivedPacket.ProtocolHeader.Pkt_Type == Pkt_Type.StateService)
                        {
                            //Decoder.PrintFields(receivedBytes);
                            StateService stateService = new StateService(receivedPacket.Payload.Bytes);
                            //Console.WriteLine(stateService.Service);
                            //Console.WriteLine(stateService.Port);

                            NetworkInfo newNetworkInfo = new NetworkInfo(RemoteIpEndPoint.Address.ToString(), (int)stateService.Port, receivedPacket);

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
        }*/

        /*public void GetProductInfo(List<NetworkInfo> networkInfos)
        {
            foreach (NetworkInfo networkInfo in networkInfos)
            {
                LifxPacket getVersionPacket = new LifxPacket(networkInfo.Packet.FrameAddress.Target, Pkt_Type.GetVersion);
                SendPacket(getVersionPacket, new IPEndPoint(IPAddress.Parse(networkInfo.Address), networkInfo.Port));
                //ReceivePacket();
            }
        }*/

        /// <summary>
        /// Sends the specified <see cref="LifxPacket"/> to the specified ip on the specified port.
        /// </summary>
        /// <param name="packet">The <see cref="LifxPacket"/> to send to the device</param>
        /// <param name="addr">The ip address and port to send our packet to</param>
        /// <returns>The number of bytes sent</returns>
        /*public int SendPacket(LifxPacket packet, IPEndPoint addr)
        {
            unchecked { packet.FrameAddress.Sequence = Sequence++; }
            if (Decoder.IsValid(packet.ToBytes())) //no point sending useless data that might damage our devices
            {
                try
                {
                    int bytesSent = UdpClient.Send(packet.ToBytes(), packet.ToBytes().Length, addr);
                    Debug.WriteLine($"Sent     {addr.Address,-ADDR_SPACE}:{addr.Port,-PORT_SPACE} {packet.ProtocolHeader.Pkt_Type,-26} {BitConverter.ToString(packet.ToBytes())}");
                    //Decoder.PrintFields(packet.ToBytes());

                    return bytesSent;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return 0;
                }
            }
            return 0;
        }*/

        public async Task<int> SendPacketAsync(LifxPacket packet, IPEndPoint addr, CancellationToken cancellationToken)
        {
            //if (!typeof(ISendable).IsAssignableFrom(packet.Payload.GetType()))
            //    throw new ArgumentException($"Payload type {packet.Payload.GetType()} cannot be sent");

            unchecked { packet.FrameAddress.Sequence = Sequence++; }
            if (Decoder.IsValid(packet.ToBytes())) //no point sending useless data that might damage our devices
            {
                try
                {
                    int bytesSent = await UdpClient.SendAsync(packet.ToBytes(), addr, cancellationToken);
                    Debug.WriteLine($"Sent     {addr.Address,-ADDR_SPACE}:{addr.Port,-PORT_SPACE} {packet.ProtocolHeader.Pkt_Type,-PKT_SPACE} {BitConverter.ToString(packet.ToBytes())}");
                    //Decoder.PrintFields(packet.ToBytes());

                    return bytesSent;
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine($"Not sent {addr.Address,-ADDR_SPACE}:{addr.Port,-PORT_SPACE} {packet.ProtocolHeader.Pkt_Type,-PKT_SPACE} {BitConverter.ToString(packet.ToBytes())}");
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">The port number we are waiting for a response on</param>
        /// <returns></returns>
        /*public LifxPacket ReceivePacket(bool printMessages = false)
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
        }*/
    }
}