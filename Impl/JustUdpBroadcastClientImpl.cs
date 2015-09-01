using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// UDP传输类
    /// </summary>
    class JustUdpBroadcastClientImpl : JustClientInterface
    {
        private bool isStartRecv = true;

        private AutoResetEvent recvEvent = new AutoResetEvent(true);

        private Queue<JustUdpBroadcastMessage> messages = new Queue<JustUdpBroadcastMessage>();

        /// <summary>
        /// 初始化,并打开客户端
        /// </summary>
        public void OpenClient(JustAdapter adapter)
        {
            List<UdpClient> sockets = new List<UdpClient>();
            //throw new NotImplementedException();
            Console.WriteLine("InitClient");

            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();

            recvEvent.Reset();
            foreach (NetworkInterface net in networks)
            {
                IPInterfaceProperties IPInterfaceProperties = net.GetIPProperties();
                UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                {
                    if (UnicastIPAddressInformation.Address.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
                    {
                        Console.WriteLine("ipaddr = " + UnicastIPAddressInformation.Address.ToString());

                        IPEndPoint endport = new IPEndPoint(UnicastIPAddressInformation.Address, 0);
                        UdpClient udpSocket = new UdpClient(endport);

                        //设置超时时间
                        udpSocket.Client.ReceiveTimeout = adapter.ReceiveTimeout;
                        udpSocket.Client.SendTimeout = adapter.SendTimeout;

                        sockets.Add(udpSocket);

                        //开启接收
                        udpSocket.BeginReceive(ReceiveCallback, udpSocket);
                    }
                }
            }

            adapter.TAG = sockets;
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <returns></returns>
        public byte[] LowLevelRecv(JustAdapter adapter)
        {
            //Console.WriteLine("接收到一个数据包");

            List<UdpClient> sockets = adapter.TAG as List<UdpClient>;

            byte[] buffer = null;

            JustUdpBroadcastMessage msg = null;

            adapter.LastEventType = JustEventType.Successful;

            if (messages.Count > 0)
            {
                msg = this.messages.Dequeue();
                this.recvEvent.Reset();
            }
            else
            {
                this.recvEvent.WaitOne();
                msg = this.messages.Dequeue();
            }

            if(msg != null)
            {
                adapter.LastEventType = msg.Type;
                buffer = msg.buffer;
            }

            return buffer;
        }

        /// <summary>
        /// 接收线程回掉
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Console.WriteLine("ReceiveCallback");

            UdpClient udpSocket = (UdpClient)ar.AsyncState;

            JustUdpBroadcastMessage msg = new JustUdpBroadcastMessage();
            IPEndPoint endport = new IPEndPoint(IPAddress.Any, 0);
            UdpClient socket = (UdpClient)ar.AsyncState;

            msg.Type = JustEventType.Successful;
            try
            {
                //msg.buffer = udpSocket.Receive(ref endport);
                msg.buffer = udpSocket.EndReceive(ar, ref endport);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e);
                msg.Type = JustEventType.Timeout;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                msg.Type = JustEventType.Unknow;
            }
            finally
            {
                this.messages.Enqueue(msg);
                recvEvent.Set();
                Console.WriteLine("收到包");
                if (isStartRecv)
                {
                    udpSocket.BeginReceive(ReceiveCallback, udpSocket);
                }
            }
        }


        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="args"></param>
        public void LowLevelSend(JustAdapter adapter, JustArgs args)
        {
            //Console.WriteLine("发送一个数据包");
            //Thread.Sleep(1000);

            List<UdpClient> sockets = adapter.TAG  as List<UdpClient>;

            byte[] buf = args.buffer;

            adapter.LastEventType = JustEventType.Successful;
            foreach (UdpClient udpSocket in sockets)
            {
                try
                {
                    udpSocket.Send(buf, buf.Length, adapter.RemoteAddress, adapter.RemotePort);
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine(e);
                    adapter.LastEventType = JustEventType.Timeout;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    adapter.LastEventType = JustEventType.Unknow;
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void StopClient(JustAdapter adapter)
        {
            //argsQueue.Clear();
            Console.WriteLine("StopClient");

            List<UdpClient> sockets = adapter.TAG as List<UdpClient>;
            isStartRecv = false;
            foreach (UdpClient udpSocket in sockets)
            {
                udpSocket.Close();
            }
        }

        internal class JustUdpBroadcastMessage
        {
            public byte[] buffer;
            public JustEventType Type;
        }
    }
}
