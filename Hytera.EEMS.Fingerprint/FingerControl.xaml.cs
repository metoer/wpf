using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Fingerprint.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hytera.EEMS.Fingerprint
{
    /// <summary>
    /// FingerControl.xaml 的交互逻辑
    /// </summary>
    public partial class FingerControl : UserControl
    {
        /// <summary>
        /// 父窗口
        /// </summary>
        private Window parentWindow;

        /// <summary>
        /// 最后一次查询部门信息
        /// </summary>
        string lastOrgId = string.Empty;

        /// <summary>
        /// 最后一次查询名称
        /// </summary>
        string lastName = string.Empty;

        /// <summary>
        /// 最后查询的用户ID
        /// </summary>
        string lastUserId = string.Empty;

        public FingerControl(Window parentWindow)
        {
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Hytera.EEMS.Resources;Component/Styles/Commom/FingerStyles.xaml", UriKind.RelativeOrAbsolute) });
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Hytera.EEMS.Fingerprint;Component/Styles/PersonStyles.xaml", UriKind.RelativeOrAbsolute) });
            InitializeComponent();
            this.parentWindow = parentWindow;
            lvData.SetBinding(ListView.ItemsSourceProperty, new Binding() { Source = FingerViewModel.PoliceInfos });
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty MoreVisibilityProperty = DependencyProperty.Register("MoreVisibility", typeof(Visibility), typeof(FingerControl), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 是否显示更多
        /// </summary>
        public Visibility MoreVisibility
        {
            get
            {
                return (Visibility)GetValue(MoreVisibilityProperty);
            }
            set
            {
                SetValue(MoreVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 点击查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            lastOrgId = cmbOrg.SelectValue;
            lastName = txtName.Text;
            lastUserId = AppConfigInfos.CurrentUserInfos.UserID;
            SearchInfo("1", "FingerSearchInfo", "FingerSearchOverTime", "FingerSearchFail");
        }

        /// <summary>
        /// 点击加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            SearchInfo((++FingerViewModel.PageIndex).ToString(), "FingerAddInfo", "FingerAddInfoOverTime", "FingerAddInfoFail");
        }

        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="tipInfo"></param>
        /// <param name="overInfo"></param>
        /// <param name="failInfo"></param>
        private void SearchInfo(string pageIndex, string tipInfo, string overInfo, string failInfo)
        {
            MoreVisibility = Visibility.Collapsed;
            Conditions con = new Conditions();
            con.AddItem("OrgID", lastOrgId);
            con.AddItem("UserName", lastName);
            con.AddItem("PageIndex", pageIndex);
            con.AddItem("PageCount", AppConfigInfos.AppStateInfos.SearchPageCount.ToString());

            // 发送配对消息
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(parentWindow, MsgType.FingerInfosRequest, MsgType.FingerInfosRespond, con, TryFindResource(tipInfo).ToString(), 60);
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                NewMessageBox.Show(TryFindResource(overInfo).ToString(), parentWindow);
            }
            else if (msgBoxResult == MessageBoxResult.Yes)
            {
                MoreVisibility = FingerViewModel.IsMoreData ? Visibility.Visible : Visibility.Collapsed;
                spNodata.Visibility = lvData.Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                ModelResponsible.Instance.SendOperationLog("CollectSearchFinger");
            }
            else if (msgBoxResult == MessageBoxResult.No)
            {
                NewMessageBox.Show(TryFindResource(failInfo).ToString(), parentWindow);
            }
        }

        private void my_Loaded(object sender, RoutedEventArgs e)
        {
            // 非同一个用户切换清空数据
            if (!AppConfigInfos.CurrentUserInfos.UserID.Equals(lastUserId))
            {
                cmbOrg.SelectValue = AppConfigInfos.CurrentUserInfos.OrgID;
                FingerViewModel.PoliceInfos.Clear();
                txtName.Text = string.Empty;
                btnSearch_Click(null, null);
            }
        }

    }


}
