using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using System.Linq;
using Hytera.EEMS.Log;

namespace Hytera.EEMS.Common
{
    ///  
    /// 系统信息类 - 获取CPU、内存、磁盘、进程信息 
    ///  
    public class SystemInfo
    {
        private const int INTERNET_CONNECTION_MODEM = 1;

        private const int INTERNET_CONNECTION_LAN = 2;

        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref   int dwFlag, int dwReserved);

        static PerformanceCounter pcCpuLoad;

        static ManagementClass mos = new ManagementClass("Win32_OperatingSystem");

        /// <summary>
        /// 静态构造
        /// </summary>
        static SystemInfo()
        {
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.MachineName = ".";

            // 获取物理内存
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
        }

        /// <summary>
        /// 物理内存
        /// </summary>
        public static long PhysicalMemory
        {
            get;
            private set;
        }

        /// <summary>
        /// 可用内存
        /// </summary>
        public static long MemoryAvailable
        {
            get
            {
                long availablebytes = 0;

                foreach (ManagementObject mo in mos.GetInstances())
                {
                    if (mo["FreePhysicalMemory"] != null)
                    {
                        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    }
                }
                return availablebytes;
            }
        }

        /// <summary>
        /// 获取Cpu使用率
        /// </summary>
        public static float GetCpuLoad
        {
            get
            {
                return pcCpuLoad.NextValue();
            }
        }

        #region IP
        /// <summary>
        /// 主机名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// IP地址集合
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetLocalIPs()
        {
            List<IPAddress> ipList = new List<IPAddress>();
            try
            {
                string hostName = Dns.GetHostName();//本机名   
                IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;//会警告GetHostByName()已过期，我运行时且只返回了一个IPv4的地址   
                //System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   

                ipList = addressList.ToList();
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }

            return ipList;
        }

        /// <summary>
        /// 获取第一个能联网的
        /// </summary>
        /// <returns></returns>
        public static string GetFirstIP()
        {
            List<IPAddress> CableIP = new List<IPAddress>();
            List<IPAddress> WirelessIP = new List<IPAddress>();

            // 获取本机所有网卡对象
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                // 获取以太网卡网络接口信息
                IPInterfaceProperties ip = adapter.GetIPProperties();

                // 获取单播地址集
                UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ipadd in ipCollection)
                    {
                        //InterNetwork    IPV4地址      InterNetworkV6        IPV6地址
                        //Max            MAX 位址
                        if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            string address = ipadd.Address.ToString();
                            if (address.IndexOf("169.254") >= 0 || address.IndexOf("169.165") >= 0)
                            {
                                continue;
                            }

                            if (!GetNetConStatus(address))
                            {
                                continue;
                            }

                            // 判断是否为ipv4
                            switch (adapter.NetworkInterfaceType)
                            {
                                case NetworkInterfaceType.Ethernet:
                                    // 以太网卡
                                    CableIP.Add(ipadd.Address);
                                    break;

                                case NetworkInterfaceType.Wireless80211:
                                    //无线网卡
                                    WirelessIP.Add(ipadd.Address);
                                    break;
                            }
                        }
                    }
                }
            }

            if (CableIP != null && CableIP.Count > 0)
            {
                return CableIP[0].ToString();
            }
            else if (WirelessIP != null && WirelessIP.Count > 0)
            {
                return WirelessIP[0].ToString();
            }

            throw new Exception("获取IP异常");
        }

        /// <summary>
        /// 判断网络的连接状态
        /// </summary>
        /// <returns>
        /// 网络状态(1-->未联网;2-->采用调治解调器上网;3-->采用网卡上网)
        ///</returns>
        public static bool GetNetConStatus(string strNetAddress)
        {
            System.Int32 dwFlag = new int();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                //没有能连上互联网
                return false;
            }
            else if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
            {
                //采用调治解调器上网,需要进一步判断能否登录具体网站
                if (PingNetAddress(strNetAddress))
                {
                    //可以ping通给定的网址,网络OK
                    return true;
                }
                else
                {
                    //不可以ping通给定的网址,网络不OK
                    return false;
                }
            }

            else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
            {
                //采用网卡上网,需要进一步判断能否登录具体网站
                if (PingNetAddress(strNetAddress))
                {
                    //可以ping通给定的网址,网络OK
                    return true;
                }
                else
                {
                    //不可以ping通给定的网址,网络不OK
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// ping 具体的网址看能否ping通
        /// </summary>
        /// <param name="strNetAdd"></param>
        /// <returns></returns>
        private static bool PingNetAddress(string strNetAdd)
        {
            bool Flage = false;
            Ping ping = new Ping();
            try
            {
                PingReply pr = ping.Send(strNetAdd, 500);
                if (pr.Status == IPStatus.TimedOut)
                {
                    Flage = false;
                }
                if (pr.Status == IPStatus.Success)
                {
                    Flage = true;
                }
                else
                {
                    Flage = false;
                }
            }
            catch
            {
                Flage = false;
            }
            return Flage;
        }
        #endregion

        #region 盘符信息
        /// <summary>
        /// 指定盘符的大小，单位B
        /// </summary>
        /// <param name="hardDiskName"></param>
        /// <returns></returns>
        public static long GetHardDiskSpace(string hardDiskName)
        {

            long totalSize = new long();

            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            foreach (System.IO.DriveInfo drive in drives)
            {

                if (drive.Name == hardDiskName)
                {

                    totalSize = drive.TotalSize / (1024 * 1024 * 1024);

                }

            }

            return totalSize;
        }

        /// <summary>
        /// 指定盘符剩余大小，单位B
        /// </summary>
        /// <param name="hardDiskName"></param>
        /// <returns></returns>
        public static long GetHardDiskFreeSpace(string hardDiskName)
        {

            long freeSpace = new long();

            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            foreach (System.IO.DriveInfo drive in drives)
            {

                if (drive.Name == hardDiskName)
                {

                    freeSpace = drive.TotalFreeSpace / (1024 * 1024 * 1024);

                }

            }

            return freeSpace;

        }

        /// <summary>
        /// 内存单位转换
        /// </summary>
        /// <param name="value">单位B</param>
        /// <returns></returns>
        public static string GetMemoryUnit(long value)
        {
            string result = string.Empty;
            if (value < 1024.00)
                result = value.ToString("F2") + " Byte";
            else if (value >= 1024.00 && value < 1048576)
                result = (value / 1024.00).ToString("F2") + " K";
            else if (value >= 1048576 && value < 1073741824)
                result = (value / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (value >= 1073741824)
                result = (value / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";

            return result;
        }

        #endregion

        public static void StartProcess(string fileName, string paras)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = paras;
            process.Start();
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            // 设置结构体块容量
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            // 捕获的时间
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        // 获取键盘和鼠标没有操作的时间
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            // 捕获时间
            if (!GetLastInputInfo(ref vLastInputInfo))
                return 0;
            else
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }
    }
}
