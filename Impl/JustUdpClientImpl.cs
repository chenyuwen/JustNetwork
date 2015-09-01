using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// UDP传输类
    /// </summary>
    class JustUdpClientImpl : JustClientInterface
    {

        /// <summary>
        /// 初始化,并打开客户端
        /// </summary>
        public void OpenClient(JustAdapter adapter)
        {
            //throw new NotImplementedException();
            Console.WriteLine("InitClient");

            IPEndPoint endport = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpSocket = new UdpClient(endport);

            //设置超时时间
            udpSocket.Client.ReceiveTimeout = adapter.ReceiveTimeout;
            udpSocket.Client.SendTimeout = adapter.SendTimeout;

            adapter.TAG = udpSocket;
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <returns></returns>
        public byte[] LowLevelRecv(JustAdapter adapter)
        {
            Console.WriteLine("接收到一个数据包");

            byte[] buffer = null;
            IPEndPoint endport = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpSocket = adapter.TAG as UdpClient;

            adapter.LastEventType = JustEventType.Successful;
            try
            {
                buffer = udpSocket.Receive(ref endport);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e);
                adapter.LastEventType = JustEventType.Timeout;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                adapter.LastEventType = JustEventType.Unknow;
            }
            return buffer;
        }


        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="args"></param>
        public void LowLevelSend(JustAdapter adapter, JustArgs args)
        {
            Console.WriteLine("发送一个数据包");
            //Thread.Sleep(1000);

            UdpClient udpSocket = adapter.TAG as UdpClient;

            byte[] buf = args.buffer;

            adapter.LastEventType = JustEventType.Successful;
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

        /// <summary>
        /// 关闭
        /// </summary>
        public void StopClient(JustAdapter adapter)
        {
            //argsQueue.Clear();
            Console.WriteLine("StopClient");

            UdpClient udpSocket = adapter.TAG as UdpClient;
            udpSocket.Close();
        }
    }
}
