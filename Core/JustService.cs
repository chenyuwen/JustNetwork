using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventEditor.JustNetwork
{
    public class JustService : ServiceInterface
    {
        private JustAdapter mAdapter = null;

        /// <summary>
        /// 需要发送的信息的队列
        /// </summary>
        private Queue<JustArgs> argsQueue = new Queue<JustArgs>();

        /// <summary>
        /// 发送线程
        /// </summary>
        private Thread mSendThread = null;

        /// <summary>
        /// 发送信号
        /// </summary>
        private AutoResetEvent mSendEvent = new AutoResetEvent(false);

        /// <summary>
        /// 接收线程
        /// </summary>
        private Thread mRecvThread = null;

        public void GetServiceName()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 当服务开启的时候执行
        /// </summary>
        public void OnServiceStart()
        {
            JustAdapter adapter = this.mAdapter;

            if (adapter != null)
            {
                adapter.mJustClientInterface.OpenClient(adapter);
                adapter.mJustEventInterface.OnOpenEvent(adapter);
                mSendEvent.Reset();
            }
            
        }

        /// <summary>
        /// 当服务关闭的时候执行
        /// </summary>
        public void OnServiceStop()
        {
            JustAdapter adapter = this.mAdapter;

            if(adapter != null)
            {
                adapter.mJustClientInterface.StopClient(adapter);
                //关闭线程
                StopServiceThread();

                adapter = null;
            }

            
        }

        /// <summary>
        /// 设置Adapter
        /// </summary>
        /// <param name="adapter">适配器</param>
        public void SetAdapter(JustAdapter adapter)
        {
            this.mAdapter = adapter;

            OnServiceStart();
        }

        /// <summary>
        /// 发送Just数据包
        /// </summary>
        /// <param name="args"></param>
        public void SendJust(JustArgs args)
        {
            JustAdapter adapter = this.mAdapter;

            if(adapter == null)
            {
                throw new JustException("");
            }

            argsQueue.Enqueue(args);
            StartServiceThread();

            this.mSendEvent.Set();
        }

        /// <summary>
        /// 关闭线程
        /// </summary>
        private void StopServiceThread()
        {
            if (this.mSendThread != null)
            {
                mSendThread.Abort();
                mSendThread = null;
            }

            if (this.mRecvThread != null)
            {
                mRecvThread.Abort();
                mRecvThread = null;
            }
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        private void StartServiceThread()
        {
            if (this.mRecvThread == null)
            {
                mRecvThread = new Thread(new ThreadStart(JustServiceRecvThread));
                mRecvThread.IsBackground = true;
                mRecvThread.Start();
            }

            if (this.mSendThread == null)
            {
                mSendThread = new Thread(new ThreadStart(JustServiceSendThread));
                mSendThread.IsBackground = true;
                mSendThread.Start();
            }
        }

        /// <summary>
        /// 接收线程
        /// </summary>
        private void JustServiceRecvThread()
        {
            byte[] buffer = null;
            JustAdapter adapter = this.mAdapter;
            while(true)
            {
                buffer = adapter.mJustClientInterface.LowLevelRecv(mAdapter);

                JustEventInterface JustEvent = adapter.mJustEventInterface;
                if (JustEvent != null)
                {
                    JustEvent.OnRecvEvent(adapter, buffer);

                    //TODO：根据返回值，决定是否需要继续接收
                }
            }
        }

        /// <summary>
        /// 发送线程
        /// </summary>
        private void JustServiceSendThread()
        {
            JustArgs args = null;
            JustAdapter adapter = this.mAdapter;
            while (true)
            {
                Queue<JustArgs> argsQueue = this.argsQueue;

                args = null;
                if (argsQueue.Count > 0)
                {
                    args = argsQueue.Dequeue();
                }

                if (args == null)
                {
                    mSendEvent.WaitOne();
                    continue;
                    //args = argsQueue.Dequeue();
                }

                adapter.mJustClientInterface.LowLevelSend(mAdapter, args);

                JustEventInterface JustEvent = adapter.mJustEventInterface;
                if (JustEvent != null)
                {
                    JustEvent.OnSendEvent(adapter, args);
                }
            }
        }

        /// <summary>
        /// 发送Just数据包
        /// </summary>
        /// <param name="just">数据包</param>
        public void SendJust(Just.Just just)
        {
            JustArgs args = new JustArgs();
            args.buffer = just.get();
            this.SendJust(args);
        }

        /// <summary>
        /// 发送Just数据包
        /// </summary>
        /// <param name="just">数据包</param>
        public void SendJust(Just.Just just, object obj)
        {
            JustArgs args = new JustArgs();
            args.buffer = just.get();
            args.TAG = obj;
            this.SendJust(args);
        }

        /// <summary>
        /// 发送一个buffer
        /// </summary>
        /// <param name="buffer">数据包</param>
        public void SendBuffer(byte[] buffer)
        {
            JustArgs args = new JustArgs();
            args.buffer = buffer;
            this.SendJust(args);
        }

    }
}
