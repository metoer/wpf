using Hytera.EEMS.Common;
using Hytera.EEMS.Fingerprint.Controls;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Hytera.EEMS.Fingerprint
{
    /// <summary>
    /// CollectFingerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CollectFingerWindow : BaseWindow
    {
        public CollectFingerWindow(UserInfos currentUser)
        {
            InitializeComponent();
            this.CurrentUser = currentUser;
            this.MouseDown += CollectFingerWindow_MouseDown;
            SendStartCollectFinger();
        }

        /// <summary>
        /// 指纹采集返回数据
        /// </summary>
        public string FingerImage
        {
            get;
            set;
        }

        /// <summary>
        /// 采集指纹用户
        /// </summary>
        public UserInfos CurrentUser
        {
            get;
            private set;
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty FingerStatusProperty = DependencyProperty.Register("FingerStatus", typeof(FingerStatus), typeof(CollectFingerWindow), new PropertyMetadata(FingerStatus.Recording, OnStateChanged));

        /// <summary>
        /// 状态
        /// </summary>
        public FingerStatus FingerStatus
        {
            get
            {
                return (FingerStatus)GetValue(FingerStatusProperty);
            }
            set
            {
                SetValue(FingerStatusProperty, value);
            }
        }

        /// <summary>
        /// 发送采集指纹命令
        /// </summary>
        private void SendStartCollectFinger()
        {
            Conditions con = new Conditions();
            con.AddItem("UserID", CurrentUser.UserID);
            ModelResponsible.Instance.SendMessage(MsgType.FingerStartRequest, con);
        }

        /// <summary>
        /// 点击非指纹名编辑区，退出编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectFingerWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is EditorButton))
            {
                btnEditor.EditorButtonStatus = EditorButtonStatus.Reading;
            }
        }

        /// <summary>
        /// 采集指纹状态切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CollectFingerWindow fingerWindow = sender as CollectFingerWindow;
            FingerStatus fingerStatus = (FingerStatus)e.NewValue;
            fingerWindow.txtMsg.Visibility = Visibility.Collapsed;
            fingerWindow.recordGrid.Visibility = Visibility.Collapsed;
            fingerWindow.btnStart.Content = fingerWindow.TryFindResource("appSure").ToString();
            switch (fingerStatus)
            {
                case FingerStatus.Recording:
                    fingerWindow.recordGrid.Visibility = Visibility.Visible;
                    fingerWindow.fingerImg.Source = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Finger/recordImg.png", UriKind.RelativeOrAbsolute));
                    break;
                case FingerStatus.RecordFail:
                    fingerWindow.fingerImg.Source = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Finger/recordFail.png", UriKind.RelativeOrAbsolute));
                    fingerWindow.txtMsg.Visibility = Visibility.Visible;
                    fingerWindow.btnStart.Content = fingerWindow.TryFindResource("FingerReRecord").ToString(); ;
                    break;
                case FingerStatus.RecordSuccess:
                    fingerWindow.fingerImg.Source = new BitmapImage(new Uri(@"/Hytera.EEMS.Resources;Component/Images/Finger/recordSuccess.png", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            if (FingerStatus == FingerStatus.RecordSuccess)
            {
                Conditions con = new Conditions();
                con.AddItem("UserID", CurrentUser.UserID);
                con.AddItem("FingerName", btnEditor.Text);
                con.AddItem("FingerImage", FingerImage);


                // 发送配对消息
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.FingerSaveRequest, MsgType.FingerSaveRespond, con, TryFindResource("FingerSaveData").ToString());
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    tbMsg.Text = TryFindResource("FingerSaveOverTime").ToString();
                }
                else if (msgBoxResult == MessageBoxResult.Yes)
                {
                    ModelResponsible.Instance.SendOperationLog("CollectFingerGather");
                    FingerStatus = FingerStatus.Finish;
                    this.Close();
                    NewMessageBox.ShowTip(TryFindResource("FingerSaveSuccess").ToString(), ModelResponsible.Instance.ParentWindow);
                }
                else if (msgBoxResult == MessageBoxResult.No)
                {
                    FingerResult fingerResult = resultWindow.ResultValue as FingerResult;

                    tbMsg.Text = (TryFindResource("FingerCollectCode_" + fingerResult.ResultCode) ?? string.Empty).ToString();
                }
            }
            else if (FingerStatus == FingerStatus.RecordFail)
            {
                SendStartCollectFinger();
                FingerStatus = FingerStatus.Recording;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗口关闭时，判断指纹采集情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (FingerStatus == FingerStatus.RecordSuccess)
            {
                MessageBoxResult messageBoxResult = NewMessageBox.Show(TryFindResource("FingerCloseSaveWindow").ToString(), MessageBoxButton.YesNo, this);
                if (messageBoxResult != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Conditions con = new Conditions();
            con.AddItem("UserID", CurrentUser.UserID);
            ModelResponsible.Instance.SendMessage(MsgType.FingerStopRequest, con);

        }
    }

    public enum FingerStatus
    {
        Recording,
        RecordFail,
        RecordSuccess,
        Finish
    }

}
