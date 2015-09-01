using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface JustClientInterface
    {
        /// <summary>
        /// 初始化,并打开客户端
        /// </summary>
        /// <param name="adapter">适配器</param>
        void OpenClient(JustAdapter adapter);

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="adapter">适配器</param>
        /// <param name="args"></param>
        void LowLevelSend(JustAdapter adapter, JustArgs args);

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="adapter">适配器</param>
        /// <returns>接收到的数据</returns>
        byte[] LowLevelRecv(JustAdapter adapter);

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="adapter">适配器</param>
        void StopClient(JustAdapter adapter);

        /// <summary>
        /// 设置适配器
        /// </summary>
        /// <param name="adapter"></param>
        //void SetAdapter(JustAdapter adapter);
    }
}
