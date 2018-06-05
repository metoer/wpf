using Hytera.EEMS.Manage.Enums;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// PlayComboBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayComboBoxControl : UserControl
    {
        HintTextBox hintTextBox;

        public event TextChangedEventHandler TextChanged;

        public List<ComBoxItem> Items = new List<ComBoxItem>();
        
        public PlayComboBoxControl()
        {
            InitializeComponent();
            cmbMain.DisplayMemberPath = "ItemName";
        }

        /// <summary>
        /// 是否插入空白行
        /// </summary>
        public bool InsertEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// 副本值
        /// </summary>
        public string SelectValue
        {
            get
            {
                return (string)GetValue(SelectValueProperty);
            }
            set
            {
                SetValue(SelectValueProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty SelectValueProperty = DependencyProperty.Register("SelectValue", typeof(string), typeof(PlayComboBoxControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 值
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
                if (TextChanged != null)
                {
                    TextChanged(this, null);
                }
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PlayComboBoxControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 副本值
        /// </summary>
        public string PartText
        {
            get
            {
                return (string)GetValue(PartTextProperty);
            }
            set
            {
                SetValue(PartTextProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty PartTextProperty = DependencyProperty.Register("PartText", typeof(string), typeof(PlayComboBoxControl), new PropertyMetadata(string.Empty));


        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (hintTextBox == null)
            //{
            //    return;
            //}
            
            SetPartText(Text);

            if (TextChanged != null)
            {
                TextChanged(this, null);
            }

            cmbMain.IsDropDownOpen = true;
        }
        
        private void SetPartText(string Pattern)
        {
            if (Pattern != null)
            {
                PartText = Pattern;
                SelectValue = Pattern;
            }
            else
            {
                PartText = string.Empty;
                SelectValue = string.Empty;
            }
        }

        /// <summary>
        /// 下拉框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComBoxItem userInfo = cmbMain.SelectedItem as ComBoxItem;
            if (userInfo != null)
            {
                PartText = userInfo.ItemName;
                SelectValue = userInfo.ItemCode;
                Text = userInfo.ItemName;
            }
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void my_Loaded(object sender, RoutedEventArgs e)
        {
            hintTextBox = cmbMain.Template.FindName("PART_EditableTextBox", cmbMain) as HintTextBox;
            SetSelectTextByValue(SelectValue);
            cmbMain.ItemsSource = Items;
        }

        private void SetSelectTextByValue(string value)
        {
            if (Items == null || cmbMain == null)
            {
                return;
            }

            int index = Items.FindIndex(p => p.Equals(value));
            if (index >= 0)
            {
                SelectValue = Items[index].ItemCode;
                Text = Items[index].ItemName;
                PartText = Items[index].ItemName;
            }
        }
    }
}
