using Newtonsoft.Json;

namespace Hytera.EEMS.Common
{
    public class JsonUnityConvert
    {
        // sting 转换为对象
        public static T DeserializeObject<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 对象转换string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
