using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Dispatcher
{
    public class SelfMessageEventArgs : EventArgs
    {
        public SelfMessageEventArgs(AppSelfMsgType msgType, string message)
        {
            MsgType = msgType;
            Message = message;
        }

        public AppSelfMsgType MsgType
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            set;
        }
    }
}
