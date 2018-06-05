using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Fingerprint.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources;
using Hytera.EEMS.Resources.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Fingerprint.Controls
{
    /// <summary>
    /// PersonItem.xaml 的交互逻辑
    /// </summary>
    public partial class PersonItem : UserControl
    {
        public PersonItem()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnCollect = this.Template.FindName("btnCollect", this) as Button;
            if (btnCollect != null)
            {
                btnCollect.Click -= btnCollect_Click;
                btnCollect.Click += btnCollect_Click;
            }

            Button btnEditor = this.Template.FindName("btnEditor", this) as Button;
            if (btnEditor != null)
            {
                btnEditor.Click -= btnEditor_Click;
                btnEditor.Click += btnEditor_Click;
            }
        }

        /// <summary>
        /// 编辑指纹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }

            //if (LoginValidate(PermissionConfig.FingerprintCollect))
            //{
            //    return;
            //}

            UserInfos userInfo = GetUser(sender);

            WindowsHelper.ShowDialogWindow<EditorFingerWindow>(ModelResponsible.Instance.ParentWindow, userInfo);
        }

        /// <summary>
        /// 采集指纹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCollect_Click(object sender, RoutedEventArgs e)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }

            //if (LoginValidate(PermissionConfig.FingerprintCollect))
            //{
            //    return;
            //}

            UserInfos userInfo = GetUser(sender);

            WindowsHelper.ShowDialogWindow<CollectFingerWindow>(ModelResponsible.Instance.ParentWindow, userInfo);
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="permissionValue"></param>
        /// <returns></returns>
        private bool LoginValidate(string permissionValue)
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(ModelResponsible.Instance.ParentWindow, permissionValue, "0");
            if (loginWindow.MessageBoxResult != MessageBoxResult.OK)
            {
                NewMessageBox.Show(TryFindResource("FingerNoPower").ToString(), ModelResponsible.Instance.ParentWindow);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private UserInfos GetUser(object sender)
        {
            Button button = sender as Button;

            string useId = (button.Tag ?? string.Empty).ToString();

            UserInfos userInfo = FingerViewModel.PoliceInfos.Find(p => p.UserID.Equals(useId));

            return userInfo;
        }


    }
}
