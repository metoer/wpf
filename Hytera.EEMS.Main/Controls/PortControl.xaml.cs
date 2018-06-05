using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model.Models;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hytera.EEMS.Main.Controls
{
    /// <summary>
    /// PortControl.xaml 的交互逻辑
    /// </summary>
    public partial class PortControl : UserControl
    {
        /// <summary>
        /// 当前按钮是否选中状态
        /// </summary>
        public event Action<object, bool> SelectedHandler;

        /// <summary>
        /// 值改变状态
        /// </summary>
        public event Action<object, string> ChangeValueHandler;

        /// <summary>
        /// 上一次选值
        /// </summary>
        string lastOldValue = string.Empty;

        ComboBox comboBox;

        public PortControl()
        {
            InitializeComponent();
            this.AddHandler(ComboBox.MouseDownEvent, new MouseButtonEventHandler(UserControl_MouseDown), true);

        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PortControl), new PropertyMetadata(false));

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty InventedIndexProperty = DependencyProperty.Register("InventedIndex", typeof(int), typeof(PortControl), new PropertyMetadata(0));

        /// <summary>
        /// 虚拟编号
        /// </summary>
        public int InventedIndex
        {
            get
            {
                return (int)GetValue(InventedIndexProperty);
            }
            set
            {
                SetValue(InventedIndexProperty, value);
            }
        }

        /// <summary>
        /// 物理端口编码
        /// </summary>
        public string OldPhysicsCode
        {
            get;
            set;
        }

        /// <summary>
        /// 物理端口编码
        /// </summary>
        public string NewPhysicsCode
        {
            get;
            set;
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(List<PortIsDevice>), typeof(PortControl));

        /// <summary>
        /// 端口编号集合
        /// </summary>
        public List<PortIsDevice> Source
        {
            get
            {
                return (List<PortIsDevice>)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected)
            {
                return;
            }

            IsSelected = true;
            if (SelectedHandler != null)
            {
                SelectedHandler(this, true);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox = this.Template.FindName("cbPort", this) as ComboBox;
            comboBox.SelectedValue = OldPhysicsCode;
            lastOldValue = OldPhysicsCode;
            comboBox.SelectionChanged += comboBox_SelectionChanged;
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewPhysicsCode = (comboBox.SelectedValue ?? string.Empty).ToString();
            PortPairInfo portPairInfo;
            if (!NewPhysicsCode.Equals("-----"))
            {
                portPairInfo = AppConfigInfos.PortPairInfos.Find(p => p.PortCode.Equals(NewPhysicsCode) && !p.Index.Equals(InventedIndex.ToString()));
                if (portPairInfo != null)
                {
                    int countPort = AppConfigInfos.AppStateInfos.FaceplateColumn * AppConfigInfos.AppStateInfos.FaceplateRow;
                    // 只提示在面板中有显示的端口占用情况
                    if (Convert.ToInt32(portPairInfo.Index) <= countPort)
                    {
                        comboBox.SelectionChanged -= comboBox_SelectionChanged;
                        comboBox.SelectedValue = lastOldValue;
                        comboBox.SelectionChanged += comboBox_SelectionChanged;
                        NewMessageBox.Show(string.Format(TryFindResource("GatherPortRePair").ToString(), portPairInfo.Index));
                        return;
                    }
                    else
                    {
                        portPairInfo.PortCode = lastOldValue;
                    }
                }
            }

            portPairInfo = AppConfigInfos.PortPairInfos.Find(p => p.Index.Equals(InventedIndex.ToString()));
            if (portPairInfo == null)
            {
                AppConfigInfos.PortPairInfos.Add(new PortPairInfo() { Index = InventedIndex.ToString(), PortCode = NewPhysicsCode });
            }
            else
            {
                portPairInfo.PortCode = NewPhysicsCode;
            }


            if (ChangeValueHandler != null)
            {
                ChangeValueHandler(this, NewPhysicsCode);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetValue()
        {
            comboBox.SelectionChanged -= comboBox_SelectionChanged;
            comboBox.SelectedValue = OldPhysicsCode;
            lastOldValue = OldPhysicsCode;

            PortPairInfo portPairInfo = AppConfigInfos.PortPairInfos.Find(p => p.PortCode.Equals(OldPhysicsCode) && p.Index.Equals(InventedIndex.ToString()));
            if (portPairInfo != null)
            {
                if (string.IsNullOrEmpty(OldPhysicsCode))
                {
                    AppConfigInfos.PortPairInfos.Remove(portPairInfo);
                }
                else
                {
                    portPairInfo.PortCode = OldPhysicsCode;
                }
            }

            comboBox.SelectionChanged += comboBox_SelectionChanged;
        }

    }
}
