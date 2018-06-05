using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        /// <summary>
        /// 菜单按钮
        /// </summary>
        Dictionary<string, SelectButton> toolButtons = new Dictionary<string, SelectButton>();

        /// <summary>
        /// 计时器
        /// </summary>
        Timer timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Styles/PortControlStyle.xaml", UriKind.RelativeOrAbsolute) });
        }

        /// <summary>
        /// 定时没有操作调到第一个页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    //正在播放视频音频文件不做界面跳转
                    if (!AppConfigInfos.IsPlay && SystemInfo.GetLastInputTime() > 300000)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ModuleBaseEntry moduleBase = ModuleDispather.Instance.Modules.Find(p => p.ModuleNavigable.Index.ToString().Equals("0"));
                            if (toolButtons.ContainsKey(moduleBase.ModuleCode))
                            {
                                ButtonClick(toolButtons[moduleBase.ModuleCode], moduleBase, false);
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.WirteErrorMsg(ex.Message);
                }
            }));
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddTools();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            // 告警信息
            WarnInfo.WarnHelper.Instance.ReceiveDataHandler += Instance_ReceiveDataHandler;
            MainMessage.Instance.selfMessageNotic += Instance_selfMessageNotic;
            FixPopupBug();
            this.AddHandler(Button.PreviewKeyDownEvent, new KeyEventHandler(Button_PreviewKeyDown));
        }

        /// <summary>
        /// 去掉Backspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 设置下拉框方向
        /// </summary>
        private static void FixPopupBug()
        {
            var ifLeft = SystemParameters.MenuDropAlignment;
            if (ifLeft)
            {
                var t = typeof(SystemParameters);
                var field = t.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
                field.SetValue(null, false);
            }
        }

        /// <summary>
        /// 接收模块间的消息传递
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Instance_selfMessageNotic(object sender, SelfMessageEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                switch (e.MsgType)
                {
                    case AppSelfMsgType.PortSet:
                        WindowsHelper.ShowDialogWindow<PortSetWindow>(this);
                        break;
                    default:
                        break;
                }
            }));
        }

        /// <summary>
        /// 添加工具栏
        /// </summary>
        private void AddTools()
        {
            try
            {
                int index = 0;
                foreach (var item in ModuleDispather.Instance.Modules)
                {
                    item.ShowViewNotice += item_ShowViewNotice;

                    SelectButton button = new SelectButton()
                    {
                        Name = "Name" + item.ModuleNavigable.Index,
                        DefaultSource = item.ModuleNavigable.DefaultSource,
                        MouseOverSource = item.ModuleNavigable.MouseOverSource,
                        SelectImageSource = item.ModuleNavigable.MouseDownSource,
                        Tag = item.ModuleCode
                    };

                    button.SetResourceReference(Button.ContentProperty, item.ModuleNavigable.Name);

                    button.Style = TryFindResource("syToolButton") as Style;

                    button.Click += (sender, e) =>
                    {
                        if (button.IsSelect)
                        {
                            return;
                        }

                        if (!button.Name.Equals("Name" + ModuleDispather.Instance.Modules[0].ModuleNavigable.Index) && !AppHelper.CheckAppState(this))
                        {
                            return;
                        }

                        ButtonClick(button, item);
                    };

                    spTool.Children.Add(button);
                    index++;
                    if (index == 1)
                    {
                        ButtonClick(button, item);
                    }

                    toolButtons.Add(item.ModuleCode, button);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
        }

        /// <summary>
        /// 模块通知显示自己
        /// </summary>
        /// <param name="sender"></param>
        private void item_ShowViewNotice(object sender)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ModuleBaseEntry moduleBase = sender as ModuleBaseEntry;
                if (toolButtons.ContainsKey(moduleBase.ModuleCode))
                {
                    ButtonClick(toolButtons[moduleBase.ModuleCode], moduleBase, false);
                }

            }));
        }

        /// <summary>
        /// 按钮切换内容
        /// </summary>
        /// <param name="button"></param>
        /// <param name="item"></param>
        /// <param name="validate">是否需要验证</param>
        private void ButtonClick(SelectButton button, ModuleBaseEntry item, bool validate = true)
        {
            if (button.IsSelect)
            {
                return;
            }

            //验证权限
            if (!string.IsNullOrEmpty(item.ModuleNavigable.PermissionCode) && validate)
            {
                LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(this, item.ModuleNavigable.PermissionCode, "1");
                if (loginWindow.MessageBoxResult != MessageBoxResult.OK)
                {
                    return;
                }
            }

            frameContent.Content = null;
            item.ShowModelView(frameContent, this);
            ChangeSelectButton(button);
        }


        /// <summary>
        /// 修改按钮选择状态
        /// </summary>
        /// <param name="button"></param>
        private void ChangeSelectButton(SelectButton button)
        {
            foreach (SelectButton item in spTool.Children)
            {
                item.IsSelect = item.Name.Equals(button.Name);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(this, PermissionConfig.SystemSetLook, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                WindowsHelper.ShowDialogWindow<SetWindow>(this);
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 数据同步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!AppHelper.CheckAppState(this))
            {
                return;
            }

            // 发送数据同步
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.DataRefreshRequest, MsgType.DataRefreshRespond, TryFindResource("appMainSendDataRefresh").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                NewMessageBox.Show(TryFindResource("appMainDataRefreshOverTime").ToString());
            }
            else if (msgBoxResult == MessageBoxResult.Yes)
            {
                WindowsHelper.ShowDialogWindow<RefershWindow>(this, this);
            }
            else
            {
                NewMessageBox.Show(TryFindResource("appMainDataRefreshFailed").ToString());
            }
        }

        /// <summary>
        /// 主窗口关闭，系统退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 数据库没有连接上则不需要验证
            if (!AppConfigInfos.AppStateInfos.DataBaseState.Equals("1"))
            {
                LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(this, PermissionConfig.AppClose, "0");
                if (loginWindow.MessageBoxResult != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    MainMessage.Instance.SendOperationLog("CollectAppExit");
                }
            }
            else
            {
                MessageBoxResult result = NewMessageBox.Show(TryFindResource("appMainExitApp").ToString(), MessageBoxButton.OKCancel, this);
                if (result != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }

            WarnInfo.WarnHelper.Instance.ReceiveDataHandler -= Instance_ReceiveDataHandler;
            ModuleDispather.Instance.Dispose();
            LogHelper.Instance.Dispose();
            AppHelper.CloseKey();
            App.Current.Shutdown();
        }

        /// <summary>
        /// 设置下拉按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click_1(object sender, RoutedEventArgs e)
        {
            popupSet.IsOpen = true;
        }

        /// <summary>
        /// 语言设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Language_Click(object sender, RoutedEventArgs e)
        {
            WindowsHelper.ShowDialogWindow<LanguageWindow>(this);
        }

        /// <summary>
        /// 告警信息
        /// </summary>
        /// <param name="obj"></param>
        private void Instance_ReceiveDataHandler(string obj)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                warnControl.ReceiveMsg(obj);
            }));
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            WindowsHelper.ShowDialogWindow<AboutWindow>(this);
        }

        /// <summary>
        /// 端口设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPortSet_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(this, PermissionConfig.SystemSetLook, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                WindowsHelper.ShowDialogWindow<PortSetWindow>(this);
            }
        }

        /// <summary>
        /// 启动键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKey_Click(object sender, RoutedEventArgs e)
        {
            AppHelper.StartKey(this);
        }

        private void BaseWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
    }
}
