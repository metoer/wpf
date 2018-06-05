using Hytera.EEMS.Common;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hytera.EEMS.Resources.Controls
{
    /// <summary>
    /// 基础窗口
    /// </summary>
    public class BaseWindow : Window
    {
        private Thickness temp = new Thickness(0);

        bool writeShow = false;

        public BaseWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            //图片与字体清晰显示
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;

            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
            //RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            Icon = Properties.Resources.eems.ToImageSource();
            this.Loaded += BaseWindow_Loaded;
        }

        void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            writeShow = AppHelper.IsWriteShow();
            if (isShowWrite)
            {
                AppHelper.StartKey(this);               
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (this.WindowState != WindowState.Minimized)
            {

            }

            if (this.WindowState == WindowState.Maximized)
            {
                if (Content is Border)
                {
                    temp = (Content as Border).Margin;
                    (Content as Border).Margin = new Thickness(0);
                }
            }
            else
            {
                if (Content is Border)
                {
                    (Content as Border).Margin = temp;
                }
            }

            base.OnStateChanged(e);
        }

        #region 支持窗口移动

        private bool isDragMove;

        /// <summary>
        /// 是否支持窗口移动
        /// </summary>
        public bool IsDragMove
        {
            get
            {
                return isDragMove;
            }
            set
            {
                isDragMove = value;
                if (value)
                {
                    MouseLeftButtonDown += new MouseButtonEventHandler(BaseWindow_MouseLeftButtonDown);
                }
                else
                {
                    MouseLeftButtonDown -= new MouseButtonEventHandler(BaseWindow_MouseLeftButtonDown);
                }
            }
        }

        private bool isShowWrite;

        /// <summary>
        /// 是否显示输入法
        /// </summary>
        public bool IsShowWrite
        {
            get
            {
                return isShowWrite;
            }
            set
            {
                isShowWrite = value;
            }
        }

        private void BaseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    this.DragMove();
                }
                catch (Exception) { }
            }
        }
        #endregion

        #region 保持窗体在屏幕里面

        private bool isInScreen;

        /// <summary>
        /// 保持窗体在屏幕里面
        /// </summary>
        public bool IsInScreen
        {
            get
            {
                return isInScreen;
            }
            set
            {
                isInScreen = value;
                if (value)
                {
                    MouseUp += new MouseButtonEventHandler(Window_MouseUp);
                }
                else
                {
                    MouseUp -= new MouseButtonEventHandler(Window_MouseUp);
                }
            }
        }

        void Window_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Left < 0)
            {
                Left = 0;
            }
            else if (Left > (WindowsHelper.GetScreenWidth(true) - ActualWidth))
            {
                Left = WindowsHelper.GetLeftByRightPosition(this, 0);
            }
        }
        #endregion

        #region 纯颜色背景

        /// <summary>
        ///纯颜色背景
        /// </summary>
        public bool IsPureBackground
        {
            get
            {
                return OverridesDefaultStyle;
            }
            set
            {
                OverridesDefaultStyle = value;
            }
        }
        #endregion

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.DataContext = null;
            if (!writeShow)
            {
                AppHelper.CloseKey();
            }
        }

        /// <summary>
        /// 窗口正在关闭事件 可以挽回
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
