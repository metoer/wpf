using Hytera.EEMS.Model;
using System;

namespace Hytera.EEMS.Dispatcher
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(MsgType msgType, string message)
        {
            MsgType = msgType;
            Message = message;
        }

        public MsgType MsgType
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

    public class UnknownMessageEventArgs : EventArgs
    {
        public UnknownMessageEventArgs(string msgType, string message)
        {
            MsgType = msgType;
            Message = message;
        }

        public string MsgType
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
