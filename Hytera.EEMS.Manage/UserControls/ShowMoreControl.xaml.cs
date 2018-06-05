using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Manage.UserControls
{

    /// <summary>
    /// ShowMoreControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShowMoreControl : UserControl
    {
        public static readonly DependencyProperty ShowMoreTextProperty = DependencyProperty.Register("ShowMoreText", typeof(string), typeof(ShowMoreControl));
        public string ShowMoreText
        {
            get
            {
                return (string)GetValue(ShowMoreTextProperty);
            }
            set
            {
                SetValue(ShowMoreTextProperty, value);
            }
        }
        public ShowMoreControl()
        {
            InitializeComponent();
        }
    }
}
