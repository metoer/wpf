using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Hytera.EEMS.Fingerprint.Controls
{
    /// <summary>
    /// TreeComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class TreeComboBox : UserControl
    {
        public TreeComboBox()
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
        public static readonly DependencyProperty SelectValueProperty = DependencyProperty.Register("SelectValue", typeof(string), typeof(TreeComboBox), new PropertyMetadata(string.Empty));

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView.SelectedItem != null)
            {
                treeComboBox.Text = ((OrgInfos)treeView.SelectedItem).OrgName;
                SelectValue = ((OrgInfos)treeView.SelectedItem).OrgID;
            }
            else
            {
                treeComboBox.Text = string.Empty;
                SelectValue = string.Empty;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<OrgInfos> OrgInfoList = new ObservableCollection<OrgInfos>();
            int levelCode = 1000;
            OrgInfos orgInfo = new OrgInfos()
            {
                OrgID = AppConfigInfos.LimitsUserInfos.OrgID,
                OrgIDCode = AppConfigInfos.LimitsUserInfos.OrgIDCode,
                OrgName = AppConfigInfos.LimitsUserInfos.OrgName,
                IsExpanded = false,
                IsSelected = true,
                Level = 1,
                Children = new List<OrgInfos>()
            };

            //调度员方才创建机构树
            if (AppConfigInfos.LimitsUserInfos.UserType == "1")
            {
                foreach (OrgInfos os in AppConfigInfos.LimitsUserInfos.OrgList.orgList)
                {
                    if (!string.IsNullOrEmpty(os.LevelCode) && Int32.Parse(os.LevelCode) < levelCode)
                    {
                        levelCode = Int32.Parse(os.LevelCode);
                        orgInfo = os;
                    }
                }

                orgInfo.Level = 1;
                orgInfo.IsExpanded = false;
                orgInfo.IsSelected = true;
                FindOrgChildren(orgInfo);
            }

            OrgInfoList.Add(orgInfo);

            treeComboBox.ItemsSource = null;
            treeComboBox.ItemsSource = OrgInfoList;
            treeComboBox.Text = orgInfo.OrgName;
            SelectValue = orgInfo.OrgID;

            TreeView treeView = treeComboBox.Template.FindName("tree", treeComboBox) as TreeView;
            if (treeView != null)
            {
                treeView.ItemsSource = OrgInfoList;
            }
        }

        private void FindOrgChildren(OrgInfos orgInfo)
        {
            if (AppConfigInfos.LimitsUserInfos.OrgList == null)
            {
                return;
            }

            orgInfo.Children.Clear();

            List<OrgInfos> orgInfoChild = AppConfigInfos.LimitsUserInfos.OrgList.orgList.Where((org, match) => orgInfo.OrgID.Equals(org.ParentID)).ToList();
            if (orgInfoChild != null)
            {
                foreach (OrgInfos item in orgInfoChild)
                {
                    item.Level = orgInfo.Level + 1;
                    item.Children = new List<OrgInfos>();
                    orgInfo.Children.Add(item);
                    orgInfo.HasItem = true;
                    FindOrgChildren(item);
                }
            }
        }
    }
}
