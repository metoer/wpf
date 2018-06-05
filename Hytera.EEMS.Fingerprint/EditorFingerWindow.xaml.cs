using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Fingerprint.Controls;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Controls;
using Hytera.EEMS.Resources.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Hytera.EEMS.Fingerprint
{
    /// <summary>
    /// EditorFingerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditorFingerWindow : BaseWindow
    {
        List<EditorFingerControl> controls = new List<EditorFingerControl>();
        UserInfos userInfo;

        bool isClose = false;
        public EditorFingerWindow(UserInfos userInfo)
        {
            InitializeComponent();
            this.userInfo = userInfo;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in userInfo.Fingers)
            {
                EditorFingerControl editorFingerControl = new EditorFingerControl();
                editorFingerControl.Text = item.FingerName;
                editorFingerControl.ID = item.FingerID;
                editorFingerControl.Margin = new Thickness(0, 25, 0, 0);
                editorFingerControl.TextChanged += editorFingerControl_TextChanged;
                editorFingerControl.DeleteClick += editorFingerControl_DeleteClick;
                panel.Children.Add(editorFingerControl);
                controls.Add(editorFingerControl);
            }
            this.MouseDown += EditorFingerWindow_MouseDown;
        }

        private void EditorFingerWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is EditorButton))
            {
                controls.ForEach(p => p.EditorButtonStatus = EditorButtonStatus.Reading);
            }
        }

        private void editorFingerControl_DeleteClick(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            EditorFingerControl editorFingerControl = sender as EditorFingerControl;
            panel.Children.Remove(editorFingerControl);
            controls.Remove(editorFingerControl);
        }

        void editorFingerControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbMsg.Text = string.Empty;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            string delValue = string.Empty;
            string editorValue = string.Empty;
            List<Finger> newFingers = GetNewFinger();
            GetChangeValue(ref delValue, ref editorValue, userInfo.Fingers, newFingers);

            Conditions con = new Conditions();
            con.AddItem("UserID", userInfo.UserID);
            con.AddItem("FingersEditor", editorValue);
            con.AddItem("FingersDelete", delValue);

            // 发送配对消息
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(this, MsgType.FingerEditorRequest, MsgType.FingerEditorRespond, con, TryFindResource("FingerEditorData").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                tbMsg.Text = TryFindResource("FingerEditorOverTime").ToString();
            }
            else if (msgBoxResult == MessageBoxResult.Yes)
            {              
                ModelResponsible.Instance.SendOperationLog("CollectFingerEditor");
                isClose = true;
                this.Close();
                NewMessageBox.ShowTip(TryFindResource("FingerEditorSuccess").ToString(), ModelResponsible.Instance.ParentWindow);
            }
            else if (msgBoxResult == MessageBoxResult.No)
            {
                tbMsg.Text = TryFindResource("FingerEditorFail").ToString();
            }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = string.Empty;
            this.Close();
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClose)
            {
                return;
            }

            string delValue = string.Empty;
            string editorValue = string.Empty;
            List<Finger> newFingers = GetNewFinger();

            GetChangeValue(ref delValue, ref editorValue, userInfo.Fingers, newFingers);
            if (!string.IsNullOrEmpty(delValue) || !string.IsNullOrEmpty(editorValue))
            {
                MessageBoxResult messageBoxResult = NewMessageBox.Show(TryFindResource("FingerCloseEditorWindow").ToString(), MessageBoxButton.YesNo, this);
                if (messageBoxResult != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private List<Finger> GetNewFinger()
        {
            List<Finger> fingers = new List<Finger>();
            foreach (var item in controls)
            {
                fingers.Add(new Finger() { FingerID = item.ID, FingerName = item.Text });
            }

            return fingers;
        }

        private void GetChangeValue(ref string delValue, ref string editorValue, List<Finger> oldFingers, List<Finger> newFingers)
        {
            foreach (var item in oldFingers)
            {
                Finger newFinger = newFingers.Find(p => p.FingerID.Equals(item.FingerID));
                if (newFinger == null)
                {
                    delValue += "," + item.FingerID;
                }
                else if (!newFinger.FingerName.Equals(item.FingerName))
                {
                    editorValue += string.Format(",{0}:{1}", item.FingerID, newFinger.FingerName);
                }
            }

            if (!string.IsNullOrEmpty(delValue))
            {
                delValue = delValue.Substring(1);
            }

            if (!string.IsNullOrEmpty(editorValue))
            {
                editorValue = editorValue.Substring(1);
            }
        }


    }
}
