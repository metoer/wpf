using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Gather.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources;
using Hytera.EEMS.Resources.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hytera.EEMS.Gather.Controls
{
    /// <summary>
    /// DeviceInfoItem.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceInfoItem : UserControl
    {
        DeviveInfo deviveInfo;

        private Window parentWindow;

        bool isLoad = false;

        public DeviceInfoItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 索引
        /// </summary>
        public string Index
        {
            get
            {
                return (string)GetValue(IndexProperty);
            }
            set
            {
                SetValue(IndexProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(string), typeof(DeviceInfoItem), new PropertyMetadata("1"));


        /// <summary>
        /// 是否优先端口
        /// </summary>
        public bool IsFirstPort
        {
            get
            {
                return (bool)GetValue(IsFirstPortProperty);
            }
            set
            {
                SetValue(IsFirstPortProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsFirstPortProperty = DependencyProperty.Register("IsFirstPort", typeof(bool), typeof(DeviceInfoItem), new PropertyMetadata(false));

        /// <summary>
        /// 是否显示执法记录仪
        /// </summary>
        public bool HasDevice
        {
            get
            {
                return (bool)GetValue(HasDeviceProperty);
            }
            set
            {
                SetValue(HasDeviceProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty HasDeviceProperty = DependencyProperty.Register("HasDevice", typeof(bool), typeof(DeviceInfoItem), new PropertyMetadata(false));

        /// <summary>
        /// 采集状态
        /// </summary>
        public DeviceState DeviceState
        {
            get
            {
                return (DeviceState)GetValue(DeviceStateProperty);
            }
            set
            {
                SetValue(DeviceStateProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty DeviceStateProperty = DependencyProperty.Register("DeviceState", typeof(DeviceState), typeof(DeviceInfoItem), new PropertyMetadata(DeviceState.Default));

        /// <summary>
        /// 配对状态
        /// </summary>
        public IsMatchOrRegist IsMatchOrRegist
        {
            get
            {
                return (IsMatchOrRegist)GetValue(IsMatchOrRegistProperty);
            }
            set
            {
                SetValue(IsMatchOrRegistProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsMatchOrRegistProperty = DependencyProperty.Register("IsMatchOrRegist", typeof(IsMatchOrRegist), typeof(DeviceInfoItem), new PropertyMetadata(IsMatchOrRegist.UnRegister));


        /// <summary>
        /// 绑定UI信息
        /// </summary>
        /// <param name="deviveInfo"></param>
        public void BingData(DeviveInfo deviveInfo, Window parentWindow)
        {
            this.parentWindow = parentWindow;
            this.deviveInfo = deviveInfo;
            this.DataContext = deviveInfo;
            this.SetBinding(DeviceInfoItem.DeviceStateProperty, new Binding("DeviceState") { Source = deviveInfo });
            this.SetBinding(DeviceInfoItem.IsMatchOrRegistProperty, new Binding("IsMatchOrRegist") { Source = deviveInfo });
            HasDevice = true;
        }

        /// <summary>
        /// 清除信息
        /// </summary>
        public void ClearData()
        {
            HasDevice = false;
            deviveInfo = null;
            this.DataContext = null;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void my_Loaded(object sender, RoutedEventArgs e)
        {
            if (isLoad)
            {
                return;
            }

            isLoad = true;

            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(Button_Click), true);
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            if (button != null)
            {
                if (!AppHelper.CheckAppState(parentWindow))
                {
                    return;
                }

                if (button.Name.Equals("btnMatch"))
                {
                    MatchClick();
                }
                else if (button.Name.Equals("btnCollect"))
                {
                    CollectClick();
                }
            }
        }

        /// <summary>
        /// 配对按钮点击
        /// </summary>
        private void MatchClick()
        {
            switch (deviveInfo.IsMatchOrRegist)
            {
                case IsMatchOrRegist.UnRegister:
                    // 注册
                    RgisterDevice();
                    break;

                case IsMatchOrRegist.Matched:
                    // 编辑配对
                    MatchDevice();
                    break;
                case IsMatchOrRegist.Registered:
                    // 配对
                    MatchDevice();
                    break;
            }
        }

        /// <summary>
        /// 采集按钮点击
        /// </summary>
        public void CollectClick()
        {
            switch (deviveInfo.DeviceState)
            {
                case DeviceState.Default:
                case DeviceState.CollectFailed:
                    // 采集 、 重新开始采集
                    CollectOpertion
                        (
                            PermissionConfig.StartCollect,
                            TryFindResource("GatherCollecting").ToString(),
                            TryFindResource("GatherCollectOvertime").ToString(),
                            MsgType.DeviceHandCollect,
                            MsgType.DeviceHandCollectResult,
                            TryFindResource("GatherCollectFailed").ToString()
                        );
                    break;

                case DeviceState.PauseCollect:
                case DeviceState.Collecting:
                    break;

                case DeviceState.CollectFinish:
                    // 查询回放
                    DataSearch();
                    break;
            }
        }


        /// <summary>
        /// 注册
        /// </summary>
        private void RgisterDevice()
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(parentWindow, PermissionConfig.DeviceRegister, "0");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                DeviveInfo selectDevice = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(deviveInfo.DeviceCode));
                if (selectDevice == null)
                {
                    NewMessageBox.Show(string.Format(TryFindResource("GatherDeviceRemove").ToString(), deviveInfo.DeviceCode));
                    return;
                }

                WindowsHelper.ShowDialogWindow<DeviceRegisterWindow>(parentWindow, deviveInfo);
            }
        }

        /// <summary>
        /// 配对
        /// </summary>
        private void MatchDevice()
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(parentWindow, PermissionConfig.DeviceMatch, "1");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                // 防止执法记录仪在验证过程中拔掉
                DeviveInfo selectDevice = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(deviveInfo.DeviceCode));
                if (selectDevice == null)
                {
                    NewMessageBox.Show(string.Format(TryFindResource("GatherDeviceRemove").ToString(), deviveInfo.DeviceCode), parentWindow);
                    return;
                }

                if (AppConfigInfos.LimitsUserInfos.UserType == "1")
                {
                    // 调度员可以帮其他人配对
                    WindowsHelper.ShowDialogWindow<MatchWindow>(parentWindow, deviveInfo);
                }
                else
                {
                    MatchDeviceByNormalPolice();
                }
            }
        }

        /// <summary>
        /// 正常警员配对
        /// </summary>
        private void MatchDeviceByNormalPolice(string forceMatch = "0")
        {
            // 普通警员：先判断是否此设备和自己配对，如果是则弹出配对窗口，里面只有自己可以选择，否则提示不能操作
            if (deviveInfo.IsMatchOrRegist == Model.IsMatchOrRegist.Matched)
            {
                EditorMatchByNormal();
            }
            else
            {
                Conditions con = new Conditions();
                con.AddItem("UserID", AppConfigInfos.LimitsUserInfos.UserID);
                con.AddItem("DeviceID", deviveInfo.DeviceCode);
                con.AddItem("UserName", AppConfigInfos.CurrentUserInfos.UserName);
                con.AddItem("UserCode", AppConfigInfos.CurrentUserInfos.UserCode);
                con.AddItem("OrgID", AppConfigInfos.CurrentUserInfos.OrgID);
                con.AddItem("OrgName", AppConfigInfos.CurrentUserInfos.OrgName);
                con.AddItem("ForceMatch", forceMatch);

                // 发送配对消息
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(parentWindow, MsgType.DevicePair, MsgType.DevicePairResult, con, TryFindResource("GatherPairing").ToString());
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(TryFindResource("GatherPairOverTime").ToString(), parentWindow);
                }
                else if (msgBoxResult == MessageBoxResult.Yes)
                {
                    NewMessageBox.Show(TryFindResource("GatherPairSuccess").ToString(), parentWindow);
                    ModelResponsible.Instance.SendOperationLog("CollectDevicePair");
                }
                else if (msgBoxResult == MessageBoxResult.No)
                {
                    DevicePairInfo devicePairInfo = resultWindow.ResultValue as DevicePairInfo;
                    if (devicePairInfo.ResultCode == 2)
                    {
                        // 已配对
                        MessageBoxResult messageBoxResult = NewMessageBox.Show(string.Format(TryFindResource("GatherAlreadyPair").ToString(), devicePairInfo.ResultMsg), MessageBoxButton.YesNo, parentWindow);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            MatchDeviceByNormalPolice("1");
                        }
                    }
                    else
                    {
                        string msg = (TryFindResource("GatherPairCode_" + devicePairInfo.ResultCode) ?? string.Empty).ToString();
                        msg = string.IsNullOrEmpty(msg) ? TryFindResource("GatherPairCode_-1").ToString() : msg;
                        NewMessageBox.Show(msg, parentWindow);
                    }
                }
            }
        }

        /// <summary>
        /// 普通警员不能编辑其他用户的设备，编辑自己的配对页面只能选择自己的信息
        /// </summary>
        private void EditorMatchByNormal()
        {
            if (deviveInfo.MatchUserID.Equals(AppConfigInfos.LimitsUserInfos.UserID))
            {
                AppConfigInfos.LimitsUserInfos.Users.UserList.Clear();
                UserInfos myInfo = new UserInfos()
                {
                    UserGuid = AppConfigInfos.LimitsUserInfos.UserGuid,
                    UserID = AppConfigInfos.LimitsUserInfos.UserID,
                    UserName = AppConfigInfos.LimitsUserInfos.UserName,
                    UserCode = AppConfigInfos.LimitsUserInfos.UserCode
                };

                AppConfigInfos.LimitsUserInfos.Users.UserList.Add(myInfo);
                WindowsHelper.ShowDialogWindow<MatchWindow>(parentWindow, deviveInfo);
            }
            else
            {
                NewMessageBox.Show(TryFindResource("GatherPairNotPow").ToString(), parentWindow);
            }
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        private void CollectOpertion(string permissionID, string disContent, string overTimeMsg, MsgType sendMsgType, MsgType receiveMsgType, string failedMsg)
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(parentWindow, permissionID, "1");
            if (loginWindow.MessageBoxResult != MessageBoxResult.OK)
            {
                return;
            }

            // 防止执法记录仪在验证过程中拔掉
            DeviveInfo selectDevice = GatherViewModel.DeviveInfoList.Find(p => p.DeviceCode.Equals(deviveInfo.DeviceCode));
            if (selectDevice == null)
            {
                NewMessageBox.Show(string.Format(TryFindResource("GatherDeviceRemove").ToString(), deviveInfo.DeviceCode), parentWindow);
                return;
            }

            if (AppConfigInfos.LimitsUserInfos.UserType == "1")
            {

                WindowsHelper.ShowDialogWindow<HandCollectWindow>(parentWindow, deviveInfo);
            }
            else
            {
                Conditions con = new Conditions();
                con.AddItem("DeviceID", deviveInfo.DeviceCode);
                con.AddItem("UserID", AppConfigInfos.LimitsUserInfos.UserID);

                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(parentWindow, sendMsgType, receiveMsgType, con, disContent);
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(overTimeMsg, parentWindow);
                }
                else if (msgBoxResult == MessageBoxResult.No)
                {
                    NewMessageBox.Show(failedMsg, parentWindow);
                }
                else
                {
                    // 操作日志
                    string code = "CollectHandGather";

                    ModelResponsible.Instance.SendOperationLog(code);
                }
            }
        }

        /// <summary>
        /// 查询回放
        /// </summary>
        private void DataSearch()
        {
            LoginWindow loginWindow = WindowsHelper.ShowDialogWindow<LoginWindow>(parentWindow, PermissionConfig.DataSearchMoudle, "1");
            if (loginWindow.MessageBoxResult == MessageBoxResult.OK)
            {
                Conditions con = new Conditions();
                con.AddItem("DeviceID", deviveInfo.DeviceCode);
                ModelResponsible.Instance.SendMsgToSelf(AppSelfMsgType.DataSearchPlay, JsonUnityConvert.SerializeObject(con));
            }
        }
    }
}
