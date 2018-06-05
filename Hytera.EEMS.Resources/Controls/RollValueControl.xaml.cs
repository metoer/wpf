using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Hytera.EEMS.Resources.Controls
{
    /// <summary>
    /// RollValueControl.xaml 的交互逻辑
    /// </summary>
    public partial class RollValueControl : UserControl
    {
        public event Action<bool> LeaveEvent = null;

        Storyboard storyboard = new Storyboard();

        DoubleAnimation animation = new DoubleAnimation();

        DoubleAnimation selectAnimation = new DoubleAnimation();

        double lastY = 0;

        double newUnSelectY = 0;

        double newSelectY = 0;

        /// <summary>
        /// 显示个数
        /// </summary>
        int displayNum = 5;

        bool isLoad = false;

        List<string> values = new List<string>();

        #region 鼠标滑轮
        /// <summary>
        /// 滑动后的新坐标，以及最后一次滑动的方向
        /// </summary>
        double newWheelHight1, newWheelHight2, lastDelt;

        /// <summary>
        /// 滑动的队列数值
        /// </summary>
        Queue<int> deltaList = new Queue<int>();

        // 是否正在滑动动画
        bool isStart = false;
        #endregion

        /// <summary>
        /// 选择值
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RollValueControl));

        /// <summary>
        /// 项高度
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(RollValueControl));

        /// <summary>
        /// 选择项的背景颜色
        /// </summary>
        public static readonly DependencyProperty SelectBorderBrushProperty = DependencyProperty.Register("SelectBorderBrush", typeof(Brush), typeof(RollValueControl), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        /// <summary>
        /// 选择项的font颜色
        /// </summary>
        public static readonly DependencyProperty SelectFontBrushProperty = DependencyProperty.Register("SelectFontBrush", typeof(Brush), typeof(RollValueControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// 字位置
        /// </summary>
        public static readonly DependencyProperty TextHorizontalAlignmentProperty = DependencyProperty.Register("TextHorizontalAlignment", typeof(HorizontalAlignment), typeof(RollValueControl), new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// TextMargin
        /// </summary>
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(RollValueControl));

        /// <summary>
        /// 字位置
        /// </summary>
        public static readonly DependencyProperty TextMarkHorizontalAlignmentProperty = DependencyProperty.Register("TextMarkHorizontalAlignment", typeof(HorizontalAlignment), typeof(RollValueControl), new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// TextMargin
        /// </summary>
        public static readonly DependencyProperty TextMarkMarginProperty = DependencyProperty.Register("TextMarkMargin", typeof(Thickness), typeof(RollValueControl));


        /// <summary>
        /// TextMark
        /// </summary>
        public static readonly DependencyProperty TextMarkProperty = DependencyProperty.Register("TextMark", typeof(string), typeof(RollValueControl));

        public RollValueControl()
        {
            InitializeComponent();
            Values = new List<string>();
            ItemHeight = 50;
        }

        /// <summary>
        /// TextMark
        /// </summary>
        public string TextMark
        {
            get
            {
                return (string)GetValue(TextMarkProperty);
            }
            set
            {
                SetValue(TextMarkProperty, value);
            }
        }

        /// <summary>
        /// TextMargin
        /// </summary>
        public Thickness TextMargin
        {
            get
            {
                return (Thickness)GetValue(TextMarginProperty);
            }
            set
            {
                SetValue(TextMarginProperty, value);
            }
        }

        /// <summary>
        /// 字位置
        /// </summary>
        public HorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty);
            }
            set
            {
                SetValue(TextHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// TextMargin
        /// </summary>
        public Thickness TextMarkMargin
        {
            get
            {
                return (Thickness)GetValue(TextMarkMarginProperty);
            }
            set
            {
                SetValue(TextMarkMarginProperty, value);
            }
        }

        /// <summary>
        /// 字位置
        /// </summary>
        public HorizontalAlignment TextMarkHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(TextMarkHorizontalAlignmentProperty);
            }
            set
            {
                SetValue(TextMarkHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// 选择项的背景颜色
        /// </summary>
        public Brush SelectBorderBrush
        {
            get
            {
                return (Brush)GetValue(SelectBorderBrushProperty);
            }
            set
            {
                SetValue(SelectBorderBrushProperty, value);
            }
        }

        /// <summary>
        /// 选择项的font颜色
        /// </summary>
        public Brush SelectFontBrush
        {
            get
            {
                return (Brush)GetValue(SelectFontBrushProperty);
            }
            set
            {
                SetValue(SelectFontBrushProperty, value);
            }
        }

        /// <summary>
        /// 项高度
        /// </summary>
        public double ItemHeight
        {
            get
            {
                return (double)GetValue(ItemHeightProperty);
            }
            set
            {
                SetValue(ItemHeightProperty, value);
            }
        }

        /// <summary>
        /// 设置项
        /// </summary>
        public List<string> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                if (isLoad)
                {
                    // 重新加载
                    stackPanelMain.Children.Clear();
                    Init();
                    SetTop();
                }
            }
        }

        /// <summary>
        /// 当前选中的值
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
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            isLoad = true;

            Init();

            SetTop();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            unSelectPanel.Children.Clear();
            selectPanel.Children.Clear();
            // 加3层
            for (int i = 0; i < 3; i++)
            {
                foreach (string value in Values)
                {
                    TextBlock unSelectTextBlock = CreateItem(value, unSelectPanel);

                    unSelectTextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding("Foreground") { Source = this });

                    TextBlock selectTextBlock = CreateItem(value, selectPanel);

                    selectTextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding("SelectFontBrush") { Source = this });
                }
            }

            double allHeight = Values.Count * ItemHeight * 3;

            // 设置滚动总高度
            stackPanelMain.Height = displayNum * ItemHeight;

            unSelectPanel.Height = allHeight;

            selectPanel.Height = allHeight;

            canvasMain.Height = displayNum * ItemHeight;
        }

        /// <summary>
        /// 设置初始位置
        /// </summary>
        private void SetTop()
        {
            if (string.IsNullOrEmpty(Text))
            {
                Canvas.SetTop(unSelectPanel, -Values.Count * ItemHeight);
                Canvas.SetTop(selectPanel, (-Values.Count - 2) * ItemHeight);
                GetText();
            }
            else
            {
                int index = Values.IndexOf(Text);
                if (index >= 0)
                {
                    Canvas.SetTop(unSelectPanel, -(index - 2 + Values.Count) * ItemHeight);
                    Canvas.SetTop(selectPanel, -(index + Values.Count) * ItemHeight);
                }
            }
        }

        /// <summary>
        /// 创建子项
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private TextBlock CreateItem(string value, Panel stackPanel)
        {
            Grid grid = new Grid();

            grid.Height = ItemHeight;

            TextBlock textBlock = new TextBlock()
            {
                Text = value,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = SelectFontBrush
            };

            textBlock.SetBinding(TextBlock.HorizontalAlignmentProperty, new Binding("TextHorizontalAlignment") { Source = this });
            textBlock.SetBinding(TextBlock.MarginProperty, new Binding("TextMargin") { Source = this });

            grid.Children.Add(textBlock);
            stackPanel.Children.Add(grid);
            return textBlock;
        }

        #region 鼠标事件
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stackPanelMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stackPanelMain.CaptureMouse();
            if (storyboard != null)
            {
                storyboard.Stop(this);
                storyboard = null;
            }

            deltaList.Clear();

            isStart = false;

            System.Windows.Point position = e.GetPosition(canvasMain);
            lastY = position.Y;
            e.Handled = true;
        }

        /// <summary>
        /// 鼠标松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stackPanelMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stackPanelMain.ReleaseMouseCapture();
            CountPoint();
            e.Handled = true;
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stackPanelMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (!stackPanelMain.IsMouseCaptured) return;

            System.Windows.Point position = e.GetPosition(canvasMain);

            double newUnSelectTop = Canvas.GetTop(unSelectPanel) + (position.Y - lastY);
            newUnSelectTop = -Math.Abs(newUnSelectTop % (ItemHeight * Values.Count)) - ItemHeight * Values.Count;

            double newSelectTop = Canvas.GetTop(selectPanel) + (position.Y - lastY);
            newSelectTop = -Math.Abs(newSelectTop % (ItemHeight * Values.Count)) - ItemHeight * Values.Count;

            Canvas.SetTop(selectPanel, newSelectTop);
            Canvas.SetTop(unSelectPanel, newUnSelectTop);
            lastY = position.Y;
            GetText();
            e.Handled = true;
        }

        /// <summary>
        /// 停止滑动后重新计算位置
        /// </summary>
        private void CountPoint()
        {
            storyboard = new Storyboard();

            double nowUnSelectTop = Canvas.GetTop(unSelectPanel);

            double nowSelectTop = Canvas.GetTop(selectPanel);

            // 多滚动的距离
            double moreValue = Math.Abs(nowUnSelectTop % ItemHeight);

            if (moreValue < ItemHeight / 2)
            {
                newUnSelectY = nowUnSelectTop + moreValue;
                newSelectY = nowSelectTop + moreValue;
            }
            else
            {
                newUnSelectY = nowUnSelectTop - (ItemHeight - moreValue);
                newSelectY = nowSelectTop - (ItemHeight - moreValue);
            }

            animation = CreateDoubleAnimation(nowUnSelectTop, newUnSelectY, unSelectPanel, 200);

            selectAnimation = CreateDoubleAnimation(nowSelectTop, newSelectY, selectPanel, 200);

            storyboard.Children.Add(animation);

            storyboard.Children.Add(selectAnimation);

            storyboard.Completed += storyboard1_Completed;

            storyboard.Begin(this, true);
        }

        private DoubleAnimation CreateDoubleAnimation(double from, double to, FrameworkElement element, int seconds)
        {
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = from;

            animation.To = to;

            animation.Duration = new Duration(TimeSpan.FromMilliseconds(seconds));

            Storyboard.SetTargetName(animation, element.Name);

            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.TopProperty));

            return animation;
        }

        /// <summary>
        /// 动画结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storyboard1_Completed(object sender, EventArgs e)
        {
            if (storyboard != null)
                storyboard.Stop(this);

            storyboard = null;

            animation = null;

            selectAnimation = null;

            double countHeight = ItemHeight * Values.Count;

            Canvas.SetTop(unSelectPanel, -Math.Abs(newUnSelectY % countHeight - countHeight));

            Canvas.SetTop(selectPanel, -Math.Abs(newSelectY % countHeight - countHeight));
            GetText();

            if (LeaveEvent != null)
                LeaveEvent(true);

            isStart = false;
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stackPanelMain_MouseLeave(object sender, MouseEventArgs e)
        {
            stackPanelMain.ReleaseMouseCapture();
        }
        #endregion

        /// <summary>
        /// 设置值
        /// </summary>
        private void GetText()
        {
            if (ItemHeight > 0 && Values.Count > 0)
            {
                double value = Canvas.GetTop(unSelectPanel);
                Text = Values[((int)Math.Abs(value / ItemHeight) + 2) % Values.Count];
            }
        }

        private void stackPanelMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            deltaList.Enqueue(e.Delta);
            if (!isStart)
            {
                isStart = true;
                Start();
            }
        }

        private void Start()
        {
            int value = deltaList.Dequeue();

            storyboard = new Storyboard();

            newWheelHight1 = Canvas.GetTop(unSelectPanel) + value / 120 * ItemHeight;

            newWheelHight1 = -Math.Abs(newWheelHight1 % (ItemHeight * Values.Count)) - ItemHeight * Values.Count;

            newWheelHight2 = Canvas.GetTop(selectPanel) + value / 120 * ItemHeight;

            newWheelHight2 = -Math.Abs(newWheelHight2 % (ItemHeight * Values.Count)) - ItemHeight * Values.Count;

            animation = CreateDoubleAnimation(Canvas.GetTop(unSelectPanel), newWheelHight1, unSelectPanel, 100);

            selectAnimation = CreateDoubleAnimation(Canvas.GetTop(selectPanel), newWheelHight2, selectPanel, 100);

            storyboard.Children.Add(animation);

            storyboard.Children.Add(selectAnimation);

            storyboard.Completed += storyboard_Completed;

            storyboard.Begin(this, true);
        }


        /// <summary>
        /// 动画结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storyboard_Completed(object sender, EventArgs e)
        {
            if (storyboard != null)
                storyboard.Stop(this);

            storyboard = null;

            animation = null;

            selectAnimation = null;

            WheelNewPoint();
            if (deltaList.Count > 0)
            {
                GetText();
                Thread.Sleep(1);
                Start();
            }
            else
            {
                CountPoint();
                isStart = false;
            }
        }

        private void WheelNewPoint()
        {
            double countHeight = ItemHeight * Values.Count;

            Canvas.SetTop(unSelectPanel, newWheelHight1);

            Canvas.SetTop(selectPanel, newWheelHight2);
        }
    }
}
