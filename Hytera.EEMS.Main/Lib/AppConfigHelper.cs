using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Hytera.EEMS.Main.Lib
{
    public class AppConfigHelper
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string appConfigPath = AppDomain.CurrentDomain.BaseDirectory + "Config\\AppConfig.xml";

        /// <summary>
        /// 加载appconfig信息
        /// </summary>
        public static void InitAppConfig()
        {
            try
            {
                if (!File.Exists(appConfigPath))
                {
                    return;
                }

                XmlDocument doc = new XmlDocument();

                doc.Load(appConfigPath);

                AppConfigInfos.LanguageList = GetLanguageInfo(doc);

                GetSetInfos(doc, "AppConfig/SetInfo");

                GetSetInfos(doc, "AppConfig/BasicInfo");

                GetPortPairInfos(doc);
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
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
                // EEMS当前选中的语言
                string checkValue = string.Empty;

                XmlElement xn = (XmlElement)doc.SelectSingleNode("AppConfig/Language");


                if (xn != null)
                {
                    checkValue = xn.GetAttribute("Checked");
                }

                // 读取指定文件夹下的文件
                string file = AppDomain.CurrentDomain.BaseDirectory + "Language\\";
                if (!Directory.Exists(file))
                {
                    return LanguageList;
                }

                string[] languageFiles = Directory.GetFiles(file);
                foreach (var item in languageFiles)
                {
                    try
                    {
                        XmlDocument lanDoc = new XmlDocument();
                        lanDoc.Load(item);

                        XmlElement xmlElement = (XmlElement)lanDoc.SelectSingleNode("PluginLanguage");

                        foreach (XmlElement element in xmlElement.ChildNodes)
                        {
                            string id = element.GetAttribute("Name").ToString();
                            if (id.Equals("Language"))
                            {
                                string languageId = string.Empty;
                                string displayName = string.Empty;                               
                                foreach (XmlElement childElement in element.ChildNodes)
                                {
                                    string itemID = childElement.GetAttribute("ID").ToString();
                                    string value = childElement.GetAttribute("Value").ToString();
                                    if (itemID.Equals("Language"))
                                    {
                                        languageId = childElement.GetAttribute("Value").ToString();
                                    }
                                    else if (itemID.Equals("Display"))
                                    {
                                        displayName = childElement.GetAttribute("Value").ToString();
                                    }
                                }

                                bool isCheck = string.IsNullOrEmpty(checkValue) ? false : checkValue.Equals(languageId);

                                LanguageList.Add(new LanguageInfos() { ID = languageId, DisplayName = displayName, FileName = item, IsChecked = isCheck });
                            }
                        } 
                    }
                    catch (Exception ex)
                    {
                        // 保证后面语言文件读取正常
                        LogHelper.Instance.WirteErrorMsg("EEMS Language Error" + ex.Message);
                    }
                }

                // 如果没有选择使用哪种语言，默认选择第一个
                if (string.IsNullOrEmpty(checkValue) && languageFiles.Length > 0)
                {
                    LanguageList[0].IsChecked = true;
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg("EEMS AppConfig Error" + e.Message);
            }

            return LanguageList;
        }


        /// <summary>
        /// 读取设置信息
        /// </summary>
        /// <param name="doc"></param>
        private static void GetSetInfos(XmlDocument doc, string nodePath)
        {
            try
            {
                Type type = typeof(AppStateInfos);
                PropertyInfo[] infos = type.GetProperties();

                XmlElement xn = (XmlElement)doc.SelectSingleNode(nodePath);
                if (xn != null)
                {
                    foreach (var item in xn.ChildNodes)
                    {
                        XmlElement node = item as XmlElement;
                        if (node == null)
                        {
                            continue;
                        }

                        string id = node.GetAttribute("ID").ToString();
                        string value = node.GetAttribute("Value").ToString();

                        PropertyInfo propertyInfo = infos.FirstOrDefault(p => p.Name.Equals(id));
                        if (propertyInfo == null)
                        {
                            continue;

                        }

                        if (propertyInfo.PropertyType.Name.Equals("Int32"))
                        {
                            propertyInfo.SetValue(AppConfigInfos.AppStateInfos, Convert.ToInt32(value));
                        }
                        else
                        {
                            propertyInfo.SetValue(AppConfigInfos.AppStateInfos, value);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        /// <summary>
        /// 获取端口设置信息
        /// </summary>
        /// <param name="doc"></param>
        private static void GetPortPairInfos(XmlDocument doc)
        {
            try
            {
                XmlElement xn = (XmlElement)doc.SelectSingleNode("AppConfig/PortPair");
                if (xn != null)
                {
                    string firstPortIndex = xn.GetAttribute("FirstPortIndex").ToString();
                    string firstPortCode = xn.GetAttribute("FirstPortCode").ToString();

                    AppConfigInfos.PortDeviceList.FirstPort = firstPortIndex;
                    AppConfigInfos.PortDeviceList.FirstPortCode = firstPortCode;

                    foreach (var item in xn.ChildNodes)
                    {
                        XmlElement node = item as XmlElement;
                        if (node == null)
                        {
                            continue;
                        }

                        string id = node.GetAttribute("ID").ToString();
                        string value = node.GetAttribute("Value").ToString();
                        AppConfigInfos.PortPairInfos.Add(new PortPairInfo() { Index = id, PortCode = value });
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public static void SaveInfoToFile(string path)
        {
            try
            {
                if (!File.Exists(appConfigPath))
                {
                    return;
                }

                Type type = typeof(AppStateInfos);
                PropertyInfo[] infos = type.GetProperties();

                XmlDocument doc = new XmlDocument();

                doc.Load(appConfigPath);

                XmlElement xn = (XmlElement)doc.SelectSingleNode("AppConfig/SetInfo");
                if (xn != null)
                {
                    foreach (XmlElement node in xn.ChildNodes)
                    {
                        string id = node.GetAttribute("ID").ToString();
                        PropertyInfo item = infos.First(p => p.Name.Equals(id));
                        if (item != null)
                        {
                            node.SetAttribute("Value", item.GetValue(AppConfigInfos.AppStateInfos).ToString());
                        }
                    }
                }

                doc.Save(appConfigPath);
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        /// <summary>
        /// 保存当前选中的语言文件
        /// </summary>
        /// <param name="languageID"></param>
        public static void SaveLanguageInfo(string languageID)
        {
            try
            {
                if (!File.Exists(appConfigPath))
                {
                    return;
                }

                XmlDocument doc = new XmlDocument();

                doc.Load(appConfigPath);

                XmlElement xn = (XmlElement)doc.SelectSingleNode("AppConfig/Language");

                if (xn != null)
                {
                    xn.SetAttribute("Checked", languageID);
                }

                doc.Save(appConfigPath);
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        /// <summary>
        /// 保存端口配置信息
        /// </summary>
        public static void SavePortPairInfos()
        {
            try
            {
                if (!File.Exists(appConfigPath))
                {
                    return;
                }

                XmlDocument doc = new XmlDocument();

                doc.Load(appConfigPath);

                XmlNode xn = CheckNode(doc);

                foreach (var item in AppConfigInfos.PortPairInfos)
                {
                    XmlElement xmlElement = doc.CreateElement("Item");
                    xmlElement.SetAttribute("ID", item.Index);
                    xmlElement.SetAttribute("Value", item.PortCode);
                    xn.AppendChild(xmlElement);
                }

                doc.Save(appConfigPath);
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        /// <summary>
        /// 保存优先端口
        /// </summary>
        public static void SaveFirstPort()
        {
            try
            {
                if (!File.Exists(appConfigPath))
                {
                    return;
                }

                XmlDocument doc = new XmlDocument();

                doc.Load(appConfigPath);

                XmlElement xn = (XmlElement)CheckNode(doc, false);

                xn.SetAttribute("FirstPortIndex", AppConfigInfos.PortDeviceList.FirstPort);
                xn.SetAttribute("FirstPortCode", AppConfigInfos.PortDeviceList.FirstPortCode);

                doc.Save(appConfigPath);
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        private static XmlNode CheckNode(XmlDocument doc, bool isClear = true)
        {
            XmlNode xmlParent = doc.SelectSingleNode("AppConfig");

            XmlNode xn = doc.SelectSingleNode("AppConfig/PortPair");
            if (xn == null)
            {
                xn = doc.CreateNode(XmlNodeType.Element, "PortPair", null);
                xmlParent.AppendChild(xn);
            }

            if (isClear)
            {
                xn.RemoveAll();
            }

            return xn;
        }
    }
}
