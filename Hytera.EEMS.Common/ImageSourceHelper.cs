using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hytera.EEMS.Common
{
    public static class ImageSourceHelper
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Bitmap转换为ImageSource
        /// </summary>
        /// <param name="bitmap">图片</param>
        /// <returns>ImageSource</returns>
        public static ImageSource GetImageSourceByBitmap(this System.Drawing.Bitmap bitmap)
        {
            Debug.Assert(bitmap != null);

            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                                                                            hBitmap,
                                                                            IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hBitmap))//记得要进行内存释放。否则会有内存不足的报错。
            {
                throw new System.ComponentModel.Win32Exception();
            }

            return wpfBitmap;
        }

        /// <summary>
        /// ICO文件转换为ImageSource
        /// </summary>
        /// <param name="icoPath"></param>
        /// <returns></returns>
        public static ImageSource GetImageSourceByIcoPath(string icoPath)
        {
            if (!File.Exists(icoPath))
            {
                throw new FileNotFoundException();
            }

            Icon icon = new Icon(icoPath);

            Bitmap bitmap = icon.ToBitmap();

            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                                                                            hBitmap,
                                                                            IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }

        /// <summary>
        /// ico转换为ImageSource
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(this Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                                                                            hBitmap,
                                                                            IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
        
        /// <summary>
        /// Url转换为ImageSource
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ImageSource GetImageSource(Uri url)
        {
            BitmapImage bitmapImage = new BitmapImage(url);
            return bitmapImage;
        }

        /// <summary>
        /// 图片文件路径转换为ImageSource
        /// </summary>
        /// <param name="imgFilePath"></param>
        /// <returns></returns>
        public static ImageSource GetImageSource(string imgFilePath)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imgFilePath, UriKind.RelativeOrAbsolute));
            return bitmapImage;
        }


        /// <summary> 
        /// 按照指定大小缩放图片 
        /// </summary> 
        /// <param name="srcImage"></param> 
        /// <param name="iWidth"></param> 
        /// <param name="iHeight"></param> 
        /// <returns></returns> 
        public static Bitmap SizeImage(this Image srcImage, int iWidth, int iHeight)
        {
            try
            {
                if (srcImage == null)
                {
                    return null;
                }

                // 要保存到的图片 
                Bitmap b = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 按照图片大小等比例缩放
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Bitmap SizeImage_1(this Image srcImage, double scale)
        {
            try
            {
                if (srcImage == null)
                {
                    return null;
                }
                int iWidth = (int)(srcImage.Width * scale);
                int iHeight = (int)(srcImage.Height * scale);
                // 要保存到的图片 
                Bitmap b = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 图片加图片水印
        /// </summary>
        /// <param name="sourceBmp"></param>
        /// <param name="copyImage"></param>
        /// <returns></returns>
        public static Bitmap WaterMarkForBmp(Bitmap sourceBmp, Image copyImage)
        {
            Graphics g = Graphics.FromImage(sourceBmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(copyImage, new Rectangle(sourceBmp.Width - copyImage.Width, sourceBmp.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            g.Dispose();
            return sourceBmp;
        }

        /// <summary>
        /// 图片加文字水印
        /// </summary>
        /// <param name="sourceBmp"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bitmap WaterMarkForText(Bitmap sourceBmp, string value)
        {
            Graphics g = Graphics.FromImage(sourceBmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // g.DrawImage(sourceBmp, 0, 0, sourceBmp.Width, sourceBmp.Height);
            Font f = new Font("Verdana", 32);
            System.Drawing.Point p = new System.Drawing.Point(sourceBmp.Width - 300, sourceBmp.Height - 100);

            // 阴影
            System.Drawing.Brush TransparentBrush0 = new SolidBrush(System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black));
            System.Drawing.Brush TransparentBrush1 = new SolidBrush(System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black));
            g.DrawString(value, f, TransparentBrush0, p.X, p.Y + 1);
            g.DrawString(value, f, TransparentBrush0, p.X + 1, p.Y);

            System.Drawing.Brush b = new SolidBrush(System.Drawing.Color.White);
            string addText = value;
            g.DrawString(addText, f, b, p);
            g.Dispose();

            return sourceBmp;
        }


        /// <summary>
        /// 会产生graphics异常的PixelFormat
        /// </summary>
        private static System.Drawing.Imaging.PixelFormat[] indexedPixelFormats = { System.Drawing.Imaging.PixelFormat.Undefined, System.Drawing.Imaging.PixelFormat.DontCare,
            System.Drawing.Imaging.PixelFormat.Format16bppArgb1555, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, System.Drawing.Imaging.PixelFormat.Format4bppIndexed,
            System.Drawing.Imaging.PixelFormat.Format8bppIndexed
        };


        private static bool IsPixelFormatIndexed(System.Drawing.Imaging.PixelFormat imgPixelFormat)
        {
            foreach (System.Drawing.Imaging.PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }


        /// <summary>
        /// 图片的克隆
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <returns></returns>
        public static Bitmap BitmapClone(this Bitmap sourceBitmap)
        {
            Bitmap bmp = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);
            if (IsPixelFormatIndexed(sourceBitmap.PixelFormat))
            {
                bmp = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(bmp, 0, 0);
                }
            }
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(sourceBitmap, 0, 0, sourceBitmap.Width, sourceBitmap.Height);
                graphics.Dispose();
                GC.Collect();
            }
            return bmp;
        }

        /// <summary>
        /// 将Bitmap图片转换成byte字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();

            bmp.Save(ms, ImageFormat.Bmp);

            ms.Close();

            return ms.ToArray();
        }

        /// <summary>
        /// 将byte字节数组转换成Bitmap图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);

            Bitmap bmp = new Bitmap(ms);

            ms.Close();

            return bmp;
        }

        /// <summary>
        /// 将BitmapImage图片转换成byte字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] BitmapImageToBytes(BitmapImage bmp)
        {
            byte[] bytes = null;

            Stream s = bmp.StreamSource;

            s.Position = 0; //很重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0。 

            using (BinaryReader br = new BinaryReader(s))
            {
                bytes = br.ReadBytes((int)s.Length);
            }

            return bytes;
        }

        /// <summary>
        /// 将byte字节数组转换成BitmapImage图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BitmapImage BytesToBitmapImage(byte[] bytes)
        {
            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();

            bitmapImage.StreamSource = new MemoryStream(bytes);

            bitmapImage.EndInit();

            return bitmapImage;
        }

        /// <summary>
        /// 将Bitmap图片转换成BitmapImage图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(this Bitmap bitmap)
        {
            return BytesToBitmapImage(BitmapToBytes(bitmap));
        }

        /// <summary>
        /// 将BitmapImage图片转换成Bitmap图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap BitmapImageToBitmap(this BitmapImage bitmapImage)
        {
            return BytesToBitmap(BitmapImageToBytes(bitmapImage));
        }
    }
}
