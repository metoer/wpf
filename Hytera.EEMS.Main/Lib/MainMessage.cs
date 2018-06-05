using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hytera.EEMS.Main.Lib
{
    /// <summary>
    /// Main消息处理层
    /// </summary>
    public partial class MainMessage : MessageDispatch
    {
        private static MainMessage instance;

        public event DelegateAppSelfMessageNotic selfMessageNotic;

        /// <summary>
        /// 单实例
        /// </summary>
        public static MainMessage Instance
        {
            get
            {
                if (instance == null)
                {
                    Interlocked.CompareExchange(ref instance, new MainMessage(), null);
                }

                return instance;
            }
        }

        protected override void Init(DataResponsible responsible)
        {
            LoginWindow.Responsible = responsible;
            ResultWindow.Responsible = responsible;
        }

        protected override void OnUnknownMessageNotice(object sender, UnknownMessageEventArgs e)
        {

        }

        protected override void OnConnectNetWorkNotic(DataResponsible responsible)
        {
            // 获取执法记录仪信息列表
            responsible.SendCommand(Model.MsgType.DeviceInfosRequest);
            responsible.SendCommand(Model.MsgType.PcStateRequest);
            responsible.SendCommand(Model.MsgType.PortInfosRequest);
            responsible.SendCommand(Model.MsgType.LicenseRequest);

            // 设置优先端口
            if (!string.IsNullOrEmpty(AppConfigInfos.PortDeviceList.FirstPortCode))
            {
                Conditions con = new Conditions();
                con.AddItem("PortCode", AppConfigInfos.PortDeviceList.FirstPortCode.Equals("----") ? string.Empty : AppConfigInfos.PortDeviceList.FirstPortCode);
                con.AddItem("Respond", "1");
                MainMessage.Instance.SendMessage(Model.MsgType.SetFirstPortRequest, con);
            }
        }
        

        protected override void OnMessageNotice(object sender, MessageEventArgs e)
        {
            switch (e.MsgType)
            {
                case MsgType.PcState:
                    AnalyzePcState(e.Message);
                    break;

                case MsgType.AccountValidateResult:
                    AnalyzeAccountValidate(e.MsgType, e.Message);
                    break;

                case MsgType.DataRefreshRespond:
                    AnalyzeDataReferesh(e.MsgType, e.Message);
                    break;

                case MsgType.DataRefreshUpdate:
                    AnalyzeDataProgress(e.MsgType, e.Message);
                    break;

                case MsgType.LicenseRespond:
                    AnalyzeLicenseInfo(e.Message);
                    break;

                case MsgType.PortInfosRespond:
                    AnalyzePortInfo(e.Message);
                    break;

                case MsgType.SetFirstPortRespond:
                    AnalyzeSetFirstPort(e.MsgType, e.Message);
                    break;

                case MsgType.DeviceTypesRespond:
                    AnalyzeDeviceTypes(e.Message);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 内部模块通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnAppSelfMessageNotic(object sender, SelfMessageEventArgs e)
        {
            if (selfMessageNotic != null)
            {
                selfMessageNotic(sender, e);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="data"></param>
        public void SendMessage(MsgType msgType, object data)
        {
            Responsible.SendMsg(msgType, data);
        }

        public void SendMessage(MsgType msgType, Conditions data)
        {
            Responsible.SendMsg(msgType, data);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="msgType"></param>
        public void SendCommand(MsgType msgType)
        {
            Responsible.SendCommand(msgType);
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

            Responsible.SendMsg(MsgType.StationLogContent, data);
        }
    }
}
