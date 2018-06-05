using System.Collections.Generic;
using System.Web;

namespace Hytera.EEMS.Common.Http
{
    /// <summary>
    /// 组装Http参数
    /// </summary>
    public class HttpUrlHelper
    {
        private string address = string.Empty;

        private string action = string.Empty;

        private Dictionary<string, object> paraList = new Dictionary<string, object>();

        public string Accept = "application/json, text/javascript, */*; q=0.01";

        public string ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

        public string HeaderAcceptEncoding = "Accept-Encoding:gzip,deflate";

        public string HeaderAcceptLanguage = "Accept-Language:zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3";

        public HttpUrlHelper(string action, string address)
        {
            this.action = action;
            this.address = address;
        }

        public void AddPara(string key, object name)
        {
            paraList.Add(key, name);
        }

        /// <summary>
        /// 获取URL + GET参数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string url = GetUrl();
            url = (url.EndsWith("?") ? url : url + "?") + GetParamsEncodeString();

            return url;
        }

        /// <summary>
        /// 获取URL
        /// </summary>
        /// <returns></returns>
        public string GetUrl()
        {
            string url = address + action;
            return url;
        }

        /// <summary>
        /// 获取拼装参数
        /// </summary>
        /// <returns></returns>
        public string GetParamsString()
        {
            List<string> list = new List<string>();
            foreach (var key in paraList.Keys)
            {
                list.Add(string.Format("{0}={1}", key, paraList[key]));
            }

            return string.Join("&", list.ToArray());
        }

        /// <summary>
        /// 获取拼装参数
        /// </summary>
        /// <returns></returns>
        public string GetParamsEncodeString()
        {
            List<string> list = new List<string>();
            foreach (var key in paraList.Keys)
            {
                list.Add(string.Format("{0}={1}", key, HttpUtility.UrlEncode((paraList[key] ?? string.Empty).ToString())));
            }

            return string.Join("&", list.ToArray());
        }

        public string GetParamsJson()
        {
            string result = string.Empty;
            //Newtonsoft.
            return null;
        }
    }
}
