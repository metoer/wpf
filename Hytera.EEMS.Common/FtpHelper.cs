using System;
using System.IO;
using System.Net;

namespace Hytera.EEMS.Common
{
    public class FtpHelper
    {
        /// <summary>
        /// 进度
        /// </summary>
        public delegate void FtpProgressEventHandler(long length, long offset, decimal percent);

        /// <summary>
        /// FTP上传
        /// </summary>
        /// <param name="fileFullname">文件地址</param>
        /// <param name="ftpServerIP">ftp ip</param>
        /// <param name="ftpUserID">ftp用户名</param>
        /// <param name="ftpPassword">ftp密码</param>
        /// <param name="ftpProgress">回调方法</param>
        /// <returns>结果是否成功</returns>
        public static bool UploadFtp(string fileFullname, string ftpServerIP, string ftpUserID, string ftpPassword, FtpProgressEventHandler ftpProgress)
        {

            FileInfo fileInf = new FileInfo(fileFullname);

            long contentLength = fileInf.Length;
            long offset = 0;

            string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;

            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));
            try
            {
                // Provide the WebPermission Credintials 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // By default KeepAlive is true, where the control connection is not closed 
                // after a command is executed. 
                reqFTP.KeepAlive = false;

                // Specify the command to be executed. 
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // Specify the data transfer type. 
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file 
                reqFTP.ContentLength = contentLength;

                // The buffer size is set to 2kb 
                int buffLength = 1024 * 5;
                byte[] buff = new byte[buffLength];
                int contentLen;

                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded 
                //FileStream fs = fileInf.OpenRead(); 
                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Stream to which the file to be upload is written 
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time 
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends 
                while (contentLen != 0)
                {
                    offset += contentLen;
                    // Write Content from the file stream to the FTP Upload Stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                    if (ftpProgress != null)
                    {
                        ftpProgress(contentLength, offset, Math.Round((decimal)(offset * 100 / contentLength), 4));
                    }
                }

                // Close the file stream and the Request Stream 
                strm.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                reqFTP.Abort();
                return false;
            }
        }

        /// <summary>
        /// FTP下载
        /// </summary>
        /// <param name="filePath">下载路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="ftpServerIP">ftp地址</param>
        /// <param name="ftpUserID">ftp用户名</param>
        /// <param name="ftpPassword">ftp密码</param>
        /// <returns></returns>
        public static bool DownloadFtp(string filePath, string fileName, string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            FtpWebRequest reqFTP;
            try
            {
                //filePath = < <The full path where the file is to be created.>>, 
                //fileName = < <Name of the file to be created(Need not be the name of the file on FTP server).>> 
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception)
            {
                // Logging.WriteError(ex.Message + ex.StackTrace);
                // System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
