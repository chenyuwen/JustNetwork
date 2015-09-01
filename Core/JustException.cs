using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEditor.JustNetwork
{
    /// <summary>
    /// 错误类
    /// </summary>
    public class JustException : Exception
    {
        /*
        private JustEventInterface mJustEventInterface = null;

        private JustClientInterface mJustClientInterface = null;

        private JustArgs mJustArgs = null;

        private JustEventType type = JustEventType.Unknow;

        public JustException(JustEventInterface mJustEventInterface, JustClientInterface mJustClientInterface, 
            JustArgs mJustArgs, JustEventType type)
        {
            
        }*/

        public JustException()
        { }

        public JustException(string msg)
        {
            //this.
        }
    }
}
