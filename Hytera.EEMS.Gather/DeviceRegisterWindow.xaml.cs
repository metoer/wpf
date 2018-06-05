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
    /// DeviceRegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceRegisterWindow : BaseWindow
    {
        public DeviceRegisterWindow(DeviveInfo deviveInfo)
        {
            InitializeComponent();
            this.DeviveInfo = deviveInfo;
        }

        /// <summary>
        /// 当前执法记录仪
        /// </summary>
        public DeviveInfo DeviveInfo
        {
            get;
            private set;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cbTypes.ItemsSource = AppConfigInfos.DeviceTypeList;
            cbTypes.SelectedValuePath = "ID";
            cbTypes.DisplayMemberPath = "CategoryName";
            if (AppConfigInfos.DeviceTypeList.Count > 0)
            {
                cbTypes.SelectedValue = 0;
            }

            tbPassword.Focus();
        }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;

            if (object.Equals(cbTypes.SelectedValue, null))
            {
                tbMsg.Text = TryFindResource("GatherInputDeviceType").ToString();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbPassword.Password))
            {
                tbMsg.Text = TryFindResource("GatherInputPasswrod").ToString();
                return;
            }

            DeviveInfo selectDevice = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(DeviveInfo.DeviceCode));
            if (selectDevice == null)
            {
                NewMessageBox.Show(string.Format(TryFindResource("GatherDeviceRemove").ToString(), DeviveInfo.DeviceCode));
                return;
            }

            Conditions con = new Conditions();
            con.AddItem("UserID", AppConfigInfos.CurrentUserInfos.UserID);
            con.AddItem("DeviceID", DeviveInfo.DeviceCode);
            con.AddItem("DevicePassword", tbPassword.Password);
            con.AddItem("TypeID", cbTypes.SelectedValue.ToString());

            // 发送注册消息
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.DeviceRegister, MsgType.DeviceRegisterResult, con, TryFindResource("GatherRegistering").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                tbMsg.Text = TryFindResource("GatherRegisterOvertime").ToString();
            }
            else if (msgBoxResult == MessageBoxResult.Yes)
            {
                ModelResponsible.Instance.SendOperationLog("CollectDeviceRegister");
                this.Close();
                //NewMessageBox.Show(TryFindResource("GatherRegisterSuccess").ToString(), ModelResponsible.Instance.ParentWindow);
            }
            else if (msgBoxResult == MessageBoxResult.No)
            {
                DeviceRegisterInfo deviceRegisterInfo = resultWindow.ResultValue as DeviceRegisterInfo;
                if (deviceRegisterInfo != null)
                {
                    string msg = (TryFindResource("GatherDeviceRegisterError_" + deviceRegisterInfo.ResultCode) ?? string.Empty).ToString();

                    tbMsg.Text = msg;
                }
                else
                {
                    tbMsg.Text = TryFindResource("GatherRegisterFailed").ToString();
                }
            }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
        }
    }
}
