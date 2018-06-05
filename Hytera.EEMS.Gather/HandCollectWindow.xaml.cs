using Hytera.EEMS.Common;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Gather
{
    /// <summary>
    /// HandCollectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HandCollectWindow : BaseWindow
    {
        public HandCollectWindow()
        {
            InitializeComponent();
        }

        public HandCollectWindow(DeviveInfo deviveInfo)
            : this()
        {
            this.DeviveInfo = deviveInfo;

            tbCode.Text = deviveInfo.DeviceCode;
            autoCmb.TextChanged += autoCmb_TextChanged;
        }

        void autoCmb_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbMsg.Text = string.Empty;
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
        /// 采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCollect_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            if (string.IsNullOrEmpty(autoCmb.SelectValue))
            {
                tbMsg.Text = TryFindResource("GatherCollectInfo").ToString();
                return;
            }

            Conditions con = new Conditions();
            con.AddItem("DeviceID", DeviveInfo.DeviceCode);
            con.AddItem("UserID", autoCmb.SelectValue);

            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.DeviceHandCollect, MsgType.DeviceHandCollectResult, con, TryFindResource("GatherCollecting").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                tbMsg.Text = TryFindResource("GatherCollectOvertime").ToString();
            }
            else if (msgBoxResult == MessageBoxResult.No)
            {
                tbMsg.Text = TryFindResource("GatherCollectFailed").ToString();
            }
            else
            {
                // 操作日志
                string code = "CollectHandGather";
                ModelResponsible.Instance.SendOperationLog(code);
                this.Close();
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
    }
}
