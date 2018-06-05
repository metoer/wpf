using Hytera.EEMS.Common;
using System.Collections.Generic;
using System.Linq;

namespace Hytera.EEMS.Manage.Lib
{
    public class MediaUrl
    {
        /// <summary>
        /// 生成VLC媒体播放地址
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string GetMediaUrl(Dictionary<string, string> conditions, string password, string iv)
        {
            conditions = (from item in conditions orderby item.Key select item).ToDictionary(p => p.Key, v => v.Value);

            string paras = string.Empty;

            foreach (string key in conditions.Keys)
            {
                paras += "&" + key + "=" + conditions[key];
            }

            if (!string.IsNullOrEmpty(paras))
            {
                paras = paras.Substring(1);
            }

            string md5Sign = MD5Helper.MD5Encrypt(paras).ToUpper();

            conditions.Add("sign", md5Sign);

            string json = JsonUnityConvert.SerializeObject(conditions);

            return AESHelper.CBCEncrypt(json, password, iv).Replace("+", "-").Replace("/", "_");
        }
    }
}
