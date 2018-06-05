using System;
using System.Windows;

namespace Hytera.EEMS.Resources.Windows
{
    public class NewMessageBox
    {
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageBoxText">消息内容</param>
        /// <returns></returns>
        public static MessageBoxResult Show(string messageBoxText, Window Owner = null)
        {
            MessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new MessageBoxWindow(messageBoxText);
                window.Owner = Owner;
                window.ShowDialog();
            }));

            return window.MessageBoxResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageBoxText">消息内容</param>
        /// <returns></returns>
        public static MessageBoxResult ShowTip(string messageBoxText, Window Owner = null, int millisecond = 500)
        {
            MessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new MessageBoxWindow(messageBoxText, millisecond);
                window.Owner = Owner;
                window.Show();
                window.Activate();
                window.Focus();
            }));

            return window.MessageBoxResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        public static MessageBoxResult Show(string messageBoxText, string caption, Window Owner = null)
        {
            MessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new MessageBoxWindow(messageBoxText, caption);
                window.Owner = Owner;
                window.ShowDialog();
            }));

            return window.MessageBoxResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageBoxText">消息内容</param>
        /// <param name="MessageBoxResult">消息框按钮</param>
        public static MessageBoxResult Show(string messageBoxText, MessageBoxButton button, Window Owner = null)
        {
            MessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new MessageBoxWindow(messageBoxText, button);
                window.Owner = Owner;
                window.ShowDialog();
            }));

            return window.MessageBoxResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        /// <param name="button">消息框按钮</param>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, Window Owner = null)
        {
            MessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new MessageBoxWindow(messageBoxText, caption, button);
                window.Owner = Owner;
                window.ShowDialog();
            }));

            return window.MessageBoxResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        /// <param name="button">消息框按钮</param>
        public static MessageBoxResult Show(string messageBoxText, MessageBoxButton button, string content1, Window Owner = null)
        {
            MessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new MessageBoxWindow(messageBoxText, button, content1);
                window.Owner = Owner;
                window.ShowDialog();
            }));

            return window.MessageBoxResult;
        }
    }
}
