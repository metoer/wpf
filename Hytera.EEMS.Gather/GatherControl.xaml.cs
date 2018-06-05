using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Gather.Controls;
using Hytera.EEMS.Gather.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Hytera.EEMS.Gather
{
    /// <summary>
    /// GatherControl.xaml 的交互逻辑
    /// </summary>
    public partial class GatherControl : UserControl
    {
        #region 属性、构造、变量
        /// <summary>
        /// 面板中的容器
        /// </summary>
        List<DeviceInfoItem> deviceControls = new List<DeviceInfoItem>();

        int lastRow, lasColumn = 0;

        bool isLoad = false;

        Window parentWindow;

        public GatherControl(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Hytera.EEMS.Gather;Component/Styles/DeviceStyles.xaml", UriKind.RelativeOrAbsolute) });
        }
        #endregion

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (isLoad)
            {
                return;
            }

            isLoad = true;

            bool result = CheckPortPairInfo();

            InitData();

            GatherViewModel.DeviveInfoList.CollectionChanged += DeviveInfoList_CollectionChanged;

            AppConfigInfos.AppStateInfos.PropertyChanged += AppStateInfos_PropertyChanged;

            AppConfigInfos.PortDeviceList.PropertyChanged += PortInfos_PropertyChanged;

            if (result && NewMessageBox.Show(TryFindResource("GatherPortNoPair").ToString(), MessageBoxButton.YesNo, parentWindow) == MessageBoxResult.Yes)
            {
                ModelResponsible.Instance.SendMsgToSelf(AppSelfMsgType.PortSet, string.Empty);
            }
        }

        /// <summary>
        /// 检查端口设置配对信息
        /// </summary>
        private bool CheckPortPairInfo()
        {
            if (AppConfigInfos.PortDeviceList.PortevList.Count < 1)
            {
                return false;
            }

            // 如果没有配对信息，则默认进行配对
            if (AppConfigInfos.PortPairInfos.Count == 0)
            {
                // 过滤第一个为空白的选项
                for (int i = 1; i < AppConfigInfos.PortDeviceList.PortevList.Count; i++)
                {
                    AppConfigInfos.PortPairInfos.Add(new PortPairInfo() { Index = i.ToString(), PortCode = AppConfigInfos.PortDeviceList.PortevList[i].PortName });
                }

                return false;
            }
            else
            {
                int showCount = AppConfigInfos.AppStateInfos.FaceplateRow * AppConfigInfos.AppStateInfos.FaceplateColumn;
                if (AppConfigInfos.PortPairInfos.Count < showCount && AppConfigInfos.PortDeviceList.PortevList.Count >= showCount)
                {
                    return true;
                }

                return false;
            }

        }

        /// <summary>
        /// 当物理端口发生改变时，重新检测端口配对和重新摆列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortInfos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChanged"))
            {
                deviceControls.ForEach(p => p.ClearData());
                CheckPortPairInfo();
                InitData();

                deviceControls.ForEach(p => p.IsFirstPort = false);
                var devControl = deviceControls.Find(p => p.Index.Equals(AppConfigInfos.PortDeviceList.FirstPort));
                if (devControl != null)
                {
                    devControl.IsFirstPort = true;
                }
            }
        }

        /// <summary>
        /// 设置行或列改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppStateInfos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FaceplateRow") || e.PropertyName.Equals("FaceplateColumn"))
            {
                InitData();
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            CreateDeviceUI();

            AddDeviceInfo(GatherViewModel.DeviveInfoList);
        }

        /// <summary>
        /// 创建UI(以面板2边固定拉升中间空隙排版)
        /// </summary>
        private void CreateDeviceUI()
        {
            int row = AppConfigInfos.AppStateInfos.FaceplateRow;
            int column = AppConfigInfos.AppStateInfos.FaceplateColumn;
            if (lasColumn == column && lastRow == row)
            {
                return;
            }



            lastRow = row;
            lasColumn = column;

            deviceControls.Clear();
            canvasMain.Children.Clear();

            // 设置卡片的大小和空格
            int itemWith = 318;
            int itemHeight = 181;
            double spaceHeight = 0;
            double spaceWidth = 0;

            canvasMain.Width = column * (itemWith + spaceWidth);

            canvasMain.Height = row * (itemHeight + spaceHeight);

            sv.HorizontalScrollBarVisibility = (column <= 4 && canvasMain.Width < my.ActualWidth) ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = (row <= 5 && canvasMain.Height < my.ActualHeight) ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    int index = i * column + j + 1;

                    DeviceInfoItem deviceInfoItem = new DeviceInfoItem()
                    {
                        Index = index.ToString(),
                        Width = itemWith,
                        Height = itemHeight,
                        IsFirstPort = index.ToString().Equals(AppConfigInfos.PortDeviceList.FirstPort)
                    };

                    deviceControls.Add(deviceInfoItem);

                    double left = (itemWith + spaceWidth) * j;
                    double top = (itemHeight + spaceHeight) * i;

                    Canvas.SetLeft(deviceInfoItem, left);
                    Canvas.SetTop(deviceInfoItem, top);
                    canvasMain.Children.Add(deviceInfoItem);
                }
            }
        }

        /// <summary>
        /// 集合发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviveInfoList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddDeviceInfo(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveDeviceInfo(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    AllClearDevices();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 绑定信息到面板中
        /// </summary>
        /// <param name="items"></param>
        private void AddDeviceInfo(IList items)
        {
            foreach (var item in items)
            {
                DeviveInfo deviveInfo = item as DeviveInfo;

                PortIsDevice portIsDevice = AppConfigInfos.PortDeviceList.PortevList.Find(p => p.PortName.Equals(deviveInfo.PortCode));
                if (portIsDevice != null)
                {
                    portIsDevice.IsDeviceInfo = true;
                }

                // 寻找在面板的位置编号
                PortPairInfo portInfo = AppConfigInfos.PortPairInfos.Find(p => p.PortCode.Equals(deviveInfo.PortCode));

                if (portInfo == null)
                {
                    return;
                }

                DeviceInfoItem deviceInfoItem = deviceControls.Find(p => p.Index.ToString().Equals(portInfo.Index));

                if (deviceInfoItem != null)
                {
                    deviceInfoItem.BingData(deviveInfo, parentWindow);
                }
            }
        }

        /// <summary>
        /// 清除所有的执法记录仪信息
        /// </summary>
        private void AllClearDevices()
        {
            AppConfigInfos.PortDeviceList.PortevList.ForEach(p => p.IsDeviceInfo = false);
            deviceControls.ForEach(p => p.ClearData());
        }

        /// <summary>
        /// 状态改变修改统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deviveInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DeviceState") || e.PropertyName.Equals("IsMatchOrRegist"))
            {
                int unMatch = 0, unRegister = 0, Collected = 0;
                foreach (var item in GatherViewModel.DeviveInfoList)
                {
                    switch (item.DeviceState)
                    {
                        case DeviceState.CollectFinish:
                            Collected++;
                            break;

                        default:
                            break;
                    }

                    switch (item.IsMatchOrRegist)
                    {
                        case IsMatchOrRegist.UnRegister:
                            unRegister++;
                            break;

                        case IsMatchOrRegist.Registered:
                            unMatch++;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 移除面板中的信息
        /// </summary>
        private void RemoveDeviceInfo(IList items)
        {
            foreach (var item in items)
            {
                DeviveInfo deviveInfo = item as DeviveInfo;

                PortIsDevice portIsDevice = AppConfigInfos.PortDeviceList.PortevList.Find(p => p.PortName.Equals(deviveInfo.PortCode));
                if (portIsDevice != null)
                {
                    portIsDevice.IsDeviceInfo = false;
                }

                // 寻找在面板的位置编号
                PortPairInfo portInfo = AppConfigInfos.PortPairInfos.Find(p => p.PortCode.Equals(deviveInfo.PortCode));

                if (portInfo == null)
                {
                    return;
                }

                DeviceInfoItem deviceInfoItem = deviceControls.Find(p => p.Index.ToString().Equals(portInfo.Index));

                if (deviceInfoItem != null)
                {
                    deviceInfoItem.ClearData();
                }
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sv.HorizontalScrollBarVisibility = (lasColumn <= 4 && canvasMain.Width < my.ActualWidth) ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = (lastRow <= 5 && canvasMain.Height < my.ActualHeight) ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
        }
    }
}
