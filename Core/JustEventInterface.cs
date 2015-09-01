using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 事件描述
    /// </summary>
    public interface JustEventInterface
    {
        //void OnTimeOut();

        /// <summary>
        /// 发生任何事件都会调用
        /// </summary>
        void OnRecvEvent(JustAdapter adapter, byte[] recvBuffer);

        /// <summary>
        /// 发生任何事件都会调用
        /// </summary>
        void OnSendEvent(JustAdapter adapter, JustArgs args);

        /// <summary>
        /// 发送成功
        /// </summary>
        void OnOpenEvent(JustAdapter adapter);

        /// <summary>
        /// 
        /// </summary>
        //void OnConnectError();
    }
}
