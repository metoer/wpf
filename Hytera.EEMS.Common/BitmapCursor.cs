using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace Hytera.EEMS.Common
{
    /// <summary>
    /// 创建鼠标样式
    /// </summary>
    public class BitmapCursor : SafeHandle
    {
        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        public override bool IsInvalid
        {
            get
            {
                // test
                return handle == (IntPtr)(-1);
            }
        }


        public static Cursor CreateBmpCursor(Bitmap cursorBitmap)
        {
            var c = new BitmapCursor(cursorBitmap);

            return CursorInteropHelper.Create(c);
        }

        protected BitmapCursor(Bitmap cursorBitmap)
            : base((IntPtr)(-1), true)
        {
            handle = cursorBitmap.GetHicon();
        }


        protected override bool ReleaseHandle()
        {
            bool result = DestroyIcon(handle);

            handle = (IntPtr)(-1);

            return result;
        }

        public static Cursor CreateCrossCursor()
        {
            const int w = 25;
            const int h = 25;

            var bmp = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.Default;
            g.InterpolationMode = InterpolationMode.High;
            var pen = new System.Drawing.Pen(Brushes.Black, 2);
            g.DrawLine(pen, new Point(12, 0), new Point(12, 8)); // vertical line 
            g.DrawLine(pen, new Point(12, 17), new Point(12, 25)); // vertical line
            g.DrawLine(pen, new Point(0, 12), new Point(8, 12)); // horizontal line 
            g.DrawLine(pen, new Point(16, 12), new Point(24, 12)); // horizontal line
            g.DrawLine(pen, new Point(12, 12), new Point(12, 13)); // Middle dot 
            g.Flush();
            g.Dispose();
            pen.Dispose();
            var c = CreateBmpCursor(bmp);
            bmp.Dispose();
            return c;
        }

        public static Cursor CreateCrossCursor(Brush brush, int radius)
        {
            int w = 20 + radius * 2;
            int p = w / 2;
            int h = 6;

            var bmp = new Bitmap(w, w);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.Default;
            g.InterpolationMode = InterpolationMode.High;

            var pen = new System.Drawing.Pen(Brushes.Black, 2);
            g.DrawLine(pen, new Point(p, 0), new Point(p, h)); // vertical line 
            g.DrawLine(pen, new Point(p, w - h), new Point(p, w)); // vertical line
            g.DrawLine(pen, new Point(0, p), new Point(h, p)); // horizontal line 
            g.DrawLine(pen, new Point(w - h, p), new Point(w, p)); // horizontal line

            g.FillEllipse(brush, p - radius, p - radius, radius * 2, radius * 2); // Middle dot 
            g.Flush();
            g.Dispose();
            pen.Dispose();

            var c = CreateBmpCursor(bmp);

            bmp.Dispose();

            return c;
        }
    }
}
