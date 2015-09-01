using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 适配器
    /// </summary>
    public class JustAdapter
    {
        /// <summary>
        /// 发送接收类
        /// </summary>
        public JustClientInterface mJustClientInterface = null;

        /// <summary>
        /// 事件处理类
        /// </summary>
        public JustEventInterface mJustEventInterface = null;

        /// <summary>
        /// 远程的地址
        /// </summary>
        public string RemoteAddress = "";

        /// <summary>
        /// 远程的端口
        /// </summary>
        public int RemotePort = 0;

        /// <summary>
        /// 本地端口
        /// </summary>
        public int LocalPort = 0;

        /// <summary>
        /// 超时时间，单位为毫秒
        /// </summary>
        public int ReceiveTimeout = 0;

        /// <summary>
        /// 发送超时时间
        /// </summary>
        public int SendTimeout = 0;

        /// <summary>
        /// 上一个事件的错误代码
        /// </summary>
        public JustEventType LastEventType = JustEventType.Unknow;

        /// <summary>
        /// 另外的参数
        /// </summary>
        public object TAG;

        public JustAdapter(JustClientInterface mJustClientInterface, JustEventInterface mJustEventInterface)
        {
            this.mJustClientInterface = mJustClientInterface;
            this.mJustEventInterface = mJustEventInterface;
            this.TAG = null;
        }

        public JustAdapter(JustClientInterface mJustClientInterface, JustEventInterface mJustEventInterface,
            object TAG) : this(mJustClientInterface, mJustEventInterface)
        {
            this.TAG = TAG;
        }
    }
}
