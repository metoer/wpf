
namespace Hytera.EEMS.Ice
{
    public class MessagePackage
    {
        public MessagePackage(object msgType, object data)
        {
            MsgType = msgType;
            Data = data;
        }

        public object MsgType
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }
    }
}
