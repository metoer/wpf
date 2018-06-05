using System.Windows;
using System.Windows.Controls;

namespace Hytera.EEMS.Fingerprint.Controls
{
    /// <summary>
    /// EditorFingerControl.xaml 的交互逻辑
    /// </summary>
    public partial class EditorFingerControl : UserControl
    {
        public EditorFingerControl()
        {
            InitializeComponent();
        }

        public event TextChangedEventHandler TextChanged;

        public event RoutedEventHandler DeleteClick;

        /// <summary>
        /// 指纹ID
        /// </summary>
        public string ID
        {
            get;
            set;
        }


        /// <summary>
        /// 注册消息按钮样式属性
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditorFingerControl), new PropertyMetadata(string.Empty));

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
        public static readonly DependencyProperty EditorButtonStatusProperty = DependencyProperty.Register("EditorButtonStatus", typeof(EditorButtonStatus), typeof(EditorFingerControl), new PropertyMetadata(EditorButtonStatus.Reading, OnStateChanged));

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

        private static void OnStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EditorFingerControl editorFingerControl = sender as EditorFingerControl;
            EditorButtonStatus buttonStatus = (EditorButtonStatus)e.NewValue;

            switch (buttonStatus)
            {
                case EditorButtonStatus.Editoring:
                    editorFingerControl.btnEditorName.Visibility = Visibility.Collapsed;
                    editorFingerControl.txtName.Visibility = Visibility.Visible;
                    editorFingerControl.txtName.Focus();
                    editorFingerControl.txtName.Select(editorFingerControl.txtName.Text.Length, 0);
                    editorFingerControl.btnEditor.IsSelect = true;
                    break;

                case EditorButtonStatus.Reading:
                    editorFingerControl.btnEditorName.Visibility = Visibility.Visible;
                    editorFingerControl.txtName.Visibility = Visibility.Collapsed;
                    editorFingerControl.btnEditor.IsSelect = false;
                    editorFingerControl.btnEditorName.Focus();
                    break;
            }
        }

        private void btnEditor_Click(object sender, RoutedEventArgs e)
        {
            EditorButtonStatus = EditorButtonStatus == EditorButtonStatus.Reading ? EditorButtonStatus.Editoring : EditorButtonStatus.Reading;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteClick != null)
            {
                DeleteClick(this, e);
            }
        }
    }
}
