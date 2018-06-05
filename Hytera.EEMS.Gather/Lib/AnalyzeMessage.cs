using Hytera.EEMS.Common;
using Hytera.EEMS.Gather.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Xml;

namespace Hytera.EEMS.Gather
{
    public partial class ModelResponsible
    {

        /// <summary>
        /// 执法记录仪设备信息
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeDeviceInfos(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceInfos deviceInfos = XmlUnityConvert.XmlDeserialize<DeviceInfos>(value);

                if (deviceInfos != null && deviceInfos.DeviveInfoList != null)
                {
                    DeviceInfoHelper.DeviceInfoAddOrUpdate(deviceInfos);
                }
            }));
        }

        /// <summary>
        /// 执法记录仪设备更新
        /// </summary>

        public void AnalyzeDeviceUpdateState(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                UpdateDeviceInfos deviceInfos = XmlUnityConvert.XmlDeserialize<UpdateDeviceInfos>(value);
                if (deviceInfos != null && deviceInfos.DeviveInfoList != null)
                {
                    DeviceInfoHelper.DeviceInfoUpdate(deviceInfos);
                }

            }));
        }

        /// <summary>
        /// 执法记录仪注册结果
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="value"></param>
        private void AnalyzeDeviceRegisterResult(MsgType msgType, string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceRegisterInfo deviceRegisterInfo = XmlUnityConvert.XmlDeserialize<DeviceRegisterInfo>(value);
                ResultWindow resultWindow = CheckResultMsg(msgType);
                if (resultWindow == null)
                {
                    return;
                }

                if (deviceRegisterInfo.ResultCode == 0)
                {
                    resultWindow.SuccessCloseWindow();
                    DeviceInfoHelper.DeviceRegisterInfo(deviceRegisterInfo);
                }
                else
                {
                    resultWindow.FailedCloseWindow(deviceRegisterInfo);
                }

            }));
        }

        /// <summary>
        /// 配对信息返回结果
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="value"></param>
        private void AnalyzeDevicePairResult(MsgType msgType, string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DevicePairInfo devicePairInfo = XmlUnityConvert.XmlDeserialize<DevicePairInfo>(value);
                ResultWindow resultWindow = CheckResultMsg(msgType);
                if (resultWindow == null)
                {
                    return;
                }

                if (devicePairInfo.ResultCode == 0)
                {
                    resultWindow.SuccessCloseWindow();
                    if (msgType == MsgType.DevicePairResult)
                    {
                        DeviceInfoHelper.DevicePairInfo(devicePairInfo);
                    }
                    else
                    {
                        // 取消配对
                        DeviceInfoHelper.DeviceCancelPair(devicePairInfo.DeviceCode ?? string.Empty);
                    }
                }
                else
                {
                    resultWindow.FailedCloseWindow(devicePairInfo);
                }
            }));
        }

        /// <summary>
        /// 点击开始、停止采集操作结果
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="value">信息内容</param>
        /// <param name="deviceState">更新执法记录仪采集状态</param>
        private void AnalyzeDeviceCollectResult(MsgType msgType, string value, DeviceState deviceState)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceCollectInfo deviceCollectInfo = XmlUnityConvert.XmlDeserialize<DeviceCollectInfo>(value);
                ResultWindow resultWindow = CheckResultMsg(msgType);
                if (resultWindow == null)
                {
                    return;
                }

                if (deviceCollectInfo.ResultCode == 0)
                {
                    resultWindow.SuccessCloseWindow();
                    DeviceInfoHelper.DeviceCollectInfo(deviceCollectInfo.DeviceID, deviceState);
                }
                else
                {
                    resultWindow.FailedCloseWindow(deviceCollectInfo);
                }
            }));
        }

        /// <summary>
        /// 执法记录仪移除
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeDeviceInfosRemove(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
             {
                 XmlDocument xmlDoc = new XmlDocument();

                 xmlDoc.LoadXml(value);

                 XmlNode node = xmlDoc.SelectSingleNode("DeviveInfo");
                 if (node != null)
                 {
                     string devieCode = node.Attributes["ID"].Value;

                     DeviceInfoHelper.DevieItemRemove(devieCode);
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
