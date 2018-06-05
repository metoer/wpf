using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// AutoComboBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewControl : UserControl
    {
        TreeView tv;

        public event TextChangedEventHandler TextChanged;
        private ObservableCollection<OrgInfos> Items;

        public TreeViewControl()
        {
            InitializeComponent();
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
        public static readonly DependencyProperty SelectValueProperty = DependencyProperty.Register("SelectValue", typeof(string), typeof(TreeViewControl), new PropertyMetadata(string.Empty));

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
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TreeViewControl), new PropertyMetadata(string.Empty));


        private System.Threading.Timer timer = null;
        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, null);
            }
            if (timer == null)
            {
                timer = new System.Threading.Timer(new TimerCallback(EditableTextBoxChanged), null, 500, Timeout.Infinite);
            }
            else
            {
                timer.Change(500, Timeout.Infinite);
            }
            
        }

        private void EditableTextBoxChanged(object state)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
               if (AppConfigInfos.LimitsUserInfos.OrgList == null|| string.IsNullOrEmpty(Text))
                    return;
                OrgInfos sub = AppConfigInfos.LimitsUserInfos.OrgList.orgList.Find(oi => oi.OrgIDCode.ToLower().Contains(Text.ToLower()) || oi.OrgName.ToLower().Contains(Text.ToLower()));
                if (tv == null || sub == null)
                {
                    Text = string.Empty;
                    return;
                }
                if (cmbMain.SelectedItem != null && (((OrgInfos)cmbMain.SelectedItem).OrgIDCode.ToLower().Contains(Text.ToLower()) || ((OrgInfos)cmbMain.SelectedItem).OrgName.ToLower().Contains(Text.ToLower())))
                    sub = cmbMain.SelectedItem as OrgInfos;
                FindTreeViewItem(tv, sub);
                cmbMain.IsDropDownOpen = true;
            }));
        }
        private static TreeViewItem FindTreeViewItem(ItemsControl item, object data)
        {
            TreeViewItem findItem = null;
            bool itemIsExpand = false;
            if (item is TreeViewItem)
            {
                TreeViewItem tviCurrent = item as TreeViewItem;
                itemIsExpand = tviCurrent.IsExpanded;
                if (!tviCurrent.IsExpanded)
                {
                    //如果这个TreeViewItem未展开过，则不能通过ItemContainerGenerator来获得TreeViewItem
                    tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, true);
                    //必须使用UpdaeLayour才能获取到TreeViewItem
                    tviCurrent.UpdateLayout();
                }
            }
            for (int i = 0; i < item.Items.Count; i++)
            {
               TreeViewItem tvItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
                if (tvItem == null)
                    continue;
                object itemData = item.Items[i];
                if (itemData == data)
                {
                    findItem = tvItem;
                    findItem.IsSelected = true;
                    break;
                }
                else if (tvItem.Items.Count > 0)
                {
                    findItem = FindTreeViewItem(tvItem, data);
                    if (findItem != null)
                        break;
                }


            }
            if (findItem == null)
            {
                TreeViewItem tviCurrent = item as TreeViewItem;
                if (tviCurrent != null)
                {
                    tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, itemIsExpand);
                    tviCurrent.UpdateLayout();
                }
            }
            return findItem;
        }
       

        /// <summary>
        /// 下拉框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrgInfos userInfo = cmbMain.SelectedItem as OrgInfos;
            if (userInfo != null)
            {
                Text = userInfo.OrgName;
                SelectValue = FindSubOrgID(userInfo) + userInfo.OrgID;
            }
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void my_Loaded(object sender, RoutedEventArgs e)
        {
            tv = cmbMain.Template.FindName("tree", cmbMain) as TreeView;
            if (Items == null)
            {
                Items = new ObservableCollection<OrgInfos>();
                int LevelCode = 1000;
                OrgInfos oi = new OrgInfos() { OrgID = AppConfigInfos.LimitsUserInfos.OrgID, OrgIDCode = AppConfigInfos.LimitsUserInfos.OrgIDCode, OrgName = AppConfigInfos.LimitsUserInfos.OrgName, IsExpanded = false, IsSelected = true, Level = 1, Children = new List<OrgInfos>() };

                LogHelper.Instance.WirteLog(string.Format("TreeViewControl Loaded OrgList Count:{0}", AppConfigInfos.LimitsUserInfos.OrgList.orgList.Count), LogLevel.LogDebug);

                //调度员方才创建机构树
                if (AppConfigInfos.LimitsUserInfos.UserType == "1")
                {
                    foreach (OrgInfos os in AppConfigInfos.LimitsUserInfos.OrgList.orgList)
                    {
                        if (!string.IsNullOrEmpty(os.LevelCode) && Int32.Parse(os.LevelCode) < LevelCode)
                        {
                            LevelCode = Int32.Parse(os.LevelCode);
                            oi = os;
                        }
                    }

                    oi.Level = 1;
                    oi.IsExpanded = false;
                    oi.IsSelected = true;
                    if (string.IsNullOrEmpty(Text))
                        Text = oi.OrgName;
                    FindOrgChildren(oi);
                }

                Items.Add(oi);
            }
            cmbMain.ItemsSource = Items;
            if (tv != null)
                tv.ItemsSource = Items;
        }

        private void FindOrgChildren(OrgInfos oi)
        {
            if (AppConfigInfos.LimitsUserInfos.OrgList == null)
                return;
            oi.Children.Clear();
            List<OrgInfos> sub = AppConfigInfos.LimitsUserInfos.OrgList.orgList.Where((org, match) => oi.OrgID.Equals(org.ParentID)).ToList();
            if (sub != null)
            {
                LogHelper.Instance.WirteLog(string.Format("TreeViewControl Loaded OrgList OrgInfos OrgID:{1} SubCount:{0}", sub.Count, oi.OrgID), LogLevel.LogDebug);
                foreach (OrgInfos suboi in sub)
                {
                    suboi.Level = oi.Level + 1;
                    suboi.Children = new List<OrgInfos>();
                    oi.Children.Add(suboi);
                    oi.HasItem = true;
                    FindOrgChildren(suboi);
                }
            }
            else
                return;
        }
        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tv.SelectedItem != null)
            {
                Text = ((OrgInfos)tv.SelectedItem).OrgName;
                SelectValue = FindSubOrgID((OrgInfos)tv.SelectedItem) + ((OrgInfos)tv.SelectedItem).OrgID;
            }
            else
            {
                Text = string.Empty;
                SelectValue = string.Empty;
            }
        }

        private string FindSubOrgID(OrgInfos selectedItem)
        {
            string ordIDstr = string.Empty;
            foreach (OrgInfos oi in selectedItem.Children)
            {
                ordIDstr += oi.OrgID + ",";
                ordIDstr += FindSubOrgID(oi);
            }
            return ordIDstr;
        }
    }
}
