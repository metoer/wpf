
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Gather.Lib;
using Hytera.EEMS.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace Hytera.EEMS.Gather
{
    public partial class ModelResponsible : ModuleBaseEntry
    {
        /// <summary>
        /// 采集面板
        /// </summary>
        GatherControl gather;

        /// <summary>
        /// 消息推动对象
        /// </summary>
        public DataResponsible DataResponsible
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

        public Window ParentWindow
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ModelResponsible()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                Instance = this;

                ModuleNavigable = new ControlNavigable();
                ModuleNavigable.DefaultSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Collect/toolDefault.png", UriKind.RelativeOrAbsolute));
                ModuleNavigable.MouseOverSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Collect/toolOver.png", UriKind.RelativeOrAbsolute));
                ModuleNavigable.MouseDownSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Collect/toolSelect.png", UriKind.RelativeOrAbsolute));

                ModuleNavigable.Index = 0;

                ModuleNavigable.Name = "GatherToolName";
            }));
        }

        public override void ShowModelView(Frame parentFrame, Window parentWindow)
        {
            if (gather == null)
            {
                gather = new GatherControl(parentWindow);
            }

            parentFrame.Content = gather;
            this.ParentWindow = parentWindow;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="responsible"></param>
        public override void Init(DataResponsible responsible)
        {
            DataResponsible = responsible;
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>

        public override void OnMessageNotice(Model.MsgType msgType, string message)
        {
            switch (msgType)
            {
                case MsgType.DeviceInfosRespond:
                    AnalyzeDeviceInfos(message);
                    break;

                case MsgType.DeviceUpdateState:
                    AnalyzeDeviceUpdateState(message);
                    break;

                case MsgType.DeviceRegisterResult:
                    AnalyzeDeviceRegisterResult(msgType, message);
                    break;

                case MsgType.DevicePairResult:
                case MsgType.DeviceCancelPairResult:
                    AnalyzeDevicePairResult(msgType, message);
                    break;

                case MsgType.DeviceHandCollectResult:
                case MsgType.DeviceResumeCollectResult:
                    AnalyzeDeviceCollectResult(msgType, message, DeviceState.Collecting);
                    break;

                case MsgType.DeviceStopCollectResult:
                    AnalyzeDeviceCollectResult(msgType, message, DeviceState.PauseCollect);
                    break;

                case MsgType.DeviceInfosRemove:
                    AnalyzeDeviceInfosRemove(message);
                    break;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public override void DisConnectNetWork()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceInfoHelper.DeviceItemClear();
            }));
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
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
    }
}
