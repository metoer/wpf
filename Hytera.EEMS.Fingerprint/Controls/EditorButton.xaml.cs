using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Fingerprint.Controls
{
    /// <summary>
    /// EditorButton.xaml 的交互逻辑
    /// </summary>
    public partial class EditorButton : UserControl
    {
        public EditorButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditorButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Text
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
            }
        }

        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty EditorButtonStatusProperty = DependencyProperty.Register("EditorButtonStatus", typeof(EditorButtonStatus), typeof(EditorButton), new PropertyMetadata(EditorButtonStatus.Reading, OnStateChanged));

        /// <summary>
        /// 编辑状态
        /// </summary>
        public EditorButtonStatus EditorButtonStatus
        {
            get
            {
                return (EditorButtonStatus)GetValue(EditorButtonStatusProperty);
            }
            set
            {
                SetValue(EditorButtonStatusProperty, value);
            }
        }

        /// <summary>
        /// 状态切换UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EditorButton editorButton = sender as EditorButton;
            EditorButtonStatus buttonStatus = (EditorButtonStatus)e.NewValue;

            switch (buttonStatus)
            {
                case EditorButtonStatus.Editoring:
                    editorButton.btnEditor.Visibility = Visibility.Collapsed;
                    editorButton.txtName.Visibility = Visibility.Visible;
                    editorButton.txtName.Focus();
                    editorButton.txtName.Select(editorButton.txtName.Text.Length, 0);
                    break;

                case EditorButtonStatus.Reading:
                    editorButton.btnEditor.Visibility = Visibility.Visible;
                    editorButton.txtName.Visibility = Visibility.Collapsed;
                    editorButton.btnEditor.Focus();
                    break;
            }

        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditor_Click(object sender, RoutedEventArgs e)
        {
            EditorButtonStatus = EditorButtonStatus.Editoring;
        }

        /// <summary>
        /// 失去焦点退出编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            EditorButtonStatus = EditorButtonStatus.Reading;
        }

    }

    public enum EditorButtonStatus
    {
        Editoring,
        Reading
    }
}
