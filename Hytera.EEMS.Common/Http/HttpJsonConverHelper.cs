using System;

namespace Hytera.EEMS.Common.Http
{
    /// <summary>
    /// http请求数据转换类
    /// </summary>
    internal static class HttpJsonConverHelper
    {
        /// <summary> 
        /// Http请求结果数据转换成 HttpResultInfo 对象(JSON第一层包装)
        /// </summary>
        /// <param name="httpResult"></param>
        /// <returns></returns>
        public static HttpResultInfo HttpJsonToResult(this HttpResult httpResult)
        {
            HttpResultInfo result = new HttpResultInfo();
            try
            {
                if (httpResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = JsonUnityConvert.DeserializeObject<HttpResultInfo>(httpResult.Html);
                }
                else
                {
                    result.Msg = httpResult.StatusDescription;
                    result.Data = httpResult.Html;
                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.ToString();
                result.Data = "数据解析出错";
            }

            return result;
        }
    }
}
