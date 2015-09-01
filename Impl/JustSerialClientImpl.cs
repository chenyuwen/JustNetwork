
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 串口传输类
    /// </summary>
    class JustSerialClientImpl : JustClientInterface
    {
        private int COM_BAUDRATE = 115200;

        /// <summary>
        /// 初始化,并打开客户端
        /// </summary>
        public void OpenClient(JustAdapter adapter)
        {
            //throw new NotImplementedException();
            Console.WriteLine("InitClient");
            //SerialPort port = new SerialPort();



            SerialPort port = null;
            adapter.LastEventType = JustEventType.Successful;
            try
            {
                port = new SerialPort(adapter.RemoteAddress, COM_BAUDRATE, Parity.None, 8);
                port.Open();
            }
            catch (Exception e)
            {
                adapter.LastEventType = JustEventType.Notfound;
                Console.WriteLine(e);
            }

            adapter.TAG = port;
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <returns></returns>
        public byte[] LowLevelRecv(JustAdapter adapter)
        {
            Console.WriteLine("接收到一个数据包");
            SerialPort port = adapter.TAG as SerialPort;


            return null;
        }


        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="args"></param>
        public void LowLevelSend(JustAdapter adapter, JustArgs args)
        {
            Console.WriteLine("发送一个数据包");
            SerialPort port = adapter.TAG as SerialPort;

        }

        public void StopClient(JustAdapter adapter)
        {
            //argsQueue.Clear();
            Console.WriteLine("StopClient");

            SerialPort port = adapter.TAG as SerialPort;
            port.Close();
        }


    }
}
