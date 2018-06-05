using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Hytera.EEMS.Manage.UserControls
{
    /// <summary>
    /// BoardImageControl.xaml 的交互逻辑
    /// </summary>
    public partial class BoardImageControl : UserControl
    {
        #region 变量
        /// <summary>
        /// 原始数据
        /// </summary>
        public List<BoardImageItem> BoardImageList { get; private set; }

        /// <summary>
        /// 默认线条颜色
        /// </summary>
        public System.Windows.Media.Color DefaultColor = Colors.OliveDrab;

        /// <summary>
        /// 选择项线条颜色
        /// </summary>
        public System.Windows.Media.Color SelectColor = Colors.Red;

        /// <summary>
        /// 默认线条大小
        /// </summary>
        public double DefaultThickness = 0;

        /// <summary>
        /// 选择时线条大小
        /// </summary>
        public double SelectThickness = 2;

        /// <summary>
        /// 允许删除手势
        /// </summary>
        public bool IsManipulationDelete = false;

        /// <summary>
        /// 允许自动选中
        /// </summary>
        public bool IsAutoSelect = false;

        /// <summary>
        /// 允许双击缩放
        /// </summary>
        public bool IsManipulationDoubleClick = false;

        /// <summary>
        /// zIndex
        /// </summary>
        private int zIndex = 0;

        /// <summary>
        /// 按下左边
        /// </summary>
        private System.Windows.Point startPoint;

        /// <summary>
        /// 按下偏移量
        /// </summary>
        private System.Windows.Point startOffset;

        /// <summary>
        /// 按下时间
        /// </summary>
        private DateTime startTime;

        private bool isChanged = false;
        public bool IsChanged
        {
            get
            {
                return isChanged;
            }
            set
            {
                isChanged = value;
            }
        }

        /// <summary>
        /// 双击次数
        /// </summary>
        private int doubleClickCount;

        /// <summary>
        /// 双击定时器
        /// </summary>
        private DispatcherTimer doubleClickTimer;

        /// <summary>
        /// 双击开始偏移量
        /// </summary>
        private System.Windows.Point doubleClickOffset;
        #endregion

        #region 事件

        public delegate void SelectChangedEventHandler(BoardImageItem item);

        public event SelectChangedEventHandler SelectChanged;

        private void OnSelectChanged()
        {
            if (SelectChanged == null)
            {
                return;
            }

            var item = this.GetSelectItem();
            SelectChanged(item);
        }

        #endregion

        public BoardImageControl()
        {
            BoardImageList = new List<BoardImageItem>();

            InitializeComponent();

            Loaded += new RoutedEventHandler(BoardImageControl_Loaded);

        }

        public void BoardImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            touchPad.MouseUp += new MouseButtonEventHandler(touchPad_MouseUp);
            touchPad.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(touchPad_ManipulationStarting);
            touchPad.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(touchPad_ManipulationDelta);
            touchPad.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(touchPad_ManipulationCompleted);
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="url"></param>
        public BoardImageItem Add(string fileName)
        {
            isChanged = true;
            Border border = new Border();

            ImageBrush imageBrush = new ImageBrush();

            FileStream img = new FileStream(fileName, FileMode.Open);
            byte[] imgbyte = new byte[img.Length];
            img.Read(imgbyte, 0, imgbyte.Length);
            img.Close();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = new MemoryStream(imgbyte);
            bitmap.EndInit();

            imageBrush.ImageSource = bitmap;
            imageBrush.Stretch = Stretch.UniformToFill;

            border.Background = imageBrush;
            border.Width = bitmap.Width;
            border.Height = bitmap.Height;
            border.IsManipulationEnabled = true;

            //动态事件
            border.MouseDown += new MouseButtonEventHandler(border_MouseDown);
            border.MouseMove += new MouseEventHandler(border_MouseMove);
            border.MouseUp += new MouseButtonEventHandler(border_MouseUp);
            border.MouseWheel += new MouseWheelEventHandler(border_MouseWheel);

            //返回值
            BoardImageItem item = new BoardImageItem();
            item.BitmapImage = bitmap;
            item.FrameworkElement = border;

            this.BoardImageList.Add(item);
            this.touchPad.Children.Add(border);
            this.Select(item);

            item.ShowFit();

            if (BoardImageList.Count == 1)
            {
                item.ShowCenter();
            }

            return item;
        }

        public void ShowFitFull(bool isAdd)
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowFitFull(isAdd);
            }
        }

        public void ShowFit()
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowFit();
            }
        }

        public void ShowMoreBig()
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowMoreBig();
            }
        }

        public void ShowMoreSmall()
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowMoreSmall();
            }
        }

        public void ShowCW()
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowCW();
            }
        }

        public void ShowCWW()
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowCWW();
            }
        }

        public void ShowOriginal()
        {
            isChanged = true;
            BoardImageItem item = GetSelectItem();
            if (item != null)
            {
                item.ShowOriginal();
            }
        }

        /// <summary>
        /// 检查手势操作（删除等）
        /// </summary>
        void CheckManipulationDelete()
        {
            if (!IsManipulationDelete) return;

            System.Windows.Point nowPoint = Mouse.GetPosition(this);
            TimeSpan timeSpan = DateTime.Now - startTime;
            System.Windows.Point pointSpan = new System.Windows.Point((nowPoint.X - startPoint.X), (nowPoint.Y - startPoint.Y));

            if (timeSpan.TotalMilliseconds < 150 && (Math.Abs(pointSpan.X) > 100 || Math.Abs(pointSpan.Y) > 100))
            {
                ThicknessAnimation da = new ThicknessAnimation();
                da.From = new Thickness();
                da.To = new Thickness((pointSpan.X * 5), (pointSpan.Y * 5), 0, 0);
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.AccelerationRatio = 1;


                Storyboard story = new Storyboard();
                story.Children.Add(da);
                story.Completed += new EventHandler((ss, ee) =>
                {
                    this.Remove(this.GetSelectItem());
                });

                Storyboard.SetTarget(da, this.GetSelectItem().FrameworkElement);
                Storyboard.SetTargetProperty(da, new PropertyPath(FrameworkElement.MarginProperty));

                story.Begin();
            }

            Console.WriteLine("CheckManipulation pointSpanMilliseconds({0},{1}) timeSpan={2}", pointSpan.X, pointSpan.Y, timeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// 检查手势操作，双击缩放
        /// </summary>
        void CheckManipulationDoubleClick()
        {
            if (IsManipulationDoubleClick == false || doubleClickTimer != null) return;

            doubleClickOffset = Mouse.GetPosition(this);

            doubleClickTimer = new DispatcherTimer();
            doubleClickTimer.Interval = TimeSpan.FromMilliseconds(200);
            doubleClickTimer.Tick += new EventHandler((sender, e) =>
            {
                System.Windows.Point nowOffset = Mouse.GetPosition(this);
                if (doubleClickCount > 1 && Math.Abs(nowOffset.X - doubleClickOffset.X) < 10 &&
                    Math.Abs(nowOffset.Y - doubleClickOffset.Y) < 10)
                {
                    var item = this.GetSelectItem();
                    if (item != null)
                    {
                        if (item.Matrix.Determinant >= 1)
                        {
                            item.ShowMoreSmall(0.3);
                        }
                        else
                        {
                            item.ShowOriginal();
                        }
                    }
                }
                doubleClickCount = 0;
                doubleClickTimer.Stop();
                doubleClickTimer = null;
            });
            doubleClickTimer.Start();
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="item"></param>
        public void Remove(BoardImageItem item)
        {
            if (item == null)
            {
                return;
            }

            this.BoardImageList.Remove(item);
            this.touchPad.Children.Remove(item.FrameworkElement);

            if (IsAutoSelect && (BoardImageList.Count > 0))
            {
                this.Select(BoardImageList.Last());
            }

            this.OnSelectChanged();
            isChanged = true;
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        public void RemoveAll()
        {
            isChanged = true;
            this.BoardImageList.Clear();
            this.touchPad.Children.Clear();

            this.OnSelectChanged();
        }

        /// <summary>
        /// 设置选中项
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isSelect"></param>
        public void Select(BoardImageItem item)
        {
            UnSelect();

            if (item != null)
            {
                Border border2 = item.FrameworkElement as Border;
                border2.BorderBrush = new SolidColorBrush(this.SelectColor);
                border2.BorderThickness = new Thickness(this.DefaultThickness);
                item.IsSelect = true;

                Canvas.SetZIndex(border2, this.zIndex++);
            }

            this.OnSelectChanged();
        }

        /// <summary>
        /// 移除选中项
        /// </summary>
        public void UnSelect()
        {
            var selectItem = (from a in BoardImageList
                              where a.IsSelect == true
                              select a).FirstOrDefault();
            if (selectItem != null)
            {
                Border border1 = selectItem.FrameworkElement as Border;
                border1.BorderBrush = new SolidColorBrush(this.DefaultColor);
                border1.BorderThickness = new Thickness(this.DefaultThickness);
                selectItem.IsSelect = false;
            }
        }

        public void OnUnSelect()
        {
            UnSelect();
            this.OnSelectChanged();
        }

        /// <summary>
        /// 设置选中项
        /// </summary>
        /// <param name="border"></param>
        public void Select(FrameworkElement border)
        {
            foreach (var item in BoardImageList)
            {
                if (item.FrameworkElement == border)
                {
                    this.Select(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <returns></returns>
        public BoardImageItem GetSelectItem()
        {
            var item = (from a in BoardImageList
                        where a.IsSelect == true
                        select a).FirstOrDefault();
            return item;
        }

        #region 鼠标事件
        void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement border = sender as FrameworkElement;

            Console.WriteLine("border_MouseDown:" + touchPad.Children.IndexOf(border));

            System.Windows.Media.Matrix m = border.RenderTransform.Value;

            startPoint = Mouse.GetPosition(this);
            startOffset = new System.Windows.Point(m.OffsetX, m.OffsetY);
            startTime = DateTime.Now;
            doubleClickCount++;

            border.Opacity = 0.95;
            border.CaptureMouse();

            this.Select(border);
            Console.WriteLine("border_MouseDown:({0},{1})", startPoint.X, startPoint.Y);

            e.Handled = true;
        }

        void border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement border = sender as FrameworkElement;

            Console.WriteLine("border_MouseUp:" + touchPad.Children.IndexOf(border));

            System.Windows.Media.Matrix m = border.RenderTransform.Value;

            border.Opacity = 1;
            border.ReleaseMouseCapture();

            //检查操作
            this.CheckManipulationDelete();
            this.CheckManipulationDoubleClick();

            e.Handled = true;
        }

        void border_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement border = sender as FrameworkElement;

            if (!border.IsMouseCaptured) return;
            
            System.Windows.Point nowPoint = Mouse.GetPosition(this);
            
            System.Windows.Media.Matrix m = border.RenderTransform.Value;
            m.OffsetX = startOffset.X + (nowPoint.X - startPoint.X);
            m.OffsetY = startOffset.Y + (nowPoint.Y - startPoint.Y);

            border.RenderTransform = new MatrixTransform(m);
            e.Handled = true;

            isChanged = true;
        }

        void border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            isChanged = true;
            BoardImageItem boardImageItem = GetSelectItem();
            if (boardImageItem == null)
            {
                return;
            }

            if (e.Delta > 0)
            {
                boardImageItem.ShowMoreBig();
            }
            else
            {
                boardImageItem.ShowMoreSmall();
            }
        }

        private void touchPad_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsAutoSelect)
            {
                this.Select((BoardImageItem)null);
            }
        }

        #endregion

        #region 多点触屏事件


        void touchPad_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            isChanged = true;

            FrameworkElement element = (FrameworkElement)e.Source;
            System.Windows.Media.Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;

            e.ManipulationContainer = touchPad;
            e.Mode = ManipulationModes.All;

            startPoint = Mouse.GetPosition(this);
            startOffset = new System.Windows.Point(matrix.OffsetX, matrix.OffsetY);
            startTime = DateTime.Now;
            doubleClickCount++;

            element.Opacity = 0.95;

            Console.WriteLine("touchPad_ManipulationStarting({0},{1})", startPoint.X, startPoint.Y);
            this.Select(element);
        }

        void touchPad_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            Console.WriteLine("touchPad_ManipulationCompleted");
            FrameworkElement element = (FrameworkElement)e.Source;
            element.Opacity = 1;

            //检查操作
            this.CheckManipulationDelete();
            this.CheckManipulationDoubleClick();
            isChanged = true;
        }

        void touchPad_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            isChanged = true;
            Console.WriteLine("touchPad_ManipulationDelta");

            FrameworkElement element = (FrameworkElement)e.Source;

            System.Windows.Media.Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;

            var deltaManipulation = e.DeltaManipulation;

            System.Windows.Point center = new System.Windows.Point(element.ActualWidth / 2, element.ActualHeight / 2);
            center = matrix.Transform(center);

            matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);

            matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);

            matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

            ((MatrixTransform)element.RenderTransform).Matrix = matrix;
        }
        #endregion
    }
}
