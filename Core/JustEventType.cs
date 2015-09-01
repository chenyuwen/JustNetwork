using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    public enum JustEventType : byte
    {
        /// <summary>
        /// 超时
        /// </summary>
        Timeout,

        /// <summary>
        /// 未知错误
        /// </summary>
        Unknow,

        /// <summary>
        /// 发送或者接收成功
        /// </summary>
        Successful,

        /// <summary>
        /// 没有发现
        /// </summary>
        Notfound,
    }
}
