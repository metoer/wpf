
namespace Hytera.EEMS.Common.Http
{
    /// <summary>
    /// Http返回结构
    /// </summary>
    public class HttpResultInfo
    {
        public object Data
        {
            get;
            set;
        }

        public string DetailMsg
        {
            get;
            set;
        }

        public bool Flag
        {
            get;
            set;
        }

        public string Msg
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }
    }
}
