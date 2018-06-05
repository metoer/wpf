using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Write
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 属性、变量
        private double _WidthTouchKeyboard = 605;

        public double WidthTouchKeyboard
        {
            get { return _WidthTouchKeyboard; }
            set { _WidthTouchKeyboard = value; }
        }

        protected static bool ShiftFlag
        {
            get;
            set;
        }

        protected static bool CtrlFlag
        {
            get;
            set;
        }

        [DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIdex);

        [DllImport("User32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32.dll")]
        public static extern void keybd_event(byte bVK, byte bScan, Int32 dwFlags, int dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private const int WH_KEYBOARD_LL = 13; //键盘

        //键盘处理事件委托.
        private delegate int HookHandle(int nCode, int wParam, IntPtr lParam);

        //客户端键盘处理事件
        public delegate void ProcessKeyHandle(HookStruct param, out bool handle);

        //接收SetWindowsHookEx返回值
        private static int _hHookValue = 0;

        //勾子程序处理事件
        private HookHandle _KeyBoardHookProcedure;

        //设置钩子
        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookHandle lpfn, IntPtr hInstance, int threadId);

        //取消钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        //调用下一个钩子
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        //获取当前线程ID
        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        //Gets the main module for the associated process.
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string name);

        private IntPtr _hookWindowPtr = IntPtr.Zero;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Hook结构
        [StructLayout(LayoutKind.Sequential)]
        public class HookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        /// <summary>
        /// 是否开大小写
        /// </summary>
        public static bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x14] == 1);
            }
        }

        /// <summary>
        /// 是否按得软键盘
        /// </summary>
        bool isKeyboard = false;
        #endregion

        /// <summary>
        /// 加载窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            CmdCapsLock.Tag = CapsLockStatus.ToString();

            IntPtr a = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            int temp = GetWindowLong(a, -20);
            SetWindowLong(a, -20, temp | 0x08000000);
            HwndSource.FromHwnd(a).AddHook(new HwndSourceHook(WndProc));
            double height = System.Windows.SystemParameters.PrimaryScreenHeight;
            double width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = (width - 990) / 2;
            this.Top = height - 350;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.ShowInTaskbar = false;
        }

        /// <summary>
        /// 鼠标按下移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        /// <summary>
        /// 消息监听
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0216)
            {
                System.Drawing.Rectangle rect = (System.Drawing.Rectangle)Marshal.PtrToStructure(lParam, typeof(System.Drawing.Rectangle));
                this.Left = rect.Left;
                this.Top = rect.Top;
            }

            return IntPtr.Zero;
        }
  
        /// <summary>
        /// 软键盘按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button keybtn = sender as System.Windows.Controls.Button;
            #region//First Row
            if (keybtn.Name == "CmdTlide")
            {
                addNumkeyINput(0xc0);
            }
            else if (keybtn.Name == "cmd1")
            {
                addNumkeyINput(0x31);
            }
            else if (keybtn.Name == "cmd2")
            {
                addNumkeyINput(0x32);
            }
            else if (keybtn.Name == "cmd3")
            {
                addNumkeyINput(0x33);
            }
            else if (keybtn.Name == "cmd4")
            {
                addNumkeyINput(0x34);
            }
            else if (keybtn.Name == "cmd5")
            {
                addNumkeyINput(0x35);
            }
            else if (keybtn.Name == "cmd6")
            {
                addNumkeyINput(0x36);

            }
            else if (keybtn.Name == "cmd7")
            {
                addNumkeyINput(0x37);
            }
            else if (keybtn.Name == "cmd8")
            {
                addNumkeyINput(0x38);
            }
            else if (keybtn.Name == "cmd9")
            {
                addNumkeyINput(0x39);
            }
            else if (keybtn.Name == "cmd0")
            {
                addNumkeyINput(0x30);

            }
            else if (keybtn.Name == "cmdminus")//-_
            {
                addNumkeyINput(0xbd);
            }
            else if (keybtn.Name == "cmdplus")//+=
            {
                addNumkeyINput(0xbb);// 0x6b
            }
            else if (keybtn.Name == "cmdBackspace")//backspace
            {
                AddKeyBoardINput(0x08);
            }
            #endregion
            #region//Second Row
            else if (keybtn.Name == "CmdTab")
            {
                AddKeyBoardINput(0x09);
            }
            else if (keybtn.Name == "CmdQ")
            {
                AddKeyBoardINput(0x51);
            }
            else if (keybtn.Name == "Cmdw")
            {
                AddKeyBoardINput(0x57);

            }
            else if (keybtn.Name == "CmdE")
            {
                AddKeyBoardINput(0X45);

            }
            else if (keybtn.Name == "CmdR")
            {
                AddKeyBoardINput(0X52);

            }
            else if (keybtn.Name == "CmdT")
            {
                AddKeyBoardINput(0X54);

            }
            else if (keybtn.Name == "CmdY")
            {
                AddKeyBoardINput(0X59);

            }
            else if (keybtn.Name == "CmdU")
            {
                AddKeyBoardINput(0X55);

            }
            else if (keybtn.Name == "CmdI")
            {
                AddKeyBoardINput(0X49);

            }
            else if (keybtn.Name == "CmdO")
            {
                AddKeyBoardINput(0X4F);
            }
            else if (keybtn.Name == "CmdP")
            {
                AddKeyBoardINput(0X50);
            }
            else if (keybtn.Name == "CmdOpenCrulyBrace")
            {
                addNumkeyINput(0xdb);
            }
            else if (keybtn.Name == "CmdEndCrultBrace")
            {
                addNumkeyINput(0xdd);
            }
            else if (keybtn.Name == "CmdOR")
            {
                addNumkeyINput(0xdc);
            }
            #endregion
            #region///Third ROw

            else if (keybtn.Name == "CmdCapsLock")//caps lock
            {
                isKeyboard = true;
                CmdCapsLock.Tag = (!Convert.ToBoolean(CmdCapsLock.Tag)).ToString();

                AddKeyBoardINput(0x14);
            }
            else if (keybtn.Name == "CmdA")
            {
                AddKeyBoardINput(0x41);
            }
            else if (keybtn.Name == "CmdS")
            {
                AddKeyBoardINput(0x53);
            }
            else if (keybtn.Name == "CmdD")
            {
                AddKeyBoardINput(0x44);
            }
            else if (keybtn.Name == "CmdF")
            {
                AddKeyBoardINput(0x46);
            }
            else if (keybtn.Name == "CmdG")
            {
                AddKeyBoardINput(0x47);
            }
            else if (keybtn.Name == "CmdH")
            {
                AddKeyBoardINput(0x48);
            }
            else if (keybtn.Name == "CmdJ")
            {
                AddKeyBoardINput(0x4A);
            }
            else if (keybtn.Name == "CmdK")
            {
                AddKeyBoardINput(0X4B);
            }
            else if (keybtn.Name == "CmdL")
            {
                AddKeyBoardINput(0X4C);

            }
            else if (keybtn.Name == "CmdColon")//;:
            {
                addNumkeyINput(0xba);
            }
            else if (keybtn.Name == "CmdDoubleInvertedComma")//'"
            {
                addNumkeyINput(0xde);
            }
            else if (keybtn.Name == "CmdEnter")
            {
                AddKeyBoardINput(0x0d);
            }
            #endregion
            #region//Fourth Row
            else if (keybtn.Name == "CmdShift" || keybtn.Name == "CmdlShift")
            {
                if (CtrlFlag)
                {
                    CtrlFlag = false;
                    ShiftFlag = false;
                    changeInput();
                }
                else
                {
                    ShiftFlag = !ShiftFlag;
                    CmdShift.Tag = CmdlShift.Tag = ShiftFlag.ToString();
                }
            }
            else if (keybtn.Name == "CmdZ")
            {

                AddKeyBoardINput(0X5A);

            }
            else if (keybtn.Name == "CmdX")
            {
                AddKeyBoardINput(0X58);

            }
            else if (keybtn.Name == "CmdC")
            {
                AddKeyBoardINput(0X43);

            }
            else if (keybtn.Name == "CmdV")
            {
                AddKeyBoardINput(0X56);

            }
            else if (keybtn.Name == "CmdB")
            {
                AddKeyBoardINput(0X42);

            }
            else if (keybtn.Name == "CmdN")
            {
                AddKeyBoardINput(0x4E);

            }
            else if (keybtn.Name == "CmdM")
            {
                AddKeyBoardINput(0x4D);
            }
            else if (keybtn.Name == "CmdLessThan")//<,
            {
                addNumkeyINput(0xbc);
            }
            else if (keybtn.Name == "CmdGreaterThan")//>.
            {
                addNumkeyINput(0xbe);
            }
            else if (keybtn.Name == "CmdQuestion")//?/
            {
                addNumkeyINput(0xbf);
            }

            else if (keybtn.Name == "CmdSpaceBar")
            {
                AddKeyBoardINput(0x20);
            }
            #endregion
            #region//Last row
            else if (keybtn.Name == "CmdCtrl" || keybtn.Name == "CmdlCtrl")//ctrl
            {
                if (ShiftFlag)
                {
                    ShiftFlag = false;
                    CtrlFlag = false;
                }
                else
                {
                    CtrlFlag = true;
                }
            }
            else if (keybtn.Name == "CmdpageUp")
            {
                AddKeyBoardINput(0x21);
            }
            else if (keybtn.Name == "CmdpageDown")
            {
                AddKeyBoardINput(0x22);
            }
            else if (keybtn.Name == "CmdClose")//关闭键盘
            {
                this.Close();
            }
            #endregion

            CmdShift.Tag = ShiftFlag.ToString();
            CmdlShift.Tag = ShiftFlag.ToString();
            CmdCtrl.Tag = CtrlFlag.ToString();
            CmdlCtrl.Tag = CtrlFlag.ToString();
        }

        /// <summary>
        /// 切换输入法
        /// </summary>
        private void changeInput()
        {
            keybd_event(0x11, 0, 0, 0);
            keybd_event(0x10, 0, 0, 0);
            keybd_event(0x11, 0, 0x02, 0);
            keybd_event(0x10, 0, 0x02, 0);
        }

        private static void addNumkeyINput(byte input)
        {
            if (CtrlFlag)
            {
                CtrlFlag = false;
                ShiftFlag = false;
                keybd_event(input, 0, 0, 0);
                keybd_event(input, 0, 0x02, 0);
            }
            else
            {
                if (!ShiftFlag)
                {
                    keybd_event(input, 0, 0, 0);
                    keybd_event(input, 0, 0x02, 0);
                }
                else
                {
                    keybd_event(0x10, 0, 0, 0);//shift
                    keybd_event(input, 0, 0, 0);
                    keybd_event(input, 0, 0x02, 0);
                    keybd_event(0x10, 0, 0x02, 0);

                    ShiftFlag = false;
                }
            }
        }

        private static void AddKeyBoardINput(byte input)
        {

            if (CtrlFlag)
            {
                CtrlFlag = false;
                ShiftFlag = false;
                keybd_event(input, 0, 0, 0);
                keybd_event(input, 0, 0x02, 0);
            }
            else
            {
                if (ShiftFlag)
                {
                    keybd_event(0x10, 0, 0, 0);
                    keybd_event(input, 0, 0, 0);
                    keybd_event(input, 0, 0x02, 0);
                    keybd_event(0x10, 0, 0x02, 0);
                    ShiftFlag = false;
                }
                else
                {
                    keybd_event(input, 0, 0, 0);
                    keybd_event(input, 0, 0x02, 0);
                }
            }

        }

        private void cmd1_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));

            InstallHook();
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        public void InstallHook()
        {

            // 安装键盘钩子
            if (_hHookValue == 0)
            {
                _KeyBoardHookProcedure = new HookHandle(GetHookProc);

                _hookWindowPtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);

                _hHookValue = SetWindowsHookEx(
                                WH_KEYBOARD_LL,
                                _KeyBoardHookProcedure,
                                _hookWindowPtr,
                                0);

                //如果设置钩子失败.
                if (_hHookValue == 0)

                    UninstallHook();
            }
        }

        /// <summary>
        /// 取消钩子事件
        /// </summary>
        public void UninstallHook()
        {
            if (_hHookValue != 0)
            {
                bool ret = UnhookWindowsHookEx(_hHookValue);
                if (ret) _hHookValue = 0;
            }
        }

        /// <summary>
        /// 钩子事件内部调用,调用_clientMethod方法转发到客户端应用
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int GetHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //转换结构
                HookStruct hookStruct = (HookStruct)Marshal.PtrToStructure(lParam, typeof(HookStruct));

                //调用客户提供的事件处理程序。
                KeyPress(hookStruct);
            }

            return CallNextHookEx(_hHookValue, nCode, wParam, lParam);
        }

        int KeyTime = 0;

        /// <summary>
        /// 按键监听
        /// </summary>
        /// <param name="hookStruct"></param>
        public void KeyPress(HookStruct hookStruct)
        {          
            KeyTime++;
            if (KeyTime != 2)
            {
                return;
            }         

            KeyTime = 0;

            if (isKeyboard)
            {
                isKeyboard = false;
                return;
            }

            switch (hookStruct.vkCode)
            {
                case 20:
                    CmdCapsLock.Tag = (!Convert.ToBoolean(CmdCapsLock.Tag)).ToString();
                    break;
                case 160:

                    break;
            }
        }
    }

    public struct GUITHREADINFO
    {
        public int cbSize;
        public int flags;
        public int hwndActive;
        public int hwndFocus;
        public int hwndCapture;
        public int hwndMenuOwner;
        public int hwndMoveSize;
        public int hwndCaret;
        public System.Drawing.Rectangle rcCaret;
    }
}
