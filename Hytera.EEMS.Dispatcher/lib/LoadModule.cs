using Hytera.EEMS.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hytera.EEMS.Dispatcher
{
    internal class LoadModule
    {
        /// <summary>
        /// 加载插件模块
        /// </summary>
        /// <param name="plugPath">插件路径</param>
        /// <param name="paras">初始化参数</param>
        /// <returns>模块对象</returns>
        internal static List<ModuleBaseEntry> LoadPlugModule(string plugPath, DelegateAppSelfMessageNotic delegateAction, params object[] paras)
        {
            List<ModuleBaseEntry> modules = new List<ModuleBaseEntry>();

            try
            {
                List<Assembly> assemblyList = GetAllAssemblies(plugPath);

                foreach (var item in assemblyList)
                {
                    Type[] types = item.GetTypes();
                    foreach (var t in types)
                    {
                        if (IsEEMSModule(t))
                        {
                            ModuleBaseEntry plugPanel = (ModuleBaseEntry)Activator.CreateInstance(t, paras);
                            plugPanel.SelfMessageNotic = delegateAction;
                            modules.Add(plugPanel);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg("Module Init:" + e.Message);
            }

            modules.Sort();

            return modules;
        }

        /// <summary>
        /// 获取bin下全部dll
        /// </summary>
        private static List<Assembly> GetAllAssemblies(string dirPath)
        {
            List<Assembly> assemblyList = new List<Assembly>();

            if (!Directory.Exists(dirPath))
            {
                return assemblyList;
            }

            var dllFiles = Directory.GetFiles(dirPath, "*.dll", SearchOption.TopDirectoryOnly).ToList();

            assemblyList = dllFiles.Select(p => Assembly.LoadFile(p)).ToList();

            return assemblyList;
        }


        /// <summary>
        /// 判断与EEMSModule的Types是否有关
        /// </summary>
        private static bool IsEEMSModule(Type type)
        {
            return
                type.IsClass &&
                typeof(ModuleBaseEntry).IsAssignableFrom(type);
        }

    }
}
