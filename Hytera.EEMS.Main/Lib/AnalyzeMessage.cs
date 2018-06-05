using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Main.Lib
{
    public partial class MainMessage
    {


        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="value"></param>
        public void AnalyzePcState(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                PcState serveState = XmlUnityConvert.XmlDeserialize<PcState>(value);
                AppConfigInfos.AppStateInfos.DataBaseState = serveState.DataBaseState;
                AppConfigInfos.AppStateInfos.ServerState = serveState.ServerState;
                AppConfigInfos.AppStateInfos.ServerIp = serveState.ServerIp;
                AppConfigInfos.AppStateInfos.MemoryPath = serveState.MemoryPath;
                AppConfigInfos.AppStateInfos.StationID = serveState.StationID;
                AppConfigInfos.AppStateInfos.StationCode = serveState.StationCode;
                AppConfigInfos.AppStateInfos.ServerMsgCode = serveState.ServerMsg;
                AppConfigInfos.AppStateInfos.DataBaseMsgCode = serveState.DataBaseMsg;
            }));
        }

        /// <summary>
        /// 解析账号验证信息,分为输入账号验证或者指纹直接验证
        /// </summary>
        public void AnalyzeAccountValidate(MsgType msgType, string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
              {
                  UserResult userResult = XmlUnityConvert.XmlDeserialize<UserResult>(value);

                  if (userResult.UserInfoFrom == UserInfoFrom.Login)
                  {
                      ResultWindow resultWindow = CheckResultMsg(msgType);
                      if (resultWindow == null)
                      {
                          return;
                      }

                      resultWindow.ResultValue = userResult;

                      if (userResult.UserResultCode.Equals("0"))
                      {
                          resultWindow.SuccessCloseWindow();
                      }
                      else
                      {
                          resultWindow.FailedCloseWindow(userResult);
                      }
                  }
                  else
                  {
                      LoginWindow loginWindow = WindowsHelper.GetWindow<LoginWindow>();
                      if (userResult == null || loginWindow == null || loginWindow.PermissionID != userResult.PermissionID)
                      {
                          // 过滤
                          return;
                      }

                      loginWindow.ReciveMsg(userResult);
                  }
              }));
        }

        /// <summary>
        /// 解析数据刷新
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="value"></param>
        public void AnalyzeDataReferesh(MsgType msgType, string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DataRefreshResult dataRefreshResult = XmlUnityConvert.XmlDeserialize<DataRefreshResult>(value);
                ResultWindow resultWindow = CheckResultMsg(msgType);
                if (resultWindow == null)
                {
                    return;
                }

                if (dataRefreshResult.DataCode == ResultCode.Success)
                {
                    resultWindow.SuccessCloseWindow();
                }
                else
                {
                    resultWindow.FailedCloseWindow(dataRefreshResult);
                }
            }));
        }

        /// <summary>
        /// 解析数据刷新进程
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="value"></param>
        public void AnalyzeDataProgress(MsgType msgType, string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DataRefreshResult dataRefreshResult = XmlUnityConvert.XmlDeserialize<DataRefreshResult>(value);

                RefershWindow refershWindow = WindowsHelper.GetWindow<RefershWindow>();

                if (refershWindow != null)
                {
                    refershWindow.UpdateProgress(dataRefreshResult);
                }
            }));
        }

        /// <summary>
        /// 解析license信息
        /// </summary>
        /// <param name="value"></param>
        public void AnalyzeLicenseInfo(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                LicenseInfo licenseInfo = XmlUnityConvert.XmlDeserialize<LicenseInfo>(value);
                AppConfigInfos.LicenseInfo = licenseInfo;
            }));
        }

        /// <summary>
        /// 解析采集端口信息
        /// </summary>
        /// <param name="value"></param>
        public void AnalyzePortInfo(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                PortInfos portInfos = XmlUnityConvert.XmlDeserialize<PortInfos>(value);

                AppConfigInfos.PortDeviceList.PortevList.Clear();
                AppConfigInfos.PortDeviceList.PortevList.Add(new PortIsDevice() { PortName = "-----", IsDeviceInfo = false }); // 添加一个空白
                if (portInfos != null && portInfos.PortList != null)
                {
                    foreach (var item in portInfos.PortList)
                    {
                        AppConfigInfos.PortDeviceList.PortevList.Add(new PortIsDevice() { PortName = item, IsDeviceInfo = false });
                    }

                    AppConfigInfos.PortDeviceList.IsChanged = !AppConfigInfos.PortDeviceList.IsChanged;
                }
            }));
        }

        /// <summary>
        /// 解析设置优先端口
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="value"></param>
        public void AnalyzeSetFirstPort(MsgType msgType, string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                FirstPortResult firstPortResult = XmlUnityConvert.XmlDeserialize<FirstPortResult>(value);
                ResultWindow resultWindow = CheckResultMsg(msgType);
                if (resultWindow == null)
                {
                    return;
                }

                if (firstPortResult.ResultCode == ResultCode.Success)
                {
                    resultWindow.SuccessCloseWindow();
                }
                else
                {
                    resultWindow.FailedCloseWindow(firstPortResult);
                }
            }));
        }

        /// <summary>
        /// 解析执法记录仪类型
        /// </summary>
        /// <param name="value"></param>
        public void AnalyzeDeviceTypes(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceTypes deviceTypes = XmlUnityConvert.XmlDeserialize<DeviceTypes>(value);

                if (deviceTypes != null && deviceTypes.DeviceTypeList != null)
                {
                    AppConfigInfos.DeviceTypeList = deviceTypes.DeviceTypeList;
                }
            }));
        }

        /// <summary>
        /// 检测消息是否过时
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private ResultWindow CheckResultMsg(MsgType msgType)
        {
            ResultWindow resultWindow = WindowsHelper.GetWindow<ResultWindow>();
            if (resultWindow != null && resultWindow.ReceiveMsgType == msgType)
            {
                return resultWindow;
            }
            else
            {
                return null;
            }
        }
    }
}
