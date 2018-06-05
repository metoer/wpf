using log4net;
using log4net.Core;
using System;

namespace Hytera.EEMS.Common
{
    public class LogHelper
    {
        private static ILog _ILog;

        private static object lockOjbect = new object();

        public static void Log(string message)
        {
            InitConfig();

            _ILog.Info(message);
        }

        private static void InitConfig()
        {
            if (_ILog == null)
            {
                lock (lockOjbect)
                {
                    if (_ILog == null)
                    {
                        _ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                        log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
                        appender.File = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                        appender.AppendToFile = true;
                        appender.MaxSizeRollBackups = -1;
                        appender.MaximumFileSize = "1MB";
                        appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date;
                        //appender.DatePattern = "yyyyMMdd.txt";
                        appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
                        appender.Layout = new log4net.Layout.PatternLayout("%date  - %message%newline");
                        appender.ActivateOptions();
                        appender.Threshold = Level.Info;
                        log4net.Config.BasicConfigurator.Configure(appender);
                    }
                }
            }
        }

    }
}
