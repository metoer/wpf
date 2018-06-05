using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hytera.EEMS.Fingerprint
{
    public partial class ModelResponsible : ModuleBaseEntry
    {
        FingerControl fingerControl;

        /// <summary>
        /// 消息推动对象
        /// </summary>
        public DataResponsible DataResponsible
        {
            get;
            private set;
        }

        public Window ParentWindow
        {
            get;
            private set;
        }

        /// <summary>
        /// 模块入口对象
        /// </summary>
        public static ModelResponsible Instance
        {
            get;
            private set;
        }

        public ModelResponsible()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                Instance = this;

                ModuleNavigable = new ControlNavigable();
                ModuleNavigable.DefaultSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Finger/FingerDefault.png", UriKind.RelativeOrAbsolute));
                ModuleNavigable.MouseOverSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Finger/FingerOver.png", UriKind.RelativeOrAbsolute));
                ModuleNavigable.MouseDownSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Finger/FingerSelect.png", UriKind.RelativeOrAbsolute));

                ModuleNavigable.Index = 2;

                ModuleNavigable.Name = "FingerToolName";
                ModuleNavigable.PermissionCode = PermissionConfig.Fingerprint;
            }));
        }

        public override void Init(DataResponsible responsible)
        {
            DataResponsible = responsible;
        }

        public override void ShowModelView(Frame parentFrame, Window parentWindow)
        {
            if (fingerControl == null)
            {
                fingerControl = new FingerControl(parentWindow);
            }

            parentFrame.Content = fingerControl;
            ParentWindow = parentWindow;
            SendOperationLog("CollectFingerPrint");
        }

        public override void OnMessageNotice(MsgType msgType, string message)
        {
            switch (msgType)
            {
                case MsgType.FingerInfosRespond:
                    AnalyzePoliceInfos(message, msgType);
                    break;

                case MsgType.FingerEndResult:
                    AnalyzeCollectFingerPrint(message);
                    break;

                case MsgType.FingerEditorRespond:
                    AnalyzeEditorFingerPrint(message, msgType);
                    break;

                case MsgType.FingerSaveRespond:
                    AnalyzeSaveFingerPrint(message, msgType);
                    break;

                default:
                    break;
            }
        }

        public override void OnUnknownMessageNotice(string msgType, string message)
        {

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public void SendMessage(MsgType msgType, Conditions data)
        {
            DataResponsible.SendMsg(msgType, data);
        }

        /// <summary>
        /// 发送操作日志
        /// </summary>
        /// <param name="data">操作码</param>
        /// <param name="data">被操作对象,如果没有不传</param>
        public void SendOperationLog(string operateCode, Conditions data = null)
        {
            if (data == null)
            {
                data = new Conditions();
            }

            data.AddItem("StationID", AppConfigInfos.AppStateInfos == null ? "" : AppConfigInfos.AppStateInfos.StationID);
            data.AddItem("StationCode", AppConfigInfos.AppStateInfos == null ? "" : AppConfigInfos.AppStateInfos.StationCode);
            data.AddItem("OperatorGuid", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.UserGuid);
            data.AddItem("OperatorID", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.UserID);
            data.AddItem("OperatorName", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.UserName);
            data.AddItem("OperatorCode", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.UserCode);
            data.AddItem("OperatorOrgID", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.OrgID);
            data.AddItem("OperatorOrgName", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.OrgName);
            data.AddItem("OperatorOrgIDCode", AppConfigInfos.CurrentUserInfos == null ? "" : AppConfigInfos.CurrentUserInfos.OrgIDCode);
            data.AddItem("OpTime", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            data.AddItem("OpType", operateCode);
            data.AddItem("LogID", DateTime.Now.ToString("yyyyMMddHHmmss") + AppConfigInfos.AppStateInfos == null ? "" : AppConfigInfos.AppStateInfos.StationCode + Guid.NewGuid().ToString("N"));

            DataResponsible.SendMsg(MsgType.StationLogContent, data);
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
