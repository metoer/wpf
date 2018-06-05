using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hytera.EEMS.Common.Http
{
    /// <summary>
    /// 封装Http请求数据的2个接口
    /// </summary>
    public class HttpBaseRequestHepler
    {
        /// <summary>
        /// HttpGet
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResultInfo RequestHttpByGet(HttpUrlHelper url)
        {
            HttpItem item = new HttpItem();
            item.URL = url.ToString();
            item.Accept = url.Accept;
            item.Method = "GET";
            item.ContentType = url.ContentType;
            HttpResultInfo result = new HttpResponseHelper().GetHtml(item).HttpJsonToResult();
            return result;
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResultInfo RequestHttpByPost(HttpUrlHelper url)
        {
            HttpItem item = new HttpItem();
            item.Timeout = 10000;
            item.URL = url.GetUrl();
            item.Postdata = url.GetParamsEncodeString();
            item.Accept = url.Accept;
            item.ContentType = url.ContentType;

            HttpResultInfo result = new HttpResponseHelper().GetHtml(item).HttpJsonToResult();
            return result;
        }
    }
}
