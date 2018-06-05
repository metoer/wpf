using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Hytera.EEMS.Main.Lib
{
    public class ThemesHelper
    {
        // 记录是否第一次初始化
        private static bool IsInit = false;

        /// <summary>
        /// 初始化默认资源
        /// </summary>
        public static void InitResource()
        {
            List<string> listDefaultResource = new List<string>();

            ResourceDictionary rd = new ResourceDictionary();

            listDefaultResource.Add("/Hytera.EEMS.Resources;Component/Styles/Commom/Default.xaml");

            listDefaultResource.Add("/Hytera.EEMS.Resources;Component/Styles/Commom/MainStyles.xaml");

            listDefaultResource.Add("/Hytera.EEMS.Resources;Component/Styles/Commom/ManageStyles.xaml");

            foreach (var item in listDefaultResource)
            {
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(item, UriKind.RelativeOrAbsolute) });
            }
        }

        #region  切换语言
        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="LanguageId"></param>
        public static void SetLanguage(string LanguageId)
        {
            string file = string.Empty;

            foreach (var item in AppConfigInfos.LanguageList)
            {
                if (item.ID.Equals(LanguageId))
                {
                    if (item.IsChecked && IsInit)
                    {
                        return;
                    }

                    file = item.FileName;

                    item.IsChecked = true;
                    IsInit = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }

            if (!File.Exists(file))
            {
                return;
            }

            Dictionary<string, string> languageResoures = GetResouresByFile(file);

            ReplaceAppResources(languageResoures);
        }

        /// <summary>
        /// 获取语言文件中的资源
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetResouresByFile(string file)
        {
            Dictionary<string, string> languageResoures = new Dictionary<string, string>();
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("language File Not Found");
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(file);

            XmlNodeList xmlNodes = doc.SelectNodes("PluginLanguage/Plugin");
            if (xmlNodes != null)
            {
                foreach (XmlNode node in xmlNodes)
                {
                    if (((XmlElement)node).GetAttribute("Name").Equals("Language"))
                    {
                        continue;
                    }

                    foreach (XmlElement item in node.ChildNodes)
                    {
                        string id = item.GetAttribute("ID").ToString();
                        string value = item.GetAttribute("Value").ToString();
                        if (!languageResoures.ContainsKey(id))
                        {
                            languageResoures.Add(id, value);
                        }
                    }
                }
            }

            return languageResoures;
        }

        /// <summary>
        /// 替换当前app的资源
        /// </summary>
        /// <param name="languageResoures"></param>
        private static void ReplaceAppResources(Dictionary<string, string> languageResoures)
        {
            ResourceDictionary resourceDictionary = Application.Current.Resources;
            foreach (var item in languageResoures.Keys)
            {
                if (resourceDictionary[item] is FontFamily)
                {
                    resourceDictionary[item] = new FontFamily(languageResoures[item]);
                }
                else
                {
                    resourceDictionary[item] = languageResoures[item];
                }
            }
        }
        #endregion

        #region 切换皮肤
        /// <summary>
        /// 初始化皮肤数据，为实现皮肤切换功能
        /// </summary>
        public static void InitSkinData()
        {
            AppConfigInfos.CurrentSkinName = "Default";
            AppConfigInfos.Skins.Add("Default", new List<string>() { "/Hytera.EEMS.Resources;Component/Styles/Skins/Default/Default.xaml" });

        }

        /// <summary>
        /// 切换皮肤
        /// </summary>
        /// <param name="skinName"></param>
        public static void SetSkin(string skinName)
        {
            var rd = Application.Current.Resources.MergedDictionaries;

            // 寻找当前皮肤的资源
            List<ResourceDictionary> result = (from t in rd
                                               where Array.Exists(AppConfigInfos.Skins[AppConfigInfos.CurrentSkinName].ToArray(), p => p.Equals(t.Source.ToString()))
                                               select t).ToList();
            // 移除当前皮肤资源
            result.Clear();

            // 添加需要替换的皮肤资源
            foreach (var item in AppConfigInfos.Skins[skinName])
            {
                rd.Add(new ResourceDictionary() { Source = new Uri(item, UriKind.RelativeOrAbsolute) });
            }
        }
        #endregion
    }
}
