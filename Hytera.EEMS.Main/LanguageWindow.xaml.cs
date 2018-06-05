using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Main.Lib;
using Hytera.EEMS.Resources.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// LanguageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LanguageWindow : BaseWindow
    {
        /// <summary>
        /// 切换的语言ID
        /// </summary>
        string lanuageID = string.Empty;

        /// <summary>
        /// 构造
        /// </summary>
        public LanguageWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int num = AppConfigInfos.LanguageList.Count / 2;
            num = AppConfigInfos.LanguageList.Count % 2 == 0 ? num : ++num;
            if (num > 1)
            {
                this.Height = this.Height + (num - 1) * 60;
            }

            for (int i = 1; i < num; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60, GridUnitType.Pixel) });
            }

            Style styleBtn = TryFindResource("sySetLan") as Style;

            for (int i = 0; i < AppConfigInfos.LanguageList.Count; i++)
            {
                SelectButton button = new SelectButton()
                {
                    Name = AppConfigInfos.LanguageList[i].ID,
                    IsSelect = AppConfigInfos.LanguageList[i].IsChecked,
                    Style = styleBtn,
                    Content = AppConfigInfos.LanguageList[i].DisplayName,
                    Tag = AppConfigInfos.LanguageList[i].ID,
                    HorizontalAlignment = i % 2 == 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                grid.Children.Add(button);

                Grid.SetRow(button, i / 2);
                Grid.SetColumn(button, i % 2);

                button.Click += button_Click;
            }
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectButton button = sender as SelectButton;
            if (button.IsSelect)
            {
                return;
            }

            foreach (SelectButton item in grid.Children)
            {
                item.IsSelect = button.Name.Equals(item.Name);
            }

            lanuageID = button.Tag.ToString();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            ThemesHelper.SetLanguage(lanuageID);

            AppConfigHelper.SaveLanguageInfo(lanuageID);

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
