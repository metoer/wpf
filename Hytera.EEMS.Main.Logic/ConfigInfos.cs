using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Hytera.EEMS.Main.Logic
{
    public class ConfigInfos
    {
        /// <summary>
        /// 加载appconfig信息
        /// </summary>
        public static void InitAppConfig()
        {
            try
            {
                string appConfigPath = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\AppConfig.xml";
                if (!File.Exists(appConfigPath))
                {
                    return;
                }

                XmlDocument doc = new XmlDocument();

                doc.Load(appConfigPath);

                AppConfigInfos.LanguageList = GetLanguageInfo(doc);
            }
            catch (Exception e)
            {
                LogHelper.Log(e.Message);
            }
        }

        /// <summary>
        /// 读取语言设置项
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static List<LanguageInfos> GetLanguageInfo(XmlDocument doc)
        {            
            List<LanguageInfos> LanguageList = new List<LanguageInfos>();
            try
            {
                XmlElement xn = (XmlElement)doc.SelectSingleNode("AppConfig/Language");
                if (xn != null)
                {
                    foreach (XmlElement node in xn.ChildNodes)
                    {
                        string id = node.GetAttribute("ID").ToString();
                        string displayName = node.GetAttribute("DisplayName").ToString();
                        string fileName = node.GetAttribute("File").ToString();

                        LanguageList.Add(new LanguageInfos() { ID = id, DisplayName = displayName, FileName = fileName });
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e.Message);
            }

            return LanguageList;

        }
    }
}
