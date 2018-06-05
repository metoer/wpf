using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// RefershWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RefershWindow : BaseWindow
    {
        /// <summary>
        /// 消息转换
        /// </summary>
        Dictionary<string, string> msgList = new Dictionary<string, string>();

        Window parentWindow;

        /// <summary>
        /// 构造
        /// </summary>
        public RefershWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;

            // 初始化消息具体内容
            msgList.Add("1", TryFindResource("appMainRefreshUser").ToString());
            msgList.Add("2", TryFindResource("appMainRefreshDevice").ToString());
            msgList.Add("3", TryFindResource("appMainRefreshOrg").ToString());
            msgList.Add("4", TryFindResource("appMainRefreshPower").ToString());
            msgList.Add("5", TryFindResource("appMainRefreshRolePower").ToString());
            msgList.Add("6", TryFindResource("appMainRefreshUserPower").ToString());
            msgList.Add("7", TryFindResource("appMainRefreshRole").ToString());
            msgList.Add("8", TryFindResource("appMainRefreshUserRole").ToString());
            msgList.Add("9", TryFindResource("appMainRefreshCollectStation").ToString());
            msgList.Add("10", TryFindResource("appMainRefreshFingerprint").ToString());
            msgList.Add("11", TryFindResource("appMainRefreshFinish").ToString());

            AppConfigInfos.AppStateInfos.PropertyChanged += AppStateInfos_PropertyChanged;
        }

        /// <summary>
        /// 监听连接状态,状态提示框打开情况下，如状态正常，则关闭状态，否则修改状态内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppStateInfos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DataBaseState") || e.PropertyName.Equals("ServerState"))
            {
                if (tbResult != null)
                {
                    tbResult.Text = TryFindResource("appMainRefreshResultCode_0").ToString();
                }
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dataRefreshResult"></param>
        public void UpdateProgress(DataRefreshResult dataRefreshResult)
        {
            pbData.Tag = (dataRefreshResult.DataCode == ResultCode.Success).ToString();
            if (dataRefreshResult.DataCode == ResultCode.Success)
            {
                pbData.Value = Convert.ToDouble(dataRefreshResult.ProgressValue);
            }
            else
            {
                string errorMsg = (TryFindResource("appMainRefreshResultCode_" + dataRefreshResult.ResultMsg) ?? string.Empty).ToString();
                errorMsg = string.IsNullOrEmpty(errorMsg) ? TryFindResource("appMainRefreshResultCode_0").ToString() : errorMsg;
                tbResult.Text = errorMsg;
                windowHead.IsEnabled = true;
            }

            tbMsg.Text = msgList.ContainsKey(dataRefreshResult.ResultMsg) ? msgList[dataRefreshResult.ResultMsg] : tbMsg.Text;
            if (Convert.ToInt32(dataRefreshResult.ProgressValue) == 100)
            {
                this.Close();
                MainMessage.Instance.SendCommand(Model.MsgType.DeviceInfosRequest);
                NewMessageBox.Show(TryFindResource("appMainRefreshFinish").ToString(), parentWindow);
            }
        }

        private void BaseWindow_Closed(object sender, EventArgs e)
        {
            AppConfigInfos.AppStateInfos.PropertyChanged -= AppStateInfos_PropertyChanged;
        }
    }
}
