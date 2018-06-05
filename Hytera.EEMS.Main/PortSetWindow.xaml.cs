using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Main.Controls;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using Hytera.EEMS.Resources;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// PortSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PortSetWindow : BaseWindow
    {
        bool isChangedValue = false;

        bool firstChanged = false;

        List<PortControl> PortControlList = new List<PortControl>();
        public PortSetWindow()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreatGrid();
            isChangedValue = false;
            firstChanged = false;
        }

        private void CreatGrid()
        {
            Dictionary<string, string> firstSource = new Dictionary<string, string>(); // 优先端口
            firstSource.Add("0", "----");

            int row = AppConfigInfos.AppStateInfos.FaceplateRow;
            int column = AppConfigInfos.AppStateInfos.FaceplateColumn;

            sv.HorizontalScrollBarVisibility = column <= 4 ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = row <= 5 ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;

            int itemWith = 173;
            int itemHeight = 81;
            double spaceHeight = 5;
            double spaceWidth = 5;

            canvasMain.Width = column * (itemWith + spaceWidth);
            canvasMain.Height = row * (itemHeight + spaceHeight);

            Style syPort = TryFindResource("syPort") as Style;
            string portMark = TryFindResource("appMainInventedPort").ToString();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    int index = i * column + j + 1;

                    PortControl portControl = new PortControl()
                    {
                        Name = "prot" + index,
                        InventedIndex = index,
                        Width = itemWith,
                        Height = itemHeight,
                        Style = syPort
                    };

                    portControl.Source = AppConfigInfos.PortDeviceList.PortevList;

                    PortPairInfo portPairInfo = AppConfigInfos.PortPairInfos.Find(p => p.Index.Equals(index.ToString()));
                    if (portPairInfo != null)
                    {
                        portControl.OldPhysicsCode = portPairInfo.PortCode;
                    }

                    firstSource.Add(index.ToString(), portMark + index);
                    PortControlList.Add(portControl);

                    portControl.SelectedHandler += deviceInfoItem_SelectedHandler;
                    portControl.ChangeValueHandler += portControl_ChangeValueHandler;

                    double left = (itemWith + spaceWidth) * j;
                    double top = (itemHeight + spaceHeight) * i;

                    Canvas.SetLeft(portControl, left);
                    Canvas.SetTop(portControl, top);
                    canvasMain.Children.Add(portControl);
                }
            }

            cbFirst.SelectedValuePath = "Key";
            cbFirst.DisplayMemberPath = "Value";
            cbFirst.ItemsSource = firstSource;

            if (string.IsNullOrEmpty(AppConfigInfos.PortDeviceList.FirstPort))
            {
                cbFirst.SelectedIndex = 0;
            }
            else
            {
                cbFirst.SelectedValue = AppConfigInfos.PortDeviceList.FirstPort;
            }
        }

        /// <summary>
        /// 界面值是否发生过修改
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void portControl_ChangeValueHandler(object arg1, string arg2)
        {
            isChangedValue = true;
        }

        /// <summary>
        /// 按钮选中
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void deviceInfoItem_SelectedHandler(object arg1, bool arg2)
        {
            PortControl portControl = arg1 as PortControl;
            foreach (PortControl item in canvasMain.Children)
            {
                item.IsSelected = item.Name.Equals(portControl.Name);
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            if (!isChangedValue && !firstChanged)
            {
                this.Close();
                return;
            }

            if (!AppHelper.CheckAppState(this))
            {
                return;
            }

            if (firstChanged)
            {
                // 设置优先端口
                string portIndex = (cbFirst.SelectedValue ?? string.Empty).ToString();
                string portCode = string.Empty;
                PortPairInfo portPairInfo = AppConfigInfos.PortPairInfos.Find(p => p.Index.Equals(portIndex));
                if (portPairInfo != null)
                {
                    portCode = portPairInfo.PortCode.Equals("----") ? string.Empty : portPairInfo.PortCode;
                }

                Conditions con = new Conditions();
                con.AddItem("PortCode", portCode);
                con.AddItem("Respond", "0");

                // 发送配对消息
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.SetFirstPortRequest, MsgType.SetFirstPortRespond, con, TryFindResource("appMainSetFirstPorting").ToString());
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(TryFindResource("appMainSetPortOvertime").ToString(), this);
                    return;
                }
                else if (msgBoxResult == MessageBoxResult.Yes)
                {
                    AppConfigInfos.PortDeviceList.FirstPort = portIndex;
                    AppConfigInfos.PortDeviceList.FirstPortCode = portCode;
                    AppConfigHelper.SaveFirstPort();

                }
                else if (msgBoxResult == MessageBoxResult.No)
                {
                    NewMessageBox.Show(TryFindResource("appMainSetPortFailed").ToString(), this);
                    return;
                }
            }

            if (isChangedValue)
            {
                AppConfigHelper.SavePortPairInfos();
            }

            AppConfigInfos.PortDeviceList.IsChanged = !AppConfigInfos.PortDeviceList.IsChanged;
            this.Close();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            if ((!isChangedValue && !firstChanged) || ((isChangedValue || firstChanged) && NewMessageBox.Show(TryFindResource("appMainIsSaveData").ToString(), MessageBoxButton.OKCancel, this) == MessageBoxResult.OK))
            {
                PortControlList.ForEach(p => p.ResetValue()); // 重置数据
                this.Close();
            }

        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            PortControlList.ForEach(p => p.ResetValue());
        }

        private void cbFirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            firstChanged = true;
        }


    }
}
