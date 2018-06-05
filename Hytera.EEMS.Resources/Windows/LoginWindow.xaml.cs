using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Hytera.EEMS.Resources.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : BaseWindow
    {
        /// <summary>
        /// 计算超时
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// 构造
        /// </summary>
        public LoginWindow(string permissionID, string isLimitsInfo)
        {
            InitializeComponent();
            this.PermissionID = permissionID;
            this.IsLimitsInfo = isLimitsInfo;
            MessageBoxResult = MessageBoxResult.Cancel;
        }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string PermissionID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否需要权限信息，0：不需要；1：需要
        /// </summary>
        public string IsLimitsInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 操作结果
        /// </summary>
        public MessageBoxResult MessageBoxResult
        {
            get;
            private set;
        }

        /// <summary>
        /// 数据发送入口
        /// </summary>
        public static DataResponsible Responsible
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(2000);
            timer.Tick += new EventHandler(Timer_Tick);

            // 自动发送指纹权限验证
            SendFingerValidate();
            tbName.Focus();
            TextBoxFilterBehavior behavior = new TextBoxFilterBehavior(); 
            Interaction.GetBehaviors(tbName).Add(behavior);

        }

        /// <summary>
        /// 发送指纹验证请求
        /// </summary>
        private void SendFingerValidate()
        {
            Conditions cons = new Conditions();
            cons.AddItem("PermissionID", PermissionID);
            cons.AddItem("IsUserInfo", "1");
            cons.AddItem("IsLimitsInfo", IsLimitsInfo);
            Responsible.SendMsg(MsgType.FingerprintValidate, cons);
        }

        /// <summary>
        /// 指纹验证后的UI 
        /// </summary> 
        /// <param name="isSuccess"></param>
        private void SetFingerprintUI(bool isSuccess)
        {  
            gridFingerReocrding.Visibility = Visibility.Collapsed;
            if (isSuccess)
            {
                gridFingerSuccess.Visibility = Visibility.Visible;
            }
            else
            {
                gridFingerFail.Visibility = Visibility.Visible;
                timer.Start();
            }
        }

        /// <summary>
        /// 指纹验证失败后定时恢复UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            gridFingerFail.Visibility = Visibility.Collapsed;
            gridFingerReocrding.Visibility = Visibility.Visible;
            SendFingerValidate();
        }


        /// <summary>
        /// 指纹验证返回用户信息
        /// </summary>
        /// <param name="value"></param>
        public void ReciveMsg(UserResult userResult)
        {
            string msg = string.Empty;
            switch (userResult.UserResultCode)
            {
                case "0":
                    SetFingerprintUI(true);
                   // this.IsEnabled = false;
                    SaveLoginUserInfo(userResult);
                    new Thread(() =>
                    {
                        MessageBoxResult = MessageBoxResult.OK;
                        Thread.Sleep(500);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.Close();
                        }));
                    }) { IsBackground = true }.Start();
                    break;

                default:
                    SetFingerprintUI(false);
                    msg = (TryFindResource("appLoginCode_" + userResult.UserResultCode) ?? "Undefine Error").ToString();
                    break;
            }

            if (!string.IsNullOrEmpty(msg))
            {
                tbMsg.Text = msg;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConditions())
            {
                return;
            }

            Conditions cons = new Conditions();
            cons.AddItem("UserName", tbName.Text);
            cons.AddItem("Password", tbPassword.Text);
            cons.AddItem("PermissionID", PermissionID);
            cons.AddItem("IsUserInfo", "1");
            cons.AddItem("IsLimitsInfo", IsLimitsInfo);
            tbMsg.Text = string.Empty;

            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.AccountValidate, MsgType.AccountValidateResult, cons, TryFindResource("appValidating").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            UserResult userResult = resultWindow.ResultValue as UserResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                tbMsg.Text = TryFindResource("appLoginCode_5").ToString();
            }
            else if (msgBoxResult == System.Windows.MessageBoxResult.Yes)
            {
                SaveLoginUserInfo(userResult);
                MessageBoxResult = MessageBoxResult.OK;
                this.Close();
            }
            else if (msgBoxResult == System.Windows.MessageBoxResult.No)
            {
                string msg = string.Empty;
                msg = (TryFindResource("appLoginCode_" + userResult.UserResultCode) ?? "Undefine Error").ToString();
                tbMsg.Text = msg;
            }
        }

        /// <summary>
        /// 存储登录用户信息
        /// </summary>
        private void SaveLoginUserInfo(UserResult userResult)
        {
            AppConfigInfos.CurrentUserInfos = userResult.UserInfos;
            if (IsLimitsInfo == "1")
            {
                AppConfigInfos.LimitsUserInfos = userResult.UserInfos;
                AppConfigInfos.LimitsUserInfos.OrgIDCodeStr = userResult.UserInfos.OrgID + ",";
                if (AppConfigInfos.LimitsUserInfos.OrgList != null && AppConfigInfos.LimitsUserInfos.OrgList.orgList != null)
                {
                    foreach (OrgInfos oi in AppConfigInfos.LimitsUserInfos.OrgList.orgList)
                    {
                        AppConfigInfos.LimitsUserInfos.OrgIDCodeStr += oi.OrgID + ",";
                    }
                    AppConfigInfos.LimitsUserInfos.OrgIDCodeStr = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr.Substring(0, AppConfigInfos.LimitsUserInfos.OrgIDCodeStr.Length - 1);

                }
            }
        }

        /// <summary>
        /// 用户名和密码变动清空提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbMsg.Text = string.Empty;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <returns></returns>
        private bool CheckConditions()
        {
            if (string.IsNullOrEmpty(tbName.Text.Trim()))
            {
                tbMsg.Text = TryFindResource("appUserNameNoEmpty").ToString();
                return false;
            }
            else if (string.IsNullOrEmpty(tbPassword.Text.Trim()))
            {
                tbMsg.Text = TryFindResource("appPasswordLess").ToString();
                return false;
            }

            return true;
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Responsible.SendCommand(MsgType.FingerprintStopValidate);

        }
    }
}
