using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Hytera.EEMS.Common
{
    public class FileHelper
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 检测文件是否被占用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsOccupy(string filePath)
        {
            Boolean result = false;
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)//文件被占用
            {
                result = true;
            }
            else
            {
                result = false;
            }
            CloseHandle(vHandle); //关闭_lopen

            return result;
        }

        /// <summary>  
        /// 计算文件大小函数(保留两位小数),Size为字节大小  
        /// </summary>  
        /// <param name="Size">初始文件大小</param>  
        /// <returns></returns>  
        public static string CountSize(long Size)
        {
            string m_strSize = string.Empty;
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
            {
                m_strSize = FactSize.ToString("F2") + " Byte";
            }
            else if (FactSize >= 1024.00 && FactSize < 1048576)
            {
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            }
            else if (FactSize >= 1048576 && FactSize < 1073741824)
            {
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            }
            else if (FactSize >= 1073741824)
            {
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            }

            return m_strSize;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 打开文件所在文件夹
        /// </summary>
        /// <param name="filePath"></param>
        public static bool OpenDirFile(string filePath)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (File.Exists(filePath))
                {
                    Process.Start("Explorer", "/select," + filePath);
                }
                else
                {
                    Process.Start("Explorer", "/open," + dir);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="filePath"></param>
        public static bool OpenDir(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                Process.Start("Explorer", "/open," + dir);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateDir(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    return true;
                }

                Directory.CreateDirectory(path);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文件去重名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FileNameDisTinct(string fileName)
        {
            while (File.Exists(fileName))
            {
                Match match = Regex.Match(fileName, "^(.*)[[\\(]([0-9]*)[\\)]]{0,1}(\\..*)$");
                if (match.Success)
                {
                    fileName = match.Groups[1].ToString() + "(" + (Convert.ToInt32(match.Groups[2]) + 1) + ")" + match.Groups[3].ToString();
                }
                else
                {
                    fileName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + "(1)" + Path.GetExtension(fileName);
                }
            }

            return fileName;
        }

        /// <summary>
        /// 读取TXT内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReadTxt(string url)
        {
            string str = string.Empty;
            try
            {
                FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                str = sr.ReadToEnd();
                fs.Close();
                sr.Close();
            }
            catch (Exception)
            {
            }
            return str;
        }

        /// <summary>
        /// 更新TXT内容
        /// </summary>
        /// <param name="fileFullPath">文件名</param>
        /// <param name="str">内容</param>
        /// <param name="isCover">覆盖(true),追加(false)</param>
        /// <returns></returns>
        public static bool UpdateTxt(string fileFullPath, string str, bool isCover = true)
        {
            bool isOk = true;
            try
            {
                string path = Path.GetDirectoryName(fileFullPath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs = null;
                StreamWriter sw = null;

                if (isCover)
                {
                    fs = new FileStream((fileFullPath), FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs);
                }
                else
                {
                    sw = File.AppendText((fileFullPath));
                }
                sw.WriteLine(str);

                if (sw != null) sw.Close();
                if (fs != null) fs.Close();

            }
            catch (Exception)
            {
                isOk = false;
            }

            return isOk;
        }
    }
}
