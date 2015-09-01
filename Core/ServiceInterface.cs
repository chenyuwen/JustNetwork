using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface ServiceInterface
    {
        /// <summary>
        /// 服务开启的时候
        /// </summary>
        void OnServiceStart();

        /// <summary>
        /// 服务关闭的时候
        /// </summary>
        void OnServiceStop();

        /// <summary>
        /// 获取服务的名字
        /// </summary>
        void GetServiceName();
    }
}
