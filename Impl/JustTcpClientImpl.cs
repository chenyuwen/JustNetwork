using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// TCP传输类
    /// </summary>
    class JustTcpClientImpl : JustClientInterface
    {
        private bool isBreak = false;

        /// <summary>
        /// 初始化,并打开客户端
        /// </summary>
        public void OpenClient(JustAdapter adapter)
        {
            //throw new NotImplementedException();
            Console.WriteLine("InitClient");
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <returns></returns>
        public byte[] LowLevelRecv(JustAdapter adapter)
        {
            Console.WriteLine("接收到一个数据包");
            while (true)
            {
                Thread.Sleep(1000);
                if(isBreak)
                {
                    break;
                }
            }
            return null;
        }


        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="args"></param>
        public void LowLevelSend(JustAdapter adapter, JustArgs args)
        {
            Console.WriteLine("发送一个数据包");

            TcpClient tcpSocket = new TcpClient();
            byte[] buffer = null;

            adapter.LastEventType = JustEventType.Successful;
            try
            {
                buffer = args.buffer;

                tcpSocket.SendTimeout = adapter.SendTimeout;
                tcpSocket.ReceiveTimeout = adapter.ReceiveTimeout;
                tcpSocket.Connect(adapter.RemoteAddress, adapter.RemotePort);
                tcpSocket.GetStream().Write(buffer, 0, buffer.Length);
                tcpSocket.GetStream().Flush();
            }
            catch(TimeoutException e)
            {
                Console.WriteLine(e);
                adapter.LastEventType = JustEventType.Timeout;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                adapter.LastEventType = JustEventType.Unknow;
            }
            finally
            {
                try
                {
                    tcpSocket.Close();
                    Console.WriteLine("关闭连接");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void StopClient(JustAdapter adapter)
        {
            //argsQueue.Clear();
            Console.WriteLine("StopClient");
        }


    }
}
