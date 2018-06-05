using Hytera.EEMS.AppLib;
using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Documents;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// SetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetWindow : BaseWindow
    {
        /// <summary>
        /// 构造
        /// </summary>
        public SetWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> values = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            rollRow.Values = values;
            rollColumn.Values = values;
            rollRow.Text = AppConfigInfos.AppStateInfos.FaceplateRow.ToString();
            rollColumn.Text = AppConfigInfos.AppStateInfos.FaceplateColumn.ToString();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            AppConfigInfos.AppStateInfos.FaceplateColumn = Convert.ToInt32(rollColumn.Text);
            AppConfigInfos.AppStateInfos.FaceplateRow = Convert.ToInt32(rollRow.Text);
            AppConfigHelper.SaveInfoToFile("AppConfig/SetInfo");
            this.Close();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
