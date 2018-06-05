using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Gather.Controls
{
    /// <summary>
    /// AutoComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class AutoComboBox : UserControl
    {
        /// <summary>
        /// 提示文本框
        /// </summary>
        HintTextBox hintTextBox;

        /// <summary>
        /// 文本改变通知
        /// </summary>
        public event TextChangedEventHandler TextChanged;

        /// <summary>
        /// 是否输入打开选项
        /// </summary>
        private bool IsHandOpened = false;

        /// <summary>
        /// 构造
        /// </summary>
        public AutoComboBox()
        {
            InitializeComponent();
            cmbMain.DisplayMemberPath = "UserName";
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
        public static readonly DependencyProperty SelectValueProperty = DependencyProperty.Register("SelectValue", typeof(string), typeof(AutoComboBox), new PropertyMetadata(string.Empty));

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
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoComboBox), new PropertyMetadata(string.Empty));

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
        public static readonly DependencyProperty PartTextProperty = DependencyProperty.Register("PartText", typeof(string), typeof(AutoComboBox), new PropertyMetadata(string.Empty));


        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (hintTextBox == null)
            {
                return;
            }

            cmbMain.ItemsSource = null;

            cmbMain.ItemsSource = GetItems(Text);

            SetPartText(Text);

            if (TextChanged != null)
            {
                TextChanged(this, null);
            }

            IsHandOpened = true;
            cmbMain.IsDropDownOpen = true;
            IsHandOpened = false;
        }

        /// <summary>
        /// 点击下拉按钮拉开显示全部，否则只显示匹配项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMain_DropDownOpened(object sender, System.EventArgs e)
        {
            if (!IsHandOpened)
            {
                cmbMain.ItemsSource = null;

                cmbMain.ItemsSource = AppConfigInfos.LimitsUserInfos.Users.UserList;
            }
        }

        /// <summary>
        /// 匹配对应的数据
        /// </summary>
        /// <param name="Pattern"></param>
        /// <returns></returns>
        private List<UserInfos> GetItems(string Pattern)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                return AppConfigInfos.LimitsUserInfos.Users.UserList;
            }
            else
            {
                // 配对只显示相对应的信息，已配对用户还需加入第一行空值提供取消配对
                return AppConfigInfos.LimitsUserInfos.Users.UserList.Where(
                                                                        (user, match) => user.UserName.Contains(Pattern)
                                                                        || user.UserCode.Contains(Pattern)
                                                                        || (InsertEmpty && string.IsNullOrEmpty(user.UserName)
                                                                        && string.IsNullOrEmpty(user.UserCode))).ToList();
            }
        }

        private void SetPartText(string Pattern)
        {
            UserInfos userInfo = AppConfigInfos.LimitsUserInfos.Users.UserList.Find(p => p.UserName.Equals(Pattern));
            if (userInfo != null)
            {
                PartText = userInfo.UserCode;
                SelectValue = userInfo.UserID;
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
            UserInfos userInfo = cmbMain.SelectedItem as UserInfos;
            if (userInfo != null)
            {
                hintTextBox.TextChanged -= PART_EditableTextBox_TextChanged;
                PartText = userInfo.UserCode;
                Text = userInfo.UserName;
                SelectValue = userInfo.UserID;
                cmbMain.ItemsSource = null;
                cmbMain.ItemsSource = GetItems(Text);
                hintTextBox.TextBox.Select(hintTextBox.Text.Length, 0);
                hintTextBox.TextChanged += PART_EditableTextBox_TextChanged;
            }
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void my_Loaded(object sender, RoutedEventArgs e)
        {
            // 加入空白行
            if (InsertEmpty)
            {
                UserInfos emptyInfo = new UserInfos()
                {
                    UserGuid = string.Empty,
                    UserID = string.Empty,
                    UserName = string.Empty,
                    UserCode = string.Empty
                };

                AppConfigInfos.LimitsUserInfos.Users.UserList.Insert(0, emptyInfo);
            }

            // 去掉admin
            UserInfos adminUser = AppConfigInfos.LimitsUserInfos.Users.UserList.Find(p => p.UserID.ToLower().Equals("admin"));
            if (adminUser != null)
            {
                AppConfigInfos.LimitsUserInfos.Users.UserList.Remove(adminUser);
            }

            hintTextBox = cmbMain.Template.FindName("PART_EditableTextBox", cmbMain) as HintTextBox;
            SetSelectTextByValue(SelectValue);
            cmbMain.ItemsSource = GetItems(Text);
        }

        /// <summary>
        /// 设置所选值
        /// </summary>
        /// <param name="value"></param>
        private void SetSelectTextByValue(string value)
        {
            if (AppConfigInfos.LimitsUserInfos == null || AppConfigInfos.LimitsUserInfos.Users == null || AppConfigInfos.LimitsUserInfos.Users.UserList == null || cmbMain == null)
            {
                return;
            }

            int index = AppConfigInfos.LimitsUserInfos.Users.UserList.FindIndex(p => p.UserID.Equals(value) || p.UserCode.Equals(value));
            if (index >= 0)
            {
                Text = AppConfigInfos.LimitsUserInfos.Users.UserList[index].UserName;
                PartText = AppConfigInfos.LimitsUserInfos.Users.UserList[index].UserCode;
            }
        }
    }
}
