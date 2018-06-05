using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Gather.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System.Windows;

namespace Hytera.EEMS.Gather
{
    /// <summary>
    /// MatchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MatchWindow : BaseWindow
    {

        /// <summary>
        /// 标示当前界面是否为配对
        /// </summary>
        private bool isPair = true;

        /// <summary>
        /// 构造
        /// </summary>
        public MatchWindow()
        {
            InitializeComponent();
        }

        public MatchWindow(DeviveInfo deviveInfo)
            : this()
        {
            this.DeviveInfo = deviveInfo;

            tbCode.Text = deviveInfo.DeviceCode;
            autoCmb.TextChanged += autoCmb_TextChanged;
            isPair = DeviveInfo.IsMatchOrRegist == IsMatchOrRegist.Registered;
        }


        /// <summary>
        /// 当前配对设备信息
        /// </summary>
        public DeviveInfo DeviveInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            autoCmb.InsertEmpty = DeviveInfo.IsMatchOrRegist == IsMatchOrRegist.Matched;
            autoCmb.SelectValue = DeviveInfo.MatchUserID;
        }

        /// <summary>
        /// 配对
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMatch_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            btnCanelMatch.Visibility = Visibility.Hidden;

            if ((DeviveInfo.IsMatchOrRegist == IsMatchOrRegist.Registered &&
                string.IsNullOrEmpty(autoCmb.SelectValue)) ||
                (DeviveInfo.IsMatchOrRegist == IsMatchOrRegist.Matched &&
                !string.IsNullOrEmpty(autoCmb.Text) &&
                string.IsNullOrEmpty(autoCmb.SelectValue)))
            {
                tbMsg.Text = TryFindResource("GatherPairInfoError").ToString();
                return;
            }

            DeviveInfo selectDevice = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(DeviveInfo.DeviceCode));
            if (selectDevice == null)
            {
                NewMessageBox.Show(string.Format(TryFindResource("GatherDeviceRemove").ToString(), DeviveInfo.DeviceCode));
                return;
            }

            if (DeviveInfo.IsMatchOrRegist == IsMatchOrRegist.Matched && DeviveInfo.MatchUserID.Equals(autoCmb.SelectValue))
            {
                NewMessageBox.Show(TryFindResource("GatherPairInfoNoChange").ToString());
                // 编辑配对如果用户没有选择其他用户点击配置，则直接结束
                this.Close();
                return;
            }

            Conditions con = new Conditions();
            if (!string.IsNullOrEmpty(autoCmb.SelectValue))
            {
                // 配对
                UserInfos matchUserInfo = AppConfigInfos.LimitsUserInfos.Users.UserList.Find(p => p.UserID.Equals(autoCmb.SelectValue));

                con.AddItem("UserID", autoCmb.SelectValue);
                con.AddItem("DeviceID", DeviveInfo.DeviceCode);
                con.AddItem("UserName", matchUserInfo == null ? string.Empty : matchUserInfo.UserName);
                con.AddItem("UserCode", matchUserInfo == null ? string.Empty : matchUserInfo.UserCode);
                con.AddItem("OrgID", matchUserInfo == null ? string.Empty : matchUserInfo.OrgID);
                con.AddItem("OrgName", matchUserInfo == null ? string.Empty : matchUserInfo.OrgName);
                con.AddItem("ForceMatch", "0"); // 调度员没有取消后再配对协议
            }
            else
            {
                // 选择为空的则为取消配对
                CancelMatch(DeviveInfo.DeviceCode, (DeviveInfo.DeviceID ?? string.Empty).ToString());
                this.Close();
                return;
            }

            // 发送配对消息
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.DevicePair, MsgType.DevicePairResult, con, TryFindResource("GatherPairing").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                tbMsg.Text = TryFindResource("GatherPairOverTime").ToString();
            }
            else if (msgBoxResult == MessageBoxResult.Yes)
            {
                if (isPair)
                {
                    ModelResponsible.Instance.SendOperationLog("CollectDevicePair");                   
                }
                else
                {
                    ModelResponsible.Instance.SendOperationLog("CollectDeviceEditor");
                }

                this.Close();
            }
            else if (msgBoxResult == MessageBoxResult.No)
            {
                DevicePairInfo devicePairInfo = resultWindow.ResultValue as DevicePairInfo;
                if (devicePairInfo.ResultCode == 2)
                {
                    // 已配对
                    tbMsg.Text = string.Format(TryFindResource("GatherPairMarkStart").ToString(), devicePairInfo.ResultMsg);
                    btnCanelMatch.Visibility = Visibility.Visible;
                    btnCanelMatch.Tag = devicePairInfo.ResultMsg + "," + devicePairInfo.DeviceID; // 记录已配对DeviceCode
                }
                else
                {
                    string msg = (TryFindResource("GatherPairCode_" + devicePairInfo.ResultCode) ?? string.Empty).ToString();
                    msg = string.IsNullOrEmpty(msg) ? TryFindResource("GatherPairCode_-1").ToString() : msg;
                    tbMsg.Text = msg;
                }
            }
        }

        /// <summary>
        /// 取消配对
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanelMatch_Click(object sender, RoutedEventArgs e)
        {
            string[] value = btnCanelMatch.Tag.ToString().Split(',');
            CancelMatch(value[0], value.Length == 2 ? value[1] : string.Empty);
        }

        /// <summary>
        /// 取消配对
        /// </summary>
        /// <param name="deviceCode"></param>
        private void CancelMatch(string deviceCode, string deviceID)
        {
            Conditions con = new Conditions();
            con.AddItem("DeviceID", deviceID);
            con.AddItem("DeviceCode", deviceCode);

            // 发送配对消息
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.DeviceCancelPair, MsgType.DeviceCancelPairResult, con, TryFindResource("GatherCancelPairing").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                tbMsg.Text = TryFindResource("GatherCancelPairOverTime").ToString();
            }
            else if (msgBoxResult == MessageBoxResult.Yes)
            {
                NewMessageBox.Show(TryFindResource("GatherCancelPairSuccess").ToString(), this);
                tbMsg.Text = string.Empty;
                btnCanelMatch.Visibility = Visibility.Hidden;
            }
            else if (msgBoxResult == MessageBoxResult.No)
            {
                DevicePairInfo devicePairInfo = resultWindow.ResultValue as DevicePairInfo;
                if (devicePairInfo.ResultCode == 2)
                {
                    NewMessageBox.Show(TryFindResource("GatherCancelPairFail").ToString(), this);
                }
                else
                {
                    NewMessageBox.Show(TryFindResource("GatherCancelPairFailed").ToString(), this);
                }
            }

        }


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 重新输入清空警示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void autoCmb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            btnCanelMatch.Visibility = Visibility.Hidden;
        }
    }
}
