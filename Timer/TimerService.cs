using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 定时器服务，提供统一的定时器操作
    /// </summary>
    public class TimerService : ServiceInterface
    {
        private Action<TimerService> mAction = null;

        private Timer timer = null;

        private int mPeriod = 0;

        public void GetServiceName()
        {
            
        }

        /// <summary>
        /// 服务开启的时候执行
        /// </summary>
        public void OnServiceStart()
        {
            if(mAction != null && timer == null)
            {
                timer = new Timer(new TimerCallback(TimeoutHandler), null, 0, 0);
            }
        }

        /// <summary>
        /// 服务关闭的时候执行
        /// </summary>
        public void OnServiceStop()
        {
            Timer timer = this.timer;
            if(timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        /// <summary>
        /// 重置定时器
        /// </summary>
        private void ResetTimer()
        {
            timer.Change(0, mPeriod);
        }

        /// <summary>
        /// 超时处理
        /// </summary>
        /// <param name="obj"></param>
        private void TimeoutHandler(object obj)
        {
            Action<TimerService> action = this.mAction;

            if (action != null)
            {
                action(this);
            }
        }

        /// <summary>
        /// 定时到达的时候
        /// </summary>
        /// <param name="action"></param>
        public void SetTimeoutAction(Action<TimerService> action)
        {
            this.mAction = action;
        }

        /// <summary>
        /// 设置定时值
        /// </summary>
        /// <param name="period">定时值，单位为毫秒</param>
        public void SetPeriod(int period)
        {
            this.mPeriod = period;
        }
    }
}
