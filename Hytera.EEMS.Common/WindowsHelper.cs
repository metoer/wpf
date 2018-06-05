using System;
using System.Windows;
using System.Windows.Media;

namespace Hytera.EEMS.Common
{
    public class WindowsHelper
    {
        /// <summary>
        /// 获取window对象
        /// </summary>
        /// <typeparam name="T">window</typeparam>
        /// <returns>window对象</returns>
        public static T GetWindow<T>() where T : Window
        {
            Application app = Application.Current;
            object win = null;
            app.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window window in app.Windows)
                {
                    if (window is T)
                    {
                        win = window;
                        break;
                    }
                }
            }));

            return (T)win;
        }

        public static Window GetWindowByTypeName(string className)
        {
            Application app = Application.Current;
            object win = null;
            app.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window window in app.Windows)
                {
                    if (window.GetType().Name.Equals(className))
                    {
                        win = window;
                        break;
                    }
                }
            }));

            return (Window)win;
        }

        /// <summary>
        /// 获取或者创建新窗口对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isNewOne"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T GetOrNewWindow<T>(bool isNewOne, params object[] param) where T : Window
        {
            Application app = Application.Current;
            object win = null;
            app.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window window in app.Windows)
                {
                    if (window is T)
                    {
                        win = window;
                        break;
                    }
                }
            }));

            if (win == null && isNewOne)
            {
                win = NewWindow<T>(param);
            }

            return (T)win;
        }

        /// <summary>
        /// 创建新窗口对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T NewWindow<T>(params object[] param) where T : Window
        {
            object win = Activator.CreateInstance(typeof(T), param);
            return (T)win;
        }

        /// <summary>
        /// 窗口显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ShowWindow<T>(Window owner = null, params object[] param) where T : Window
        {
            T window = GetOrNewWindow<T>(true, param);
            window.Owner = owner;
            window.Activate();
            window.Show();
            window.Focus();
            return (T)window;
        }

        /// <summary>
        /// 窗口显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ShowNewWindow<T>(Window owner = null, params object[] param) where T : Window
        {
            T window = NewWindow<T>(param);
            window.Owner = owner;
            window.Activate();
            window.Show();
            window.Focus();
            return (T)window;
        }

        /// <summary>
        /// 模态窗口显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ShowDialogWindow<T>(Window owner = null, params object[] param) where T : Window
        {
            T window = GetOrNewWindow<T>(true, param);
            window.Owner = owner;
            window.Activate();
            window.ShowDialog();
            window.Focus();      
            return (T)window;
        }

        /// <summary>
        /// 获取屏幕宽度
        /// </summary>
        /// <param name="isHaveTaskbar">包含任务栏</param>
        public static double GetScreenWidth(bool isHaveTaskbar = false)
        {
            return isHaveTaskbar ? System.Windows.SystemParameters.PrimaryScreenWidth : SystemParameters.WorkArea.Width;
        }

        /// <summary>
        /// 获取屏幕高度
        /// </summary>
        /// <param name="isHaveTaskbar">包含任务栏</param>
        public static double GetScreenHeight(bool isHaveTaskbar = false)
        {
            return isHaveTaskbar ? System.Windows.SystemParameters.PrimaryScreenHeight : SystemParameters.WorkArea.Height;
        }

        /// <summary>
        /// 获取左坐标
        /// </summary>
        /// <param name="window">当前窗口</param>
        /// <param name="rightPosition">右坐标</param>
        /// <returns></returns>
        public static double GetLeftByRightPosition(Window window, double rightPosition)
        {
            double left = GetScreenWidth() - (window.Width + rightPosition);
            return left;
        }

        /// <summary>
        /// 计算字体长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontFamily"></param>
        /// <returns></returns>
        public static double MeasureTextWidth(string text, double fontSize, string fontFamily)
        {
            FormattedText formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface(fontFamily.ToString()),
            fontSize,
            Brushes.Black
            );
            return formattedText.WidthIncludingTrailingWhitespace;
        }
    }
}
