using Hytera.EEMS.Manage.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// AutoComboBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class AutoComboBoxControl : UserControl
    {
        HintTextBox hintTextBox;

        public event TextChangedEventHandler TextChanged;

        public List<ComBoxItem> Items = new List<ComBoxItem>();
        
        public AutoComboBoxControl()
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

        public bool TextEnabled
        {
            get
            {
                return (bool)GetValue(TextEnabledProperty);
            }
            set
            {
                SetValue(TextEnabledProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TextEnabledProperty = DependencyProperty.Register("TextEnabled", typeof(bool), typeof(AutoComboBoxControl), new PropertyMetadata(false));

        public bool CodeVisibility
        {
            get
            {
                return (bool)GetValue(CodeVisibilityProperty);
            }
            set
            {
                SetValue(CodeVisibilityProperty, value);
            }
        }
        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty CodeVisibilityProperty = DependencyProperty.Register("CodeVisibility", typeof(bool), typeof(AutoComboBoxControl), new PropertyMetadata(false));

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
        public static readonly DependencyProperty SelectValueProperty = DependencyProperty.Register("SelectValue", typeof(string), typeof(AutoComboBoxControl), new PropertyMetadata(string.Empty));

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
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoComboBoxControl), new PropertyMetadata(string.Empty));

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
        public static readonly DependencyProperty PartTextProperty = DependencyProperty.Register("PartText", typeof(string), typeof(AutoComboBoxControl), new PropertyMetadata(string.Empty));


        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextEnabled)
            {
                cmbMain.ItemsSource = null;
                cmbMain.ItemsSource = GetItems(Text);
            }
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
                Text = userInfo.ItemName;
                SelectValue = userInfo.ItemCode;
                if (TextEnabled)
                {
                    cmbMain.ItemsSource = null;
                    cmbMain.ItemsSource = GetItems(Text);
                }
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
            if (TextEnabled)
                cmbMain.ItemsSource = GetItems(Text);
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
                Text = Items[index].ItemName;
                PartText = Items[index].ItemName;
                SelectValue = Items[index].ItemCode;
            }
        }

        private List<ComBoxItem> GetItems(string Pattern)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                return Items;
            }
            else
            {
                return Items.Where((user, match) => user.ItemID.ToLower().Contains(Pattern.ToLower()) || user.ItemCode.ToLower().Contains(Pattern.ToLower()) || user.ItemName.ToLower().Contains(Pattern.ToLower())).ToList();
            }
        }
    }
}
