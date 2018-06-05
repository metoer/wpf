using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Fingerprint.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Fingerprint
{
    public partial class ModelResponsible
    {
        /// <summary>
        /// 警员信息内容解析
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzePoliceInfos(string value, MsgType msgType)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
              {
                  FingerInfos policeInfos = XmlUnityConvert.XmlDeserialize<FingerInfos>(value);

                  policeInfos = FilterFingerInfoByUserType(policeInfos);

                  ResultWindow resultWindow = CheckResultMsg(msgType);
                  if (resultWindow == null)
                  {
                      return;
                  }

                  if (policeInfos.ResultCode == 0)
                  {
                      resultWindow.SuccessCloseWindow();

                      FingerViewModel.PageIndex = policeInfos.PageIndex;
                      FingerViewModel.IsMoreData = policeInfos.UserInfoList.Count == AppConfigInfos.AppStateInfos.SearchPageCount;
                      if (policeInfos.PageIndex == 1)
                      {
                          FingerViewModel.SearchPoliceInfos(policeInfos.UserInfoList);
                      }
                      else
                      {
                          FingerViewModel.AddPoliceInfos(policeInfos.UserInfoList);
                      }
                  }
                  else
                  {
                      resultWindow.FailedCloseWindow(policeInfos);
                  }
              }));
        }

        private FingerInfos FilterFingerInfoByUserType(FingerInfos policeInfos)
        {
            if (AppConfigInfos.CurrentUserInfos.UserType.Equals("2"))
            {
                for (int i = policeInfos.UserInfoList.Count - 1; i >= 0; i--)
                {
                    if (!policeInfos.UserInfoList[i].UserID.Equals(AppConfigInfos.CurrentUserInfos.UserID))
                    {
                        policeInfos.UserInfoList.RemoveAt(i);
                    }
                }
            }

            return policeInfos;

        }

        /// <summary>
        /// 指纹采集结果
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeCollectFingerPrint(string value)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                FingerResult fingerResult = XmlUnityConvert.XmlDeserialize<FingerResult>(value);

                CollectFingerWindow collectFingerWindow = WindowsHelper.GetWindow<CollectFingerWindow>();
                if (collectFingerWindow == null || !(collectFingerWindow.CurrentUser.UserID.Equals(fingerResult.UserID)))
                {
                    return;
                }

                collectFingerWindow.FingerImage = fingerResult.FingerImage;
                collectFingerWindow.FingerStatus = fingerResult.ResultCode == 0 ? FingerStatus.RecordSuccess : FingerStatus.RecordFail;

            }));
        }

        /// <summary>
        /// 指纹编辑保存结果
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeEditorFingerPrint(string value, MsgType msgType)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                FingerResult fingerResult = XmlUnityConvert.XmlDeserialize<FingerResult>(value);
                ResultWindow resultWindow = CheckResultMsg(msgType);
                if (resultWindow == null)
                {
                    return;
                }

                if (fingerResult.ResultCode == 0)
                {
                    resultWindow.SuccessCloseWindow();
                    FingerViewModel.EditorFingerPrintByUserID(fingerResult);
                }
                else
                {
                    resultWindow.FailedCloseWindow(fingerResult);
                }
            }));
        }

        /// <summary>
        /// 采集指纹保存
        /// </summary>
        /// <param name="value"></param>
        /// <param name="msgType"></param>
        private void AnalyzeSaveFingerPrint(string value, MsgType msgType)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
             {
                 FingerResult fingerResult = XmlUnityConvert.XmlDeserialize<FingerResult>(value);
                 ResultWindow resultWindow = CheckResultMsg(msgType);
                 if (resultWindow == null)
                 {
                     return;
                 }
                 if (fingerResult.ResultCode == 0)
                 {
                     resultWindow.SuccessCloseWindow();
                     FingerViewModel.AddFingerPrintByUser(fingerResult);
                 }
                 else
                 {
                     resultWindow.FailedCloseWindow(fingerResult);
                 }

             }));
        }
    }
}
