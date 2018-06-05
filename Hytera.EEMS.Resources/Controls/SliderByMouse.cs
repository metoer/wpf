using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace Hytera.EEMS.Resources.Controls
{
    public class SliderByMouse : Slider
    {
        const int WM_LBUTTONDOWN = 0x0201;

        const int WM_LBUTTONUP = 0x0202;

        public event MouseButtonEventHandler NewMouseLeftDown;

        public SliderByMouse()
        {
            IsMoveToPointEnabled = true;
            this.Loaded += SliderByMouse_Loaded;
        }

        private void SliderByMouse_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            if (source != null)
            {
                source.AddHook(WindowProc);
            }
        }

        /// <summary>
        /// 消息过滤监视
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_LBUTTONDOWN:
                    Point point = Mouse.GetPosition(this);
                    if (point.X > 0 && point.Y > 0 && point.X < this.ActualWidth && point.Y < this.ActualHeight && NewMouseLeftDown != null)
                    {
                        NewMouseLeftDown.BeginInvoke(this, null, null, null);
                    }
                    break;

                case WM_LBUTTONUP:



                    break;
            }

            return (IntPtr)0;
        }
    }
}
