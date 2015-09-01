using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 发送的信息的一个包装
    /// </summary>
    public class JustArgs
    {
        /// <summary>
        /// 需要发送的数据
        /// </summary>
        public byte[] buffer = null;

        /// <summary>
        /// 额外的数据
        /// </summary>
        public object TAG = null;
    }
}
