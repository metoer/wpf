using Hytera.EEMS.Common;
using Hytera.EEMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Gather.Lib
{
    public class DeviceInfoHelper
    {
        /// <summary>
        /// 修改执法记录仪注册信息
        /// </summary>
        /// <param name="deviceRegisterInfo"></param>
        public static void DeviceRegisterInfo(DeviceRegisterInfo deviceRegisterInfo)
        {
            DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(deviceRegisterInfo.DeviceCode ?? String.Empty));
            if (deviveInfo != null)
            {
                deviveInfo.IsMatchOrRegist = IsMatchOrRegist.Registered;
                deviveInfo.DeviceID = (deviceRegisterInfo.DeviceID ?? string.Empty).ToString();
            }
        }

        /// <summary>
        /// 执法记录仪信息更新或者添加
        /// </summary>
        /// <param name="deviceInfos"></param>
        public static void DeviceInfoAddOrUpdate(DeviceInfos deviceInfos)
        {
            foreach (var item in deviceInfos.DeviveInfoList)
            {
                DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(item.DeviceCode));
                if (deviveInfo == null)
                {
                    // 添加
                    GatherViewModel.DeviveInfoList.AddItem(item);
                }
                else
                {
                    // 更新
                    item.ValueCloneToObject(deviveInfo);
                }
            }
        }

        /// <summary>
        /// 执法记录仪移除
        /// </summary>
        /// <param name="deviceCode"></param>
        public static void DevieItemRemove(string deviceCode)
        {
            DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(deviceCode));
            if (deviveInfo != null)
            {
                GatherViewModel.DeviveInfoList.Remove(deviveInfo);

                MatchWindow matchWindow = WindowsHelper.GetWindow<MatchWindow>();
                if (matchWindow != null && matchWindow.DeviveInfo.DeviceCode.Equals(deviceCode))
                {
                    matchWindow.Close();
                }

                DeviceRegisterWindow registerWindow = WindowsHelper.GetWindow<DeviceRegisterWindow>();
                if (registerWindow != null && registerWindow.DeviveInfo.DeviceCode.Equals(deviceCode))
                {
                    registerWindow.Close();
                }

                HandCollectWindow handCollectWindow = WindowsHelper.GetWindow<HandCollectWindow>();
                if (handCollectWindow != null && handCollectWindow.DeviveInfo.DeviceCode.Equals(deviceCode))
                {
                    handCollectWindow.Close();
                }
            }
        }

        /// <summary>
        /// 清除所有的执法记录仪
        /// </summary>
        public static void DeviceItemClear()
        {
            GatherViewModel.DeviveInfoList.Clear();

            MatchWindow matchWindow = WindowsHelper.GetWindow<MatchWindow>();
            if (matchWindow != null)
            {
                matchWindow.Close();
            }

            DeviceRegisterWindow registerWindow = WindowsHelper.GetWindow<DeviceRegisterWindow>();
            if (registerWindow != null)
            {
                registerWindow.Close();
            }
        }

        /// <summary>
        /// 执法记录仪更新记录仪电量、采集进度、采集状态更新
        /// </summary>
        /// <param name="deviceInfos"></param>
        public static void DeviceInfoUpdate(UpdateDeviceInfos deviceInfos)
        {
            foreach (var item in deviceInfos.DeviveInfoList)
            {
                DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(item.DeviceCode));
                if (deviveInfo != null)
                {
                    // 更新
                    item.ValueCloneToObjectExceptNull(deviveInfo);
                }
            }
        }

        /// <summary>
        /// 更新配对信息
        /// </summary>
        /// <param name="devicePairInfo"></param>
        public static void DevicePairInfo(DevicePairInfo devicePairInfo)
        {
            DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(devicePairInfo.DeviceID));
            if (deviveInfo != null)
            {
                deviveInfo.MatchUserID = devicePairInfo.UserID;
                deviveInfo.MatchUserName = devicePairInfo.UserName;
                deviveInfo.OrgID = devicePairInfo.OrgID;
                deviveInfo.MatchUserCode = devicePairInfo.UserCode;
                deviveInfo.MatchOrgName = devicePairInfo.OrgName;
                deviveInfo.MatchTime = devicePairInfo.MatchTime;
                deviveInfo.IsMatchOrRegist = IsMatchOrRegist.Matched;
            }
        }

        /// <summary>
        /// 更新执法记录仪采集状态
        /// </summary>
        /// <param name="devID"></param>
        /// <param name="deviceState"></param>
        public static void DeviceCollectInfo(string devID, DeviceState deviceState)
        {
            DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(devID));
            if (deviveInfo != null)
            {
                deviveInfo.DeviceState = deviceState;
            }
        }

        /// <summary>
        /// 取消执法记录仪配对
        /// </summary>
        /// <param name="deviceCode"></param>
        public static void DeviceCancelPair(string deviceCode)
        {
            DeviveInfo deviveInfo = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(deviceCode));
            if (deviveInfo != null)
            {
                deviveInfo.MatchUserID = string.Empty;
                deviveInfo.MatchUserName = string.Empty;
                deviveInfo.OrgID = string.Empty;
                deviveInfo.MatchUserCode = string.Empty;
                deviveInfo.MatchOrgName = string.Empty;
                deviveInfo.MatchTime = string.Empty;
                deviveInfo.IsMatchOrRegist = IsMatchOrRegist.Registered;
            }
        }
    }
}
