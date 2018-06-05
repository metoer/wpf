using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Enums;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hytera.EEMS.Manage
{
    /// <summary>
    /// 模块入口
    /// </summary>
    public partial class ModelResponsible : ModuleBaseEntry
    {
        /// <summary>
        /// 查询管理面板
        /// </summary>
        UCManageMain manage;

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
            set;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public ModelResponsible()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ModuleNavigable = new ControlNavigable();

                Instance = this;

                ModuleNavigable.DefaultSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Manage/manageDefault.png", UriKind.RelativeOrAbsolute));
                ModuleNavigable.MouseOverSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Manage/manageOver.png", UriKind.RelativeOrAbsolute));
                ModuleNavigable.MouseDownSource = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Manage/manageSelect.png", UriKind.RelativeOrAbsolute));

                ModuleNavigable.Index = 1;
                ModuleNavigable.Name = "ManageToolName";

                
                ModuleNavigable.PermissionCode = PermissionConfig.DataSearchMoudle;
            }));
        }

        /// <summary>
        /// 模块入口对象
        /// </summary>
        public static ModelResponsible Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="SendData"></param>
        public override void Init(DataResponsible SendData)
        {
            DataResponsible = SendData;
        }

        public override void ShowModelView(Frame parentFrame,Window parentWindow)
        {
            if (manage == null)
            {
                manage = new UCManageMain();
            }

            ParentWindow = parentWindow;

            parentFrame.Content = manage;

            SendOperationLog("CollectDevicePlayBack");
        }

        private bool isTag = false;
        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public override void OnMessageNotice(MsgType msgType, string message)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                switch (msgType)
                {
                    case MsgType.CollecLogRespond:
                        AnalyzeCollectLogsDetail(message);
                        isTag = true;
                        break;
                    case MsgType.CameraLogRespond:
                        AnalyzeCameraLogsDetail(message);
                        isTag = true;
                        break;
                    case MsgType.MediaLogRespond:
                        AnalyzeMediaListDetail(message);
                        isTag = true;
                        break;
                    case MsgType.AlarmLogRespond:
                        AnalyzeAlarmLogsDetail(message);
                        isTag = true;
                        break;
                    case MsgType.CollecLogCountRespond:
                        AnalyzeCollectLogsCount(message);
                        break;
                    case MsgType.CameraLogCountRespond:
                        AnalyzeCameraLogsCount(message);
                        break;
                    case MsgType.MediaLogCountRespond:
                        AnalyzeAllMediaListCount(message);
                        break;
                    case MsgType.AlarmCountRespond:
                        AnalyzeAlarmLogsCount(message);
                        isTag = true;
                        break;
                    case MsgType.MediaLogEditorRespond:
                        AnalyzeMediaLogEditor(message);
                        break;
                    case MsgType.MediaLogUploadRespond:
                        AnalyzeMediaLogUpload(message);
                        break;
                    case MsgType.CameraLogUploadRespond:
                        AnalyzeCameraLogUpload(message);
                        break;
                    case MsgType.CollectLogUploadRespond:
                        AnalyzeCollectLogUpload(message);
                        break;
                }

                if (isTag)
                {
                    ResultWindow resultWindow = CheckResultMsg(msgType);
                    if (resultWindow != null)
                    {
                        resultWindow.SuccessCloseWindow();
                    }
                    isTag = false;
                }
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

        public override void AppSelfMessageNotic(object sender, SelfMessageEventArgs e)
        {
            switch (e.MsgType)
            { 
                case AppSelfMsgType.DataSearchPlay:
                    Conditions con = new Conditions();
                    con = JsonUnityConvert.DeserializeObject<Conditions>(e.Message.ToString());
                    foreach(Item item in con.ToList())
                    {
                        if (item.Key.Equals("DeviceID"))
                            SearchManager.GetInstance().MediaLogsSerach.DeviceID = item.Value;
                    }
                    ShowSelfViewNotice();
                    manage.ShowDataSearchPlay();

                    SearchManager.GetInstance().MediaLogsSerach.CollectEndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    SearchManager.GetInstance().MediaLogsSerach.CollectStartTime = DateTime.Now.Date.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    SearchManager.GetInstance().MediaLogsSerach.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    SearchManager.GetInstance().MediaLogsSerach.FileName = "";
                    SearchManager.GetInstance().MediaLogsSerach.OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                    SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
                    SearchManager.GetInstance().MediaLogsSerach.UserImp = "0";
                    SearchManager.GetInstance().MediaLogsSerach.FileType = "0";
                    SearchManager.GetInstance().MediaLogsSerach.IsAdvanced = true;
                    ModelResponsible.Instance.ClearMediaList();
                    SearchManager.GetInstance().SearchMediaLogCount(SearchManager.GetInstance().MediaLogsSerach);
                    SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach);
                    LogHelper.Instance.WirteLog("CollectMain:Playback  Search", LogLevel.LogDebug);
                    break;

                default:
                    break;
                }
            
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        public void SendMessage(MsgType msgType, string message)
        {
            DataResponsible.SendMsg(msgType, message);
        }

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

        public void UpdateCount(QueryType qt, FileType ft, string count)
        {
            manage.UpdateCount(qt, ft, count);
        }

        public void UpdateDetailCount(QueryType qt)
        {
            manage.UpdateDetailCount(qt);
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
