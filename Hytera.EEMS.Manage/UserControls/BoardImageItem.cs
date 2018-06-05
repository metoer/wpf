using System;
using System.Windows;
using System.Windows.Media;


namespace Hytera.EEMS.Manage.UserControls
{
    public class BoardImageItem
    {
        public System.Windows.Media.Imaging.BitmapImage BitmapImage { get; set; }

        public FrameworkElement FrameworkElement { get; set; }

        public bool IsSelect;


        /// <summary>
        /// 缩放百分比
        /// </summary>
        public double Percent
        {
            get
            {
                Matrix m = FrameworkElement.RenderTransform.Value;
                return m.M11;
            }
        }

        /// <summary>
        /// 变换矩阵
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                return FrameworkElement.RenderTransform.Value;
            }
        }

        /// <summary>
        /// 显示中间
        /// </summary>
        /// <param name="randomOffset">偏移量</param>
        public void ShowCenter(int randomOffset = 0)
        {
            FrameworkElement touchPad = this.FrameworkElement.Parent as FrameworkElement;

            Matrix m = FrameworkElement.RenderTransform.Value;

            Point startOffset = new System.Windows.Point(m.OffsetX, m.OffsetY);

            m.OffsetX = (touchPad.Width - FrameworkElement.Width * m.M11) / 2;
            m.OffsetY = (touchPad.Height - FrameworkElement.Height * m.M22) / 2;

            if (0 > m.OffsetX || m.OffsetX > touchPad.Width)
                m.OffsetX = startOffset.X;
            if (0 > m.OffsetY || m.OffsetY > +touchPad.Height)
                m.OffsetY = startOffset.Y;

            Random random = new Random();
            m.OffsetX += random.Next(-randomOffset, randomOffset);
            m.OffsetY += random.Next(-randomOffset, randomOffset);

            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
        }

        /// <summary>
        /// 放大比例
        /// </summary>
        /// <param name="percent">percent 值0-10 效果0-1000%</param>
        public void ShowMoreBig(double percent = 1.1)
        {
            Matrix m = FrameworkElement.RenderTransform.Value;

            //if (Math.Abs(m.M11) >= 2) return;

            m.ScaleAtPrepend(percent, percent,
                this.FrameworkElement.ActualWidth / 2, this.FrameworkElement.ActualHeight / 2);

            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
        }

        /// <summary>
        /// 缩小比例
        /// </summary>
        /// <param name="percent"></param>
        public void ShowMoreSmall(double percent = 0.9)
        {
            Matrix m = FrameworkElement.RenderTransform.Value;

            //if (Math.Abs(m.M11) <= 0.3) return;

            m.ScaleAtPrepend(percent, percent,
                this.FrameworkElement.ActualWidth / 2, this.FrameworkElement.ActualHeight / 2);

            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
        }

        /// <summary>
        /// 顺时针
        /// </summary>
        /// <param name="angel">度数</param>
        public void ShowCW(double angle = 90)
        {
            Matrix m = FrameworkElement.RenderTransform.Value;
            m.RotateAtPrepend(angle, this.FrameworkElement.ActualWidth / 2, this.FrameworkElement.ActualHeight / 2);
            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
            //this.ShowFit();
        }

        /// <summary>
        /// 逆时针
        /// </summary>
        /// <param name="angel">度数</param>
        public void ShowCWW(double angle = 90)
        {
            Matrix m = FrameworkElement.RenderTransform.Value;
            m.RotateAtPrepend(-angle, this.FrameworkElement.ActualWidth / 2, this.FrameworkElement.ActualHeight / 2);

            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
            //this.ShowFit();
        }

        /// <summary>
        /// 1:1
        /// </summary>
        public void ShowOriginal()
        {
            Matrix m = FrameworkElement.RenderTransform.Value;
            m.M11 = 1;
            m.M22 = 1;
            m.M12 = 0;
            m.M21 = 0;

            this.FrameworkElement.RenderTransform = new MatrixTransform(m);

            this.ShowCenter();
        }

        public void ShowFitFull(bool isAdd)
        {
            FrameworkElement touchPad = this.FrameworkElement.Parent as FrameworkElement;

            Matrix m = FrameworkElement.RenderTransform.Value;
            if (isAdd)
            {
                m.OffsetX += (System.Windows.SystemParameters.PrimaryScreenWidth - 955) / 2;
                m.OffsetY += (System.Windows.SystemParameters.PrimaryScreenHeight - 686) / 2;
                m.ScaleAtPrepend(System.Windows.SystemParameters.PrimaryScreenWidth / 955, System.Windows.SystemParameters.PrimaryScreenHeight / 686,
                this.FrameworkElement.ActualWidth / 2, this.FrameworkElement.ActualHeight / 2);
            }
            else
            {
                m.OffsetX -= (System.Windows.SystemParameters.PrimaryScreenWidth - 955) / 2;
                m.OffsetY -= (System.Windows.SystemParameters.PrimaryScreenHeight - 686) / 2;
                m.ScaleAtPrepend(955 / System.Windows.SystemParameters.PrimaryScreenWidth, 686 / System.Windows.SystemParameters.PrimaryScreenHeight,
                this.FrameworkElement.ActualWidth / 2, this.FrameworkElement.ActualHeight / 2);
            }
            
            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
        }

        /// <summary>
        /// 适应屏幕
        /// </summary>
        /// <param name="from">源图片大小</param>
        /// <param name="area">画面大小</param>
        /// <returns>等比例大小</returns>
        public void ShowFit()
        {
            FrameworkElement touchPad = this.FrameworkElement.Parent as FrameworkElement;

            Size from = new Size(this.BitmapImage.Width, this.BitmapImage.Height);
            Size area = new Size(touchPad.ActualWidth, touchPad.ActualHeight);
            Size result = new Size(from.Width, from.Height);

            if (from.Width > area.Width)
            {
                double d = area.Width / from.Width;
                result.Width = from.Width * d;
                result.Height = from.Height * d;
            }

            if (result.Height > area.Height)
            {
                double d = area.Height / result.Height;
                result.Height = result.Height * d;
                result.Width = result.Width * d;
            }

            Matrix m = FrameworkElement.RenderTransform.Value;

            if (result.Width < from.Width)
            {
                m.M11 = result.Width / from.Width;
                m.M22 = result.Height / from.Height;
            }

            m.M12 = 0;
            m.M21 = 0;

            this.FrameworkElement.RenderTransform = new MatrixTransform(m);
            this.ShowCenter(100);
        }
        
    }




}
