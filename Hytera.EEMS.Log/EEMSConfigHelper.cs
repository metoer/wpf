
using System;
using System.IO;
using System.Xml;

namespace Hytera.EEMS.Log
{
    public class EEMSConfigHelper
    {
        /// <summary>
        /// 公共配置文件
        /// </summary>
        static string commomConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SmartEEMS\Config\appconfig.xml";
        /// <summary>
        /// 获取公共配置中指定值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValueByCommomConfig(string path, string defaultValue)
        {
            try
            {
                if (!File.Exists(commomConfigPath))
                {
                    return defaultValue;
                }

                XmlDocument doc = new XmlDocument();

                doc.Load(commomConfigPath);


                XmlElement xn = (XmlElement)doc.SelectSingleNode(path);
                if (xn == null)
                {
                    return defaultValue;
                }
                return xn.InnerText;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }
    }
}
