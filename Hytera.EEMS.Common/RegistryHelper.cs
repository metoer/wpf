using Microsoft.Win32;

namespace Hytera.EEMS.Common
{
    public class RegistryHelper
    {
        /// <summary>
        /// 获取注册表键值下面对应项的值
        /// </summary>
        /// <param name="registerUrl"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValueByRegistryKey(string registerUrl, string name)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registerUrl))
            {
                object value = key.GetValue(name);
                return (value ?? string.Empty).ToString();
            }
        }

        /// <summary>
        /// 设置注册表中指定的值
        /// </summary>
        /// <param name="registerUrl"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetValueForRegisterKey(string registerUrl, string name, string value)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registerUrl))
            {
                key.SetValue(name, value);
            }
        }
    }
}
