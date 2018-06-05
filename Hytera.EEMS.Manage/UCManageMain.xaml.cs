using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Enums;
using Hytera.EEMS.Manage.Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Manage
{
    /// <summary>
    /// UCManageMain.xaml 的交互逻辑
    /// </summary>
    public partial class UCManageMain : UserControl
    {
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        Point pos = new Point();
        Thickness tk = new Thickness(0, 0, 33, 5);
        private int count = 0;
        private bool isload = false;
        public UCManageMain()
        {
            InitializeComponent();
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Hytera.EEMS.Manage;Component/Styles/ManageStyles.xaml", UriKind.RelativeOrAbsolute) });
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Hytera.EEMS.Manage;Component/Styles/DataGridStyles.xaml", UriKind.RelativeOrAbsolute) });

            btnPalyList.AddHandler(Button.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(btn_MouseLeftButtonDown), true);
            btnPalyList.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(btn_MouseLeftButtonUp), true);
            btnPalyList.AddHandler(Button.MouseMoveEvent, new MouseEventHandler(btn_MouseMove), true);            
        }

        private void btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button tem = (Button)sender;
            pos = e.GetPosition(null);
            
            tem.CaptureMouse();
            tem.Cursor = Cursors.Hand;
            
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,100);
            count = 0;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            count += 100;
        }

        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Button tem = (Button)sender;
                double dy = pos.Y - e.GetPosition(null).Y + tem.Margin.Bottom;
                if (dy < 5)
                    dy = 5;
                if (dy > 100)
                    dy = 100;
                tem.Margin = new Thickness(0, 0, 33, dy);
                pos = e.GetPosition(null);
            }
        }

        private void btn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button tem = (Button)sender;
            tem.ReleaseMouseCapture();
            dispatcherTimer.Stop();
            if (count < 400)
            {
                playHis.Visibility = Visibility.Visible;
                tem.Margin = tk;
            }
            else
                tk = tem.Margin;
        }
        
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (electronEvidence != null && electronEvidence.IsSelected)
            {
                searchControl.QueryType = QueryType.MediaLog;
            }

            if (collectOperateLog != null && collectOperateLog.IsSelected)
            {
                searchControl.QueryType = QueryType.CollectOperateLog;
            }

            if (operateLog != null && operateLog.IsSelected)
            {
                searchControl.QueryType = QueryType.CameraOperateLog;
            }

            if (hisWarn != null && hisWarn.IsSelected)
            {
                searchControl.QueryType = QueryType.HisAlarm;
            }
        }
        
        public void UpdateCount(QueryType qt, FileType ft, string count)
        {
            switch (qt)
            {
                case QueryType.MediaLog:
                    mediaControl.UpdateCount(ft, count);
                    break;
                case QueryType.CameraOperateLog:
                    logsControl.UpdateCount(count);
                    break;
                case QueryType.CollectOperateLog:
                    collectlogsControl.UpdateCount(count);
                    break;
                case QueryType.HisAlarm:
                    alarmControl.UpdateCount(count);
                    break;
            }
        }

        public void UpdateDetailCount(QueryType qt)
        {
            switch (qt)
            {
                case QueryType.MediaLog:
                    mediaControl.UpdateDetailCount();
                    break;
                case QueryType.HisAlarm:
                    alarmControl.UpdateDetailCount();
                    break;
                case QueryType.CameraOperateLog:
                    logsControl.UpdateDetailCount();
                    break;
                case QueryType.CollectOperateLog:
                    collectlogsControl.UpdateDetailCount();
                    break;
                case QueryType.PlayList:
                    btnPalyList.Content = ManageViewModel.HisPlayMediaList.Count > 99 ? "99+" : ManageViewModel.HisPlayMediaList.Count.ToString();
                    playHis.labNum.Content = ManageViewModel.HisPlayMediaList.Count;
                    break;
            }
        }

        public void ShowDataSearchPlay()
        {
            if (electronEvidence != null)
            {
                electronEvidence.IsSelected = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppConfigInfos.LimitsUserInfos.UserCode != null && !AppConfigInfos.LimitsUserInfos.UserCode.Equals(SearchManager.GetInstance().CurrentUserInfo.UserCode))
            {
                electronEvidence.IsSelected = true;
                mediaControl.btnPic.IsSelect = true;
                mediaControl.btnGrid.IsSelect = false;
                mediaControl.SelectFileType = FileType.All;
                mediaControl.UpdateBtnSelect();
            }
        }
    }
}
